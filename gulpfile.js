var gulp = require("gulp");
var _msbuild = require("msbuild");
var msbuild = require("gulp-msbuild");
var del = require("del");
var foreach = require("gulp-foreach");
var runSequence = require("run-sequence");
var config = require("./gulp-config.js")();
var nugetRestore = require('gulp-nuget-restore');
var gutil = require('gulp-util');
var newer = require("gulp-newer");
var debug = require("gulp-debug");
var rename = require("gulp-rename");
module.exports.config = config;
var msbuildF = new _msbuild();

try {
    var localconfig = require('./local-config');
    gutil.log("Found local config file to be used " + localconfig["publishProfile"].path);
} catch (error) {
    var localconfig = {
        publishProfile: "web"
    }
}

gulp.task("Build-And-Publish", function (callback) {
    return runSequence(
        "02-Build-Solution",
        "03-Publish-Foundation-Projects",
        "04-Publish-Feature-Projects",
        "05-Publish-Project-Projects",
        callback
    );
});


gulp.task("Publish-Single-Project", function () {
    const location = "./src/Feature/Neamb.Product/";
    return gulp.src([location + "/**/code/*.csproj"])
        .pipe(foreach(function (stream, file) {
            msbuildF.solutionName = config.solutionName;
            msbuildF.publishProfile = config.publishProfile;
            msbuildF.sourcePath = file.path;
            msbuildF.configuration = config.buildConfiguration;
            msbuildF.config('version', config.toolsVersion);
            msbuildF.publish();

            return stream;
        }));
});

gulp.task("00-Clean-Folder", function (callback) {
    var solution = "./" + config.solutionName + ".sln";

    gutil.log('deployFolder: ' + config.deployFolder);
    gutil.log('deployUnicornFolder: ' + config.deployUnicornFolder);
    gutil.log('Solution: ' + solution);

    if (config.deployFolder !== "") {
        del.sync([config.deployFolder + "\\**", "!" + config.deployFolder], { force: true });
    }

    if (config.deployUnicornFolder !== "") {
        del.sync([config.deployUnicornFolder + "\\**", "!" + config.deployUnicornFolder], { force: true });
    }
    return gulp.src(solution);
});

gulp.task("01-Nuget-Restore", function (callback) {
    var solution = "./" + config.solutionName + ".sln";
    gutil.log('Solution: ' + solution);
    return gulp.src(solution).pipe(nugetRestore());
});

gulp.task("02-Build-Solution", function () {
    const solution = "./" + config.solutionName + ".sln";

    msbuildF.solutionName = config.solutionName;
    msbuildF.publishProfile = config.publishProfile;
    msbuildF.sourcePath = solution;
    msbuildF.configuration = config.buildConfiguration;
    msbuildF.config('version', config.toolsVersion);

    const overrideParams = [];
    overrideParams.push('/p:verbosity=quiet');
    msbuildF.config('overrideParams', overrideParams);
    msbuildF.build();
});

gulp.task("03-Publish-Foundation-Projects", function () {
    const location = "./src/Foundation";

    return gulp.src([location + "/**/code/*.csproj"])
        .pipe(foreach(function (stream, file) {
            msbuildF.solutionName = config.solutionName;
            msbuildF.publishProfile = config.publishProfile;
            msbuildF.sourcePath = file.path;
            msbuildF.configuration = config.buildConfiguration;
            msbuildF.config('version', config.toolsVersion);
            msbuildF.publish();
            return stream;
        }));
});

gulp.task("04-Publish-Feature-Projects", function () {
    const location = "./src/Feature";
    return gulp.src([location + "/**/code/*.csproj"])
        .pipe(foreach(function (stream, file) {
            msbuildF.solutionName = config.solutionName;
            msbuildF.publishProfile = config.publishProfile;
            msbuildF.sourcePath = file.path;
            msbuildF.configuration = config.buildConfiguration;
            msbuildF.config('version', config.toolsVersion);
            msbuildF.publish();

            return stream;
        }));
});

gulp.task("05-Publish-Project-Projects", function () {
    const location = "./src/Project";
    return gulp.src([location + "/**/code/*.csproj"])
        .pipe(foreach(function (stream, file) {
            msbuildF.solutionName = config.solutionName;
            msbuildF.publishProfile = config.publishProfile;
            msbuildF.sourcePath = file.path;
            msbuildF.configuration = config.buildConfiguration;
            msbuildF.config('version', config.toolsVersion);

            const overrideParams = [];
            overrideParams.push('/p:verbosity=quiet');
            overrideParams.push('/p:consoleLoggerParameters=ErrorsOnly');
            msbuildF.config('overrideParams', overrideParams);
            msbuildF.publish();
            return stream;
        }));
});

gulp.task("06-Copy-Unicorn-Files", function (callback) {
    return gulp.src(["./src/**/*.yml"], { base: "./src" })
        .pipe(gulp.dest(config.deployUnicornFolder));
});

