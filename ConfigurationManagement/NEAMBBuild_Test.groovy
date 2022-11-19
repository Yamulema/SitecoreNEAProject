import hudson.FilePath;
@Library('oshyn-groovy@develop') _

node {
	// global vars section (vars used in functions)
	deploy_assets = params.DeployAssets
	deploy_code = params.DeployCodeAndConfigs
	deploy_content = params.DeploySitecoreItems
	notifications=false   // change this to turn off notifications.  used for debugging the groovy process
    pwd = pwd()
	localgulpconfig= pwd + "\\local-config.json"
	config_dir = pwd + "\\ConfigurationManagement"

	def project_namespace = "Neamb.Project"
	def gulpbin = "C:\\Program Files\\nodejs\\custom_modules\\gulp"
	def code_dir=pwd+"\\src"
	def pub_dir="D:\\Projects\\neambc_web\\publish"  // this location needs to match what is in the checked in pubxml.  maybe we should generate the pubxml dynamically at some point ...
	def major_version_prefix = "1.0.1."
	  def buildPrefFile = config_dir + "\\BuildPrefix.txt"
//	  def major_version_prefix=getBuildPrefix(buildPrefFile)
	def chosen_nuspec_file = config_dir + "\\Nuspec\\NEAMB.Web.nuspec"
	def version = major_version_prefix + env.BUILD_NUMBER
	//def nuget_pkg_output = "NEAMB.Web." +  + ".nupkg"
	def nuget_pkg_output = "NEAMB.Web." + version + ".nupkg"
    def relnotes_file = config_dir + "\\Nuspec\\relnotes.txt"
	def features_dir = code_dir + "\\Feature"
	def foundations_dir = code_dir + "\\Foundation"
	def project_dir = code_dir + "\\Project"

	def releaseNumberSettingInsertFile = project_dir + 	"\\Common\\code\\App_Config\\Include\\Neamb.Project.Common\\z.Neamb.DevSettings.config"
	def releaseNumberSettingTokenToReplace="_LOCALDEVELOPMENT_"
	
	def build_env= params.Environment
	def octo_env = "Integration"
	
	def nugetRepoName="nuget/NEAMBC-Release/"
	def repoPath="repos/neambc_web"

	def configBuildType = "Release"
	def test_execution=params.Execute_Tests
//	def categoryFilter = params['Test Category']

	// test variables
	def testAssemblyContains=['.UnitTest.']  // list of assembly names that contain tests within the UITestFolderInNugetPkg
	def test_report_location = "TestResults"
	def testResultFileName="work\\TestResult.xml"
	def test_report_full_location=pwd + "\\" +test_report_location
	def testResultFileFull = test_report_full_location + "\\"+ testResultFileName

	//UI test project vars
	def uitest_solution = pwd + "\\Neambc.Neamb.UITests.sln"
	def uitest_project_namespace="Neambc.Neamb.Project.Web.UITest"
	def uitest_output_dir = code_dir + "\\UITests\\" + uitest_project_namespace + "\\bin\\" + configBuildType
	
	// notification variables
	def slackChannel="#build-notifications"
	def slackTeamDomain="oshynneamb"
	def allure_guid="412d6938-6764-471e-bf84-143294cb0182"
	def allure_report_name="NEAMB.com Unit Tests"

	try {

		notifyBuild buildStatus:"STARTED", channel:slackChannel, teamDomain: slackTeamDomain, send_notification:notifications

		properties([buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '20', daysToKeepStr: '', numToKeepStr: '100'))])

		stage('Checkout')
		{
			cleanWs deleteDirs: true
			checkout scm
			version = getBuildPrefix(buildPrefFile) + env.BUILD_NUMBER
			println "Composite build # is: " + version

			gitNeambBuildTag.setTag(version,repoPath)

		}
		
		stage('Dependency Resolution') 
		{				
			if(deploy_code){ 
				def restoreTasks = [:]
				
				restoreTasks["ProjectRestore"] = {
					nugetNeamb.restore "neambc.sln"
				}
				restoreTasks["UITestsRestores"] = {
					nugetNeamb.restore uitest_solution
				}
				restoreTasks["NodeRestores"] = {
					println "Doing node dependency download"
					bat "npm install || exit 0" 
				}
				parallel restoreTasks
			}
	
		}
		
		stage('Prebuild Clean'){
			buildDotNetSolution.clean solutionFile:"neambc.sln"
		}
		
		stage('Build and Publish'){

				major_version_prefix=getBuildPrefix(buildPrefFile)
				version = major_version_prefix + env.BUILD_NUMBER
				nuget_pkg_output = "NEAMB.Web." + version + ".nupkg"
				
			
				def gulpLocalConfigHandle = new File(localgulpconfig)
				gulpLocalConfigHandle.delete();
				def configFileText=''
				configFileText += writeLocalConfigForPublish()  // write the local-config.json file so it uses the file system publish
				gulpLocalConfigHandle.write configFileText
				
				if(deploy_code){  
					//bat "\"${gulpbin}\" 03P-BuildAndPublish-All-Projects"
					
					def buildtasks =[:]
					
					buildtasks["ProjectSolution"] = {
						bat "\"${gulpbin}\" 03S-BuildAndPublish-All-Projects"
					}
					buildtasks["UITestSolution"] = {
						buildDotNetSolution solutionFile:uitest_solution,productVersion:version, deployOnBuild:false
					}
					parallel buildtasks
				}
				else {
					println "not asked to deploy code so i'm not spending time building it"
				}
		}

		stage ('Execute Unit Tests')
		{
			if (test_execution) {

				try {
						println "Running the Test"
						def subpath="\\code\\bin"
						def test_locations=[features_dir,foundations_dir,project_dir]
						def pub_dir_bins=pub_dir + "\\bin"
						discoverAndExecuteCoveredUnitTests test_assembly_paths:test_locations, test_assembly_filter:testAssemblyContains, test_assembly_subpath:subpath, configBuildType:null ,
							withCoverage: true, publishNunitResults:true,pwd:pwd,coberturaFilter:"Neambc.*",additionalPDBSearchDir:pub_dir_bins
					} catch (e) {
						println "Unit Test failed, Exception trace: " + e
						currentBuild.result = "UNSTABLE"
					}  // end of try catch
					
					println "About to call groovy lib for allure test"
					buildAllureReport allure_guid,pwd,test_report_location,testResultFileFull,allure_report_name

			}
		}
		
	
		stage('Package'){
				major_version_prefix=getBuildPrefix(buildPrefFile)
				version = major_version_prefix + env.BUILD_NUMBER
				nuget_pkg_output = "NEAMB.Web." + version + ".nupkg"

				//set the release number into an app setting
				setReleaseNumberToAppSetting(releaseNumberSettingInsertFile,releaseNumberSettingTokenToReplace,version)

				def nuspecHandle = new File(chosen_nuspec_file)
				def compiledNuspecText=''
				// construct the <file> tags for the dynamic nuspec file
				compiledNuspecText += createNuspecFileList(features_dir)
				compiledNuspecText += createNuspecFileList(foundations_dir)
				compiledNuspecText += createNuspecFileList(project_dir)
				compiledNuspecText += addAssetsNuspecFileList(project_dir)

				// add the web.config patch that is required
				// add the bin files from publish location
				if(deploy_code){
					compiledNuspecText <<= '<file src="'+pub_dir+'\\bin\\NEAMB*.dll" target="bin" exclude="**\\*.pdb;" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Seiu\\code\\Views\\Web\\Web.*config" target="Views" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Common\\code\\Web.*config" target="" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Common\\code\\xxx_app_offline.htm" target="" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Common\\code\\500.html" target="" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Seiu\\code\\500_seiumb.html" target="" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Neamb\\code\\500_neamb.html" target="" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Neamb\\code\\Rte\\ToolsFile_Neamb.xml" target="Rte" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Seiu\\code\\Rte\\ToolsFile_Seiumb.xml" target="Rte" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+project_dir+'\\Neamb\\code\\iframe-calculators\\*.aspx" target="iframe-calculators" exclude="" />\n'
					compiledNuspecText <<= '<file src="'+uitest_output_dir+'\\*.dll" target="_UITests" exclude="**\\.pdb;" />\n'
					compiledNuspecText <<= '<file src="'+uitest_output_dir+'\\*.xml" target="_UITests" exclude="**\\.pdb;" />\n'
					//compiledNuspecText <<= '<file src="'+uitest_output_dir+'\\*.exe" target="_UITests" exclude="**\\.pdb;" />\n'
				}
				// replace section of the template nuspec file with the files fields
			    def tmpSpec = nuspecHandle.text
				//println "file text to replace is" + nuspecText
				//println "current file text is: " + tmpSpec
				tokens = tmpSpec.split("__CONTENTS__")
				tmpSpec = tokens[0] + compiledNuspecText + tokens[1]
				//println "new file text is: " + tmpSpec
				nuspecHandle.write tmpSpec

				nugetNeamb.pack chosen_nuspec_file,version
				//bat "nuget pack \"${chosen_nuspec_file}\" -NoPackageAnalysis -OutputDirectory D:\\MyPackages -Properties version=${version}"
		}

		stage('Upload to Nuget Repo')
		{
				major_version_prefix=getBuildPrefix(buildPrefFile)
				version = major_version_prefix + env.BUILD_NUMBER
				nuget_pkg_output = "NEAMB.Web." + version + ".nupkg"

			println "File to push to Octopus is: " + nuget_pkg_output
			//bat "nuget push \"C:\\MyPackages\\${nuget_pkg_output}\" -ApiKey API-81KZX50SLNJ9GCMNIHE6FUPX3TW -Source https://octopus.mbctech.net/nuget/packages?replace=true"
			//bat "nuget push \"D:\\MyPackages\\${nuget_pkg_output}\" -ApiKey ${pkgRepoKey} -Source ${pkgRepoUrl}"
			def fullpkgpath="D:\\MyPackages\\"+nuget_pkg_output
			saveToNeambRepo fullpkgpath,nugetRepoName
		}
		
		stage('Create Release and Deploy')
		{
				major_version_prefix=getBuildPrefix(buildPrefFile)
				version = major_version_prefix + env.BUILD_NUMBER
				nuget_pkg_output = "NEAMB.Web." + version + ".nupkg"

			writeReleaseNotesFile(relnotes_file)

			println "Doing Build, Release and Deploy"
			// use this one to just create the release (not deploy
			//bat "octo create-release --project \"NEAMB.Web\" --version ${major_version_prefix}${env.BUILD_NUMBER} --packageversion ${major_version_prefix}${env.BUILD_NUMBER} --server https://octopus.mbctech.net --apiKey API-81KZX50SLNJ9GCMNIHE6FUPX3TW --releasenotesfile \"${relnotes_file}\" --deployto=\"${octo_env}\" --progress --deploymenttimeout=00:15:00"	
			
			createOctoRelease "NEAMB.Web", version, relnotes_file, octo_env
		}
		
		stage ('Cleanup') 
		{
				major_version_prefix=getBuildPrefix(buildPrefFile)
				version = major_version_prefix + env.BUILD_NUMBER
				nuget_pkg_output = "NEAMB.Web." + version + ".nupkg"

			def clean_tasks = [:]
				clean_tasks["nuget_pkg"] = {
					bat "del \"D:\\MyPackages\\${nuget_pkg_output}\""
				}
				
				clean_tasks["push_tag"] = {
					withCredentials([usernamePassword(credentialsId: "${repoJenkinsCreds}", passwordVariable: 'GIT_PASSWORD', usernameVariable: 'GIT_USERNAME')]) {
						bat "git push https://${GIT_USERNAME}:${GIT_PASSWORD}@${repoUrl} --tags"
					}
				}

				clean_tasks["pub_dir"] = {
					dir (pub_dir) {
						deleteDir()
					}
				}
			// execute cleanup tasks in parallel
			parallel clean_tasks
		}

	}
	catch(e){
		currentBuild.result = "FAILED"
    	throw e
	}
	finally{
		notifyBuild buildStatus:currentBuild.result, channel:slackChannel, teamDomain: slackTeamDomain, send_notification:notifications
	}	

}

