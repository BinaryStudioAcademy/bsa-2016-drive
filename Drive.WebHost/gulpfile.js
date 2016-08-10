/// <binding BeforeBuild='develop' />
var gulp = require("gulp");
var bower = require("gulp-bower");
var concat = require("gulp-concat");
var rimraf = require("rimraf");
var cssmin = require("gulp-cssmin");
var uglify = require("gulp-uglify");
var rename = require("gulp-rename");

gulp.task("prod", ["main", "libs", "styles", "minify"]);
gulp.task("develop", ["clean", "bower", "main", "libs", "styles"]);
gulp.task("noBower", ["main", "libs", "styles"]);

var paths = {
    bower: "bower_components/**/",
    scriptBundles: "./Scripts/Bundles",
    contentBundles: "./Content/Bundles"
};

gulp.task("bower", function () {
    return bower("./bower_components");
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("clean:js", function (cb) {
    rimraf(paths.scriptBundles, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.contentBundles, cb);
});

gulp.task("main", function () {
    gulp.src(["./Scripts/App/*.js", "./Scripts/App/**/*.js"])
        .pipe(concat("main.js"))
        .pipe(gulp.dest(paths.scriptBundles));
});

gulp.task("libs", function () {
    gulp.src([
      paths.bower + "angular.js",
      paths.bower + "angular-route.js",
      paths.bower + "jquery.js",
      paths.bower + "bootstrap.js",
      paths.bower + "contextMenu.js",
      paths.bower + "ui-bootstrap.js"
    ])
        .pipe(concat("libs.js"))
        .pipe(gulp.dest(paths.scriptBundles));
});

gulp.task("styles", function () {
    gulp.src([
        paths.bower + "bootstrap.css",
        "./Content/*.css"
    ])
        .pipe(concat("styles.css"))
        .pipe(gulp.dest(paths.contentBundles));
});

gulp.task("minify", ["min:js", "min:css"]);

gulp.task("min:js", function () {
    gulp.src(paths.scriptBundles + "/*.js")
        .pipe(uglify())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest(paths.scriptBundles));
});

gulp.task("min:css", function () {
    gulp.src(paths.contentBundles + "/*.css")
        .pipe(cssmin())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest(paths.contentBundles));
});