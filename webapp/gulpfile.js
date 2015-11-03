var gulp = require('gulp');
var concat = require("gulp-concat");

gulp.task("dependencies-copy", function() {
  
  var dependencyFiles = [
    "./bower_components/jquery/dist/jquery.js",
    "./bower_components/angular/angular.js",
    "./bower_components/angular-resource/angular-resource.js",
    "./bower_components/angular-cookies/angular-cookies.js"
  ];
  
  gulp.src(dependencyFiles)
    .pipe(concat("dependencies.js"))
    .pipe(gulp.dest('./public/scripts'));
});
 
gulp.task('copy-css', function() {
  
    gulp.src(['./src/styles/*.css'])
      .pipe(gulp.dest('./public/styles/'));
});

gulp.task('copy-js', function() {
  
    gulp.src(['./src/scripts/*.js'])
      .pipe(gulp.dest('./public/scripts/'));
});

gulp.task('copy-index', function() {
  
    gulp.src(['./src/index.html'])
      .pipe(gulp.dest('./public'));
});

gulp.task('copy', ['copy-index', 'copy-css', 'copy-js']);
gulp.task('default', ['copy', 'dependencies-copy']);