def writeLocalConfigForPublish() {
	def configFileText="";
	configFileText <<= '{\n'
 	configFileText <<= '"publishProfile": "file",\n'
 	configFileText <<= '"publishProfileFile": "' + config_dir.replace("\\","\\\\")+ '\\\\PublishProfiles\\\\CustomProfile.pubxml"\n'
	configFileText <<= '}\n'
	//println "config file text is: " + configFileText;
	
	return configFileText;
}

def addAssetsNuspecFileList(String fileLocation) {
			def nuspecText=''
			dir (fileLocation) {
				projDirs = findFiles()
			}
			for (int i=0;i< projDirs.size();i++) {
					def prj_name=projDirs[i].getName()
					if (projDirs[i].isDirectory() && !prj_name.contains(".") && !prj_name.equals("Site")) {  //make sure no unwanted folders are used
					   // add assets folders
					   	def tmp_component_dir = prj_name + "\\code"
						def tmp_component_assets_dir=fileLocation + "\\" + tmp_component_dir + "\\assets"
						def asset_directory = new File(tmp_component_assets_dir)
						if(asset_directory.exists() && deploy_assets){
							nuspecText <<= '<file src="'+tmp_component_assets_dir+'\\**\\*" target="assets\\" exclude="" />\n'
						}

					}
			}
			return nuspecText
}