gulp.task("v2-00-Build-And-Publish", function (callback) {
    return runSequence(
        "v2-01-Build-Solution",
        "v2-02-Publish-Assemblies-Neamb",
        "v2-03-Publish-Assemblies-Seiumb",
        "v2-04-Publish-All-Views",
        "v2-07-Publish-Css-Neamb",
        "v2-08-Publish-Js-Neamb",
        "v2-09-Publish-Css-Seiumb",
        "v2-10-Publish-Js-Seiumb",
        callback);
});

gulp.task("v2-01-Build-Solution", function () {
    var targets = ["Build"];
    if (config.runCleanBuilds) {
        targets = ["Clean", "Build"];
    }
    var solution = "./" + config.solutionName + ".sln";
    return gulp.src(solution)
        .pipe(msbuild({
            targets: targets,
            configuration: config.buildConfiguration,
            logCommand: false,
            verbosity: "minimal",
            stdout: true,
            errorOnFail: true,
            maxcpucount: 0,
            toolsVersion: 16
        }));
});

gulp.task("v2-02-Publish-Assemblies-Neamb", function () {
    var root = "./src";
    var binFiles = root + "/**/code/**/bin/Neambc.Neamb.{Feature,Foundation,Project}.!(*.UnitTest).{dll,pdb}";
    var destination = config.sitecoreLibraries;
    console.log("Publishing to " + destination);
    return gulp.src(binFiles, { base: root })
        .pipe(rename({ dirname: "" }))
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
});

gulp.task("v2-03-Publish-Assemblies-Seiumb", function () {
    var root = "./src";
    var binFiles = root + "/**/code/**/bin/Neambc.Seiumb.{Feature,Foundation,Project}.!(*.UnitTest).{dll,pdb}";
    var destination = config.sitecoreLibraries;
    console.log("Publishing to " + destination);
    return gulp.src(binFiles, { base: root })
        .pipe(rename({ dirname: "" }))
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
});

gulp.task("v2-04-Publish-All-Views", function () {
    var root = "./src";
    var roots = [root + "/**/Views", "!" + root + "/**/obj/**/Views"];
    var files = "/**/*.cshtml";
    var destination = config.websiteRoot + "\\Views";
    return gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            console.log("Publishing to " + destination);
            gulp.src(file.path + files, { base: file.path })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});

//TODO: ajust to the project
gulp.task("v2-05-TODO-Publish-All-Images", function () {
    var root = "./src";
    var roots = [root + "/**/images", "!" + root + "/**/obj/**/images"];
    var files = "/**/*.{png,gif,jpg,jpeg}";
    var destination = config.websiteRoot + "\\images";
    return gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            gulp.src(file.path + files, { base: file.path })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});

//TODO: ajust to the project
gulp.task("v2-06-TODO-Publish-All-Configs", function () {
    var root = "./src";
    var roots = [root + "/**/App_Config", "!" + root + "/**/obj/**/App_Config"];
    var files = "/**/*.config";
    var destination = config.websiteRoot + "\\App_Config";
    return gulp.src(roots, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            gulp.src(file.path + files, { base: file.path })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});

gulp.task("v2-07-Publish-Css-Neamb", function () {
    var root = "./src";
    var rootForStyles = root + "/Project/Neamb/code/assets/neamb/styles";
    var sources = [
        root + "/Project/Neamb/code/assets/neamb/styles/**/",
    ];
    var destination = config.websiteRoot + "/assets/neamb/styles";

    return gulp.src(sources, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            gulp.src(file.path, { base: rootForStyles })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});

gulp.task("v2-08-Publish-Js-Neamb", function () {
    var root = "./src";
    var rootForStyles = root + "/Project/Neamb/code/assets/neamb/scripts";
    var sources = [
        root + "/Project/Neamb/code/assets/neamb/scripts/**/",
    ];
    var destination = config.websiteRoot + "/assets/neamb/scripts";

    return gulp.src(sources, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            gulp.src(file.path, { base: rootForStyles })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});

gulp.task("v2-09-Publish-Css-Seiumb", function () {
    var root = "./src";
    var rootForStyles = root + "/Project/Seiu/code/assets/seiumb/styles";
    var sources = [
        rootForStyles + "/**/",
    ];
    var destination = config.websiteRoot + "/assets/seiumb/styles";
    console.log(sources);
    return gulp.src(sources, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            gulp.src(file.path, { base: rootForStyles })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});

gulp.task("v2-10-Publish-Js-Seiumb", function () {
    var root = "./src";
    var rootForStyles = root + "/Project/Seiu/code/assets/seiumb/scripts";
    var sources = [
        rootForStyles + "/**/",
    ];
    var destination = config.websiteRoot + "/assets/seiumb/scripts";

    return gulp.src(sources, { base: root }).pipe(
        foreach(function (stream, file) {
            console.log("Publishing from " + file.path);
            gulp.src(file.path, { base: rootForStyles })
                .pipe(newer(destination))
                .pipe(debug({ title: "Copying " }))
                .pipe(gulp.dest(destination));
            return stream;
        })
    );
});