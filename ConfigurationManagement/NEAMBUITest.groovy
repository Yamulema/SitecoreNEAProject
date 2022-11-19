import org.jenkinsci.plugins.workflow.steps.FlowInterruptedException
@Library('oshyn-groovy@develop') _

node {
	notifications = true
	def pwd = pwd()
	def UITestFolderInNugetPkg = "_UITests"
	def testAssemblySuffixes = [".UITest.dll"]

	def projectNamespace = "NEAMB.Web"

	// test variables
	def test_category = params.Test_Category
	def target_url = params.BaseURL
	def target_browser = params.Target_Browser
	def target_platform = params.Target_Platform
	def failOnTestError = params.FailOnTestError
	def lcRecursionLevel = params.LinkCheckerRecursionLevel
	def lcThreadCount = params.LinkCheckerThreadCount
	def enableLinkChecker = params.EnableLinkChecker
	def nugetVersion = params.PackageVersion

	def startup_timeout=90
	def test_report_location = "TestResults"
	def test_dir = ""
	def test_execution=params.Execute_Tests
	def executionLocation = params.Execution_Location
	def whereClause=" --where "
	def report_pub_folder_name="osh-allure.oshyn.com"
	def test_report_full_location=pwd + "\\" +test_report_location
	def testResultFileName="work\\TestResult.xml"
	def testResultFileFull = test_report_full_location + "\\"+ testResultFileName
	def allure_guid="19df53a1-86b8-44f1-b6ec-24cad5976dd0"
	def allure_report_name="neamb.com UI Test Pack"

	def slackChannel="#build-notifications"
	def slackTeamDomain="oshynneamb"
	def o365webhookurl="https://outlook.office.com/webhook/0a396dbf-bf21-4d36-85f5-8ca760d2daea@d0e42219-9317-449b-b4bd-f8c7080d940f/JenkinsCI/2196b1aed796426a900a4d7a05646e61/51c07164-57d8-4235-b350-e58f46fcc7ee"


	try {
		notifyBuild buildStatus:"STARTED", channel:slackChannel, teamDomain: slackTeamDomain, test_category:test_category, executionLocation: executionLocation,send_notification:notifications,O365WebHookUrl:o365webhookurl
		properties([buildDiscarder(logRotator(artifactDaysToKeepStr: '', artifactNumToKeepStr: '20', daysToKeepStr: '', numToKeepStr: '100'))])

		stage ("Clean workspace") {
					cleanWs deleteDirs: true
		}
		
		parallel firstBranch: {
			stage("Retrieve Releaseable Package") {
				// pull the latest release from nexus
				sendNugetVersion=null
				if (nugetVersion != null  && nugetVersion != "latest") {
					sendNugetVersion=nugetVersion
				}
//					installAndUnpackNeambPackage projectNamespace, "nuget/NEAMBC-Release-SC9/"
				withEnv(["BundlePath=com/neamb"]) {
					installAndUnpackOshynPackage.v2 projectName:projectNamespace, repoPath:"nuget/NEAMBC-Release-SC9/",version:sendNugetVersion
				}
			}
		}, secondBranch: {
			stage("Run Link Checker") {
				//httpRequest responseHandle: 'NONE', timeout: startup_timeout, url: target_url
				// commenting out below link checker for neamb until the domain resolution works from linkchecker to www.neamb.com or qa.neamb.com (it does nto right now b/c of default site ssl certificate
				if (enableLinkChecker) {
					withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
						linkChecker url:target_url,pwd:pwd,link_test_location:test_report_location,recursionLevel:lcRecursionLevel,threadCount:lcThreadCount,archiveHtml:true,sendRemote:true
					}
				}
			}
		}
		stage("Execute UI Tests") {
			if (test_execution) {

				// find the package directory from nuget install above
				dir (pwd) {
				    foundDirs = findFiles()
				}
				def tmp_fnd_dirname=projectNamespace  //preset it to the project namespace
				boolean found=false
				for (int i=0;i< foundDirs.size() && !found;i++) {
					tmp_fnd_dirname = foundDirs[i].getName()
				  if (!tmp_fnd_dirname.contains("TestResults") && tmp_fnd_dirname.contains(projectNamespace)) {
						found=true
						dir (pwd + "\\"+ tmp_fnd_dirname) {
					  	println "found expanded nuget package here: " + tmp_fnd_dirname
							test_dir = pwd + "\\" + tmp_fnd_dirname + "\\" + UITestFolderInNugetPkg
					  }
				  }
				}
				// determine the category of tests to test
				if (test_category != "All") {  // doing high tests only
					whereClause += "\"cat == '" + test_category + "'\" "
				} else {
					whereClause = ""
				}
				if (executionLocation == "local") {
						paramsClause = "targetUrl="+target_url+";browserType="+target_browser+";platform="+target_platform+";screenshotFolder="+test_report_full_location+"\\screenshots"
				}
				else {
						paramsClause = "remoteDriver="+executionLocation+";targetUrl="+target_url+";browserType="+target_browser+";platform="+target_platform+";screenshotFolder="+test_report_full_location+"\\screenshots"
				}

				try {
					println "about to execute tests"
					withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
						discoverAndExecuteCoveredUnitTests test_assembly_path:test_dir,pwd:pwd,executionLocation:executionLocation,whereClause:whereClause, test_assembly_filter:testAssemblySuffixes,paramsClause:paramsClause,failOnTestError:failOnTestError,sendReportsRemote:true
					}
				 } finally {
					println "Publishing reports from finally block"
					
				//	compileAndPublishAllReports allure_guid,null,pwd,test_report_location,testResultFileFull,allure_report_name,test_category,target_url,target_browser,target_platform,executionLocation,sendReportsRemote:true
					withEnv(["BundlePath=com/neamb","ReportCustomer=NEAMB"]) {
						compileAndPublishAllReports allure_guid:allure_guid,remote_report_pub_folder_name:report_pub_folder_name,curpwd:pwd,test_report_location:test_report_location,
							testResultFile:testResultFileFull,report_name:allure_report_name,test_category:test_category,target_url:target_url,target_browser:target_browser,
							target_platform:target_platform,executionLocation:executionLocation,sendReportsRemote:true
					}
	// 				def build_num = env.BUILD_NUMBER

	// 				def indexredirect=""
	// 				indexredirect <<= '<html>r\n'
	// 				indexredirect <<= '  <head>\r\n'
	// 				indexredirect <<= '    <meta http-equiv="refresh" content="0;URL='+build_num+'/"/>\r\n'
	// 				indexredirect <<= '  </head>\r\n'
	// 				indexredirect <<= '</html>\r\n'
	// 				def indexredirect_file=test_report_location+"\\allure-report-redirect.html"
	// 				writeFile file: indexredirect_file, text:indexredirect.toString()

	// 				unstash 'AllureReportZip'
	// 				def unzipdir=pwd+"\\"+build_num
	// 				unzip dir:unzipdir , glob: '', zipFile: 'TestResults/allure-report.zip'
					
	// 				//dir (pwd) {
	// 				//	fileOperations([folderRenameOperation(destination: build_num, source: 'allure-report')])
	// 				//}
	// //				withAWS(profile:'neamb.jenkins') {
	// //					s3Upload acl: 'PublicRead', bucket: 'neamb-allure', cacheControl: '', excludePathPattern: '', file: build_num, metadatas: [''], sseAlgorithm: '', workingDir: ''				
	// //				}

	// 				//bat "aws s3 ls s3://neamb-allure --profile neamb.jenkins"
	// 				//bat "aws s3 cp " + build_num + " s3://neamb-allure/"+build_num+" --recursive --profile neamb.jenkins"
	// 				//bat "aws s3 cp \"" + indexredirect_file + "\" s3://neamb-allure/index.html --profile neamb.jenkins"
	// 				//zip dir: 'allure-report', glob: '', zipFile: 'TestResults/allure-report.zip'
					
				}

			} else {
				println "Tests have been disabled by build"
			}
		}  // end stage
	} catch (FlowInterruptedException fie) {
		println "Job aborted ... "
		currentBuild.result = "ABORTED"
	} catch (Exception e) {
		println "Job failed with exception: " + e
		currentBuild.result = "FAILURE"
		
	} finally {
			notifyBuild buildStatus:currentBuild.result, channel:slackChannel, teamDomain: slackTeamDomain, test_category:test_category, executionLocation: executionLocation,send_notification:notifications,O365WebHookUrl:o365webhookurl
			withEnv(["BundlePath=com/neamb"]) {
				sendBuildMetrics(influxDbName:"neamb-uitests",influxCredential:"influx-neamb")
			}
	}
}