def createNuspecFileList(String fileLocation) {
			def nuspecText=''
			dir (fileLocation) {
				featuresDirs = findFiles()
			}
			def parentdir = fileLocation.substring(fileLocation.lastIndexOf('\\')+1,fileLocation.length());
			println "Component set to work on: " + parentdir;
			
			for (int i=0;i< featuresDirs.size();i++) {
					def ftr_name=featuresDirs[i].getName()
					if (featuresDirs[i].isDirectory() && !ftr_name.equals("Site")) {  //make sure no unwanted folders are used
			
						def tmp_component_dir = ftr_name + "\\code"
						def tmp_component_bin_dir=fileLocation + "\\" + tmp_component_dir + "\\bin"
						def tmp_component_views_dir=fileLocation + "\\" + tmp_component_dir + "\\Views"
						def tmp_component_config_dir=fileLocation + "\\" + tmp_component_dir + "\\App_Config"
						def tmp_component_yml_dir = fileLocation + "\\" + ftr_name + "\\serialization"
						
						println "Config files for feature: " + tmp_component_config_dir
						println "YML Files for feature: " + tmp_component_yml_dir	
						//println "Deploy Content: " + deploy_content	

						// add lines to nuspec for each feature
						// not sure if we need full path yet or if .\src will work fine.  need to check on the nuget package
						
						// add the bins for each feature
						def bin_directory = new File(tmp_component_bin_dir)
						//if(bin_directory.exists() && deploy_code){
						//	nuspecText <<= '<file src="'+tmp_component_bin_dir+'\\NEAMB*.dll" target="bin\\" exclude="**\\*.pdb;**\\Sitecore.*;**\\Microsoft.*" />\n'
						//}

						// add the configs for each feature
						def config_directory = new File(tmp_component_config_dir)
						if (config_directory.exists() && deploy_code) {
							nuspecText <<= '<file src="'+tmp_component_config_dir+'\\**\\*.config" target="App_Config_stg" exclude="**\\*.cs;" />\n'
						}

						// add the views for each feature
						def view_directory = new File(tmp_component_views_dir)
						if (view_directory.exists() && deploy_code) {
							//nuspecText <<= '<file src="'+tmp_component_views_dir+'\\**\\*.cshtml" target="Views\\'+ftr_name+'" exclude="**\\*.cs;" />\n'
							nuspecText <<= '<file src="'+tmp_component_views_dir+'\\**\\*.cshtml" target="Views\\" exclude="**\\*.cs;" />\n'
						}

						// add the ymls for each feature
						def yml_directory = new File(tmp_component_yml_dir)
						if (yml_directory.exists() && deploy_content) {
							nuspecText <<= '<file src="'+tmp_component_yml_dir+'\\**\\*.yml" target="yml_stg\\' + parentdir +'\\'+ftr_name+'\\serialization" exclude="**\\*.cs;" />\n'
						}
						
					}
			}

			return nuspecText
}
