import org.jenkinsci.plugins.workflow.steps.FlowInterruptedException
@Library('oshyn-groovy@develop') _

node {
	// global vars section (vars used in functions)
	deploy_assets = true
	deploy_code = true
	deploy_content = true
	def branchname=params.Branch
	def test_execution=params.Execute_Tests
//	def enableJetBrainsAnalysis=params.EnableJetBrainsAnalysis
	def enableJetBrainsAnalysis=false
	def enableAssemblyAnalysis=params.EnableAssemblyAnalysis
	//def enableAssemblyAnalysis=true
	def enableDependencyVulnerabilityAnalysis=params.EnableDependencyVulnerabilityAnalysis
	def clearWorkspace=params.ClearWorkspace

	// def enableAssemblyAnalysis=false
	// def enableDependencyVulnerabilityAnalysis=true
    def withSonarQubeAnalysis=params.EnableSonarAnalysis
	def build_env= params.Environment

	notifications=true   // change this to turn off notifications.  used for debugging the groovy process
    pwd = pwd()
	localgulpconfig= pwd + "\\local-config.json"
	config_dir = pwd + "\\ConfigurationManagement"

	def msbuild_version = "MSBuild-15.0"

	def project_namespace = "Neamb9.Project"
	def gulpbin = "C:\\Program Files\\nodejs\\custom_modules\\gulp"
	def code_dir=pwd+"\\src"
	def pub_dir="D:\\Projects\\neambc_web9\\publish"  // this location needs to match what is in the checked in pubxml.  maybe we should generate the pubxml dynamically at some point ...
	def major_version_prefix = "2.0.0."
    def buildPrefFile = config_dir + "\\BuildPrefix.txt"
	def chosen_nuspec_file = config_dir + "\\Nuspec\\NEAMB.Web.nuspec"
	def version = major_version_prefix + env.BUILD_NUMBER
	def nuget_pkg_prefix="NEAMB.Web."
	def nuget_pkg_output = nuget_pkg_prefix + version + ".nupkg"
    def relnotes_file = config_dir + "\\Nuspec\\relnotes.txt"
	def features_dir = code_dir + "\\Feature"
	def foundations_dir = code_dir + "\\Foundation"
	def project_dir = code_dir + "\\Project"

	def releaseNumberSettingInsertFile = project_dir + 	"\\Common\\code\\App_Config\\Include\\Neamb.Project.Common\\z.Neamb.DevSettings.config"
	def releaseNumberSettingTokenToReplace="_LOCALDEVELOPMENT_"
	
	def octo_env = null
	def nugetRepoName="nuget/NEAMBC-Release-SC9/"
	def repoPath="repos/neambc_web"

	def configBuildType = "Release"

	def main_solution = pwd + "\\neambc.sln"
	def main_profile = config_dir + "\\PublishProfiles\\LocalProfile.pubxml"

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
	def o365webhookurl="https://outlook.office.com/webhook/0a396dbf-bf21-4d36-85f5-8ca760d2daea@d0e42219-9317-449b-b4bd-f8c7080d940f/JenkinsCI/2196b1aed796426a900a4d7a05646e61/51c07164-57d8-4235-b350-e58f46fcc7ee"

	def allure_guid="412d6938-6764-471e-bf84-143294cb0182"
	def allure_report_name="NEAMB.com Unit Tests"
	def pkgOutputDir="D:\\MyPackages"
	
	def sonarProjectKey="NEAMB"
	def sonarLoginKey="914c4c23bbdf6d21972f213e668d7f1871177e46"



	
	try {

		notifyBuild buildStatus:"STARTED", channel:slackChannel, teamDomain: slackTeamDomain, send_notification:notifications,O365WebHookUrl:o365webhookurl

		properties([buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '20', daysToKeepStr: '', numToKeepStr: '100'))])

		stage ("Clean workspace") {
			if (clearWorkspace) {
				cleanWs deleteDirs: true
			}
		}

		stage('Checkout')
		{
			checkout scm
			version = getBuildPrefix(buildPrefFile) + env.BUILD_NUMBER
			println "Composite build # is: " + version
			withEnv(["BundlePath=com/neamb"]) {
				println "bundle path is: " + env.BundlePath
				gitOshynBuildTag.setTag(version,repoPath)
			}
		}
		
		stage('Dependency Resolution') 
		{				
				if(deploy_code){ 
					def restoreTasks = [:]
					
					restoreTasks["ProjectRestore"] = {
						//buildDotNetSolution.restore solutionFile:main_solution,MSBuild:msbuild_version
						nugetOshyn.restore "neambc.sln"
					}
					restoreTasks["UITestsRestores"] = {
						//buildDotNetSolution.restore solutionFile:uitest_solution,MSBuild:msbuild_version
						nugetOshyn.restore uitest_solution
					}
					restoreTasks["NodeRestores"] = {
						println "Doing node dependency download"
						bat "npm install || exit 0" 
					}
					withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
							parallel restoreTasks
					}
				} else {
					println "not asked to deploy code so i'm not spending time RESOLUTION it"
				}
	
		}
		
		stage('Prebuild Clean'){
			withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
				buildDotNetSolution.clean solutionFile:main_solution,MSBuild:msbuild_version
			}
			println "making a material change to the groovy"
		}
		
		stage('Build and Publish'){
			
				def gulpLocalConfigHandle = new File(localgulpconfig)
				gulpLocalConfigHandle.delete();
				def configFileText=''
				configFileText += writeLocalConfigForPublish()  // write the local-config.json file so it uses the file system publish
				gulpLocalConfigHandle.write configFileText
				
				if(deploy_code){
					def buildtasks =[:]
					def sonarAnalysisParameters=[]
					buildtasks["UITestSolution"] = {
						 buildDotNetSolution solutionFile:uitest_solution, productVersion:version, deployOnBuild:false,
						 withJetBrains:false,withAssAnalyzer:false,withDependencyVulnerabilityAnalysis:false,
						 withSonarQube:false,withTestExecution:false,MSBuild:msbuild_version				
					}
					buildtasks["ProjectSolution"] = {
						buildDotNetSolution solutionFile:main_solution, productVersion:version, deployOnBuild:true, publishProfile:main_profile,
						withJetBrains:enableJetBrainsAnalysis,withAssAnalyzer:enableAssemblyAnalysis,publishLocation:pub_dir,withDependencyVulnerabilityAnalysis:enableDependencyVulnerabilityAnalysis,
						withSonarQube:withSonarQubeAnalysis,sonarQubeProjectKey:sonarProjectKey,sonarQubeLoginKey:sonarLoginKey,withTestExecution:test_execution,
						sonarAnalysisParameters:sonarAnalysisParameters,MSBuild:msbuild_version
					}
					
					withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
					buildtasks.each{ key, value ->    value()}
						//parallel buildtasks
					}
				}
				else {
					println "not asked to deploy code so i'm not spending time BUILDING it"
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
					withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
						discoverAndExecuteCoveredUnitTests 	test_assembly_paths:test_locations,
															test_assembly_filter:testAssemblyContains,
															test_assembly_subpath:subpath, 
															configBuildType:configBuildType,
															withCoverage: test_execution, 
															publishNunitResults:test_execution, 
															pwd:pwd, 
															coberturaIncludeFilter:['Neambc.*'], 
															additionalPDBSearchDir:pub_dir_bins, 
															failOnTestError:true,
															sendReportsRemote:true,
															withJetBrains:enableJetBrainsAnalysis
					}
				} finally {
					withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
						buildAllureReportMap allure_guid:allure_guid,pwd:pwd,test_report_location:test_report_location,testResultFile:testResultFileFull,
						report_name:allure_report_name,branch:branchname,sendReportsRemote:true
					}
				}
			}
		}
	
		def parallelStages=[:]

		parallelStages["PostBuildAnalysis"] = {
				stage("Post Build Analysis") {
						// if (withSonarQubeAnalysis) {
            			//     oshynSonarQube.end sonarLoginKey:sonarLoginKey
			            // }

					withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
						def assemblyAnalysisFilter=["Neambc"]
						postBuildAnalysisTools solutionFile:main_solution,MSBuild: "MSBuild-16.0",
							withJetBrains:enableJetBrainsAnalysis,withAssAnalyzer:enableAssemblyAnalysis,publishLocation:pub_dir,withDependencyVulnerabilityAnalysis:enableDependencyVulnerabilityAnalysis,
							withSonarQube:withSonarQubeAnalysis,sonarQubeLoginKey:sonarLoginKey,assemblyAnalyzerFilter:assemblyAnalysisFilter
					}
				}
		}

		parallelStages["PackAndUpload"] ={

			stage('Package FILE'){
				//set the release number into an app setting
				version = getBuildPrefix(buildPrefFile) + env.BUILD_NUMBER
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
					compiledNuspecText <<= '<file src="'+uitest_output_dir+'\\**\\*.xml" target="_UITests" exclude="**\\.pdb;" />\n'
					compiledNuspecText <<= '<file src="'+uitest_output_dir+'\\**\\*.json" target="_UITests" exclude="**\\.pdb;" />\n'
					compiledNuspecText <<= '<file src="'+uitest_output_dir+'\\*.xml" target="_UITests" exclude="**\\.pdb;" />\n'
					compiledNuspecText <<= '<file src="'+uitest_output_dir+'\\Controls\\*.xml" target="_UITests\\Controls" exclude="**\\.pdb;" />\n'
				}
				// replace section of the template nuspec file with the files fields
				def tmpSpec = nuspecHandle.text
				tokens = tmpSpec.split("__CONTENTS__")
				tmpSpec = tokens[0] + compiledNuspecText + tokens[1]
				nuspecHandle.write tmpSpec
			}
			
		
			stage('Package CREATE'){
				withEnv(["BundlePath=com/neamb"]) {
					version = getBuildPrefix(buildPrefFile) + env.BUILD_NUMBER
					nugetOshyn.pack chosen_nuspec_file,version,pkgOutputDir
				}
			}

			stage('Upload to Nuget Repo')
			{
				version = getBuildPrefix(buildPrefFile) + env.BUILD_NUMBER
				nuget_pkg_output = nuget_pkg_prefix + version + ".nupkg"
				println "File to push to Octopus is: " + nuget_pkg_output
				def fullpkgpath=pkgOutputDir+"\\"+nuget_pkg_output
				withEnv(["BundlePath=com/neamb"]) {
					saveToOshynRepo fullpkgpath,nugetRepoName
				}
			}

			 stage('Create Release and Deploy')
			 {
				withEnv(["BundlePath=com/neamb"]) {
					version = getBuildPrefix(buildPrefFile) + env.BUILD_NUMBER
					writeReleaseNotesFile(relnotes_file)
					def deployTasks = [ : ]
					//deployTasks["neamb_orig_deploy"] = {
					//createOctoRelease "NEAMB.Web.SC9", version, relnotes_file, octo_env
					//}
					deployTasks["neamb_new_deploy"] = {
					createOctoRelease.channel "NEAMB.Web", version, relnotes_file, octo_env,"NEAMB-BLUE"
					}
					parallel deployTasks
				}
			 }
		}

		parallel(parallelStages)

		stage ('Cleanup') 
		{
			version = major_version_prefix + env.BUILD_NUMBER
			nuget_pkg_output = nuget_pkg_prefix + version + ".nupkg"
			cleanupTasks(nuget_pkg_output, pub_dir,repoPath)
		}

	} catch (FlowInterruptedException fie) {
		println "Job aborted ... "
		currentBuild.result = "ABORTED"
	} catch (Exception e) {
		println "Job failed with exception: " + e
		currentBuild.result = "FAILURE"
	} finally {
		notifyBuild buildStatus:currentBuild.result, channel:slackChannel, teamDomain: slackTeamDomain, send_notification:notifications,O365WebHookUrl:o365webhookurl
		major_version_prefix=getBuildPrefix(buildPrefFile)
		version = major_version_prefix + env.BUILD_NUMBER
		nuget_pkg_output = nuget_pkg_prefix + version + ".nupkg"
		cleanupTasks(nuget_pkg_output, pub_dir,repoPath)
		
		withEnv(["BundlePath=com/neamb"]) {
			sendBuildMetrics(sonarAuthToken:sonarLoginKey,influxDbName:"neamb-build",influxCredential:"influx-neamb")
		}
	}	

}

