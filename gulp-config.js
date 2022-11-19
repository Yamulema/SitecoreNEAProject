module.exports = function () {
    var instanceRoot = "C:\\inetpub\\wwwroot";
    var config = {
        solutionName: "Neambc",
        buildConfiguration: "Debug",
        publishProfile: "CustomProfile",
        solutionPlatform: "AnyCPU",
        username: "",
        password: "",
        deployFolder: "",
        deployUnicornFolder: "",
        websiteRoot: instanceRoot + "\\neamb10sc.dev.local",
        sitecoreLibraries: instanceRoot + "\\neamb10sc.dev.local\\bin",
        publishMethod: "InProc",
        runCleanBuilds: false,
        toolsVersion: "16.0"
    };

    return config;
}