def cleanupTasks(nuget_pkg_output, pub_dir,repoPath) {
	def clean_tasks = [:]
	clean_tasks["nuget_pkg"] = {
		bat "del \"D:\\MyPackages\\${nuget_pkg_output}\""
	}
	
	clean_tasks["push_tag"] = {
			withEnv(["BundlePath=com/neamb"]) {
				println "bundle path is: " + env.BundlePath
				//gitOshynBuildTag.commitTag(repoPath)
			}
	}

	clean_tasks["pub_dir"] = {
		dir (pub_dir) {
			deleteDir()
		}
	}
	parallel clean_tasks
}

def writeLocalConfigForPublish() {
	def configFileText="";
	configFileText <<= '{\n'
 	configFileText <<= '"publishProfile": "file",\n'
 	configFileText <<= '"publishProfileFile": "' + config_dir.replace("\\","\\\\")+ '\\\\PublishProfiles\\\\CustomProfile.pubxml"\n'
	configFileText <<= '}\n'
	
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
	println "Component (PARENTDIR) set to work on: " + parentdir;
	println "Component (FILELOCATION) set to work on: " + fileLocation;
	
	for (int i=0;i< featuresDirs.size();i++) {
		def ftr_name=featuresDirs[i].getName()
		if (featuresDirs[i].isDirectory() && !ftr_name.equals("Site")) {  //make sure no unwanted folders are used
			def tmp_component_dir = ftr_name + "\\code"
			def tmp_component_views_dir=fileLocation + "\\" + tmp_component_dir + "\\Views"
			def tmp_component_config_dir=fileLocation + "\\" + tmp_component_dir + "\\App_Config"
			def tmp_component_yml_dir = fileLocation + "\\" + ftr_name + "\\serialization"
			
			println "Config files for feature: " + tmp_component_config_dir
			println "YML Files for feature: " + tmp_component_yml_dir	
			
			// add the configs for each feature
			def config_directory = new File(tmp_component_config_dir)
			if (config_directory.exists() && deploy_code) {
				nuspecText <<= '<file src="'+tmp_component_config_dir+'\\**\\*.config" target="App_Config_stg" exclude="**\\*.cs;" />\n'
			}

			// add the views for each feature
			def view_directory = new File(tmp_component_views_dir)
			if (view_directory.exists() && deploy_code) {
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
