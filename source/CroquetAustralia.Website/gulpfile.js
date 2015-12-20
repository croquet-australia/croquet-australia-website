/// <binding ProjectOpened='wireDependencies, watch' />
/// <vs BeforeBuild='wireDependencies, bower' SolutionOpened='watch' />
/*global require, pipe */
'use strict';

var config = require('./gulpconfig')();
var del = require('del');
var gulp = require('gulp');
var plugins = require('gulp-load-plugins')();
var sass = require('gulp-sass');

////////////////////////////////////////////////////
// Tasks
////////////////////////////////////////////////////

// ReSharper disable PossiblyUnassignedProperty

gulp.task('bower', function () {
    return plugins.bower();
});

gulp.task('code-quality', function () {
    log('Analyzing source with JSHint and JSCS');

    return gulp
        .src(config.jsFiles)
        .pipe(plugins.jshint())
        .pipe(plugins.jshint.reporter('jshint-stylish', { verbose: true }))
        .pipe(plugins.jshint.reporter('fail'))
        .pipe(plugins.jscs());
});

gulp.task('build', ['styles']);

gulp.task('watch', function () {
    watch('./app/styles/*.scss', ['styles']);
});

gulp.task('wireDependencies', ['wireJavaScriptDependencies']);

gulp.task('wireJavaScriptDependencies', function () {
    log('Wiring the JavaScript dependencies into layout file.');

    var wiredep = require('wiredep').stream;

    return gulp.src(config.javaScriptLayoutFile)
        .pipe(wiredep(config.wiredepOptions))
        .pipe(plugins.inject(gulp.src(config.appJsFiles, { read: false }), config.injectOptions))
        .pipe(gulp.dest(config.layoutDirectory));
});

gulp.task('styles', function () {
    return gulp
        .src(config.sassFiles)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./app/styles'));
});

////////////////////////////////////////////////////
// Functions
////////////////////////////////////////////////////

function log(message) {
    var item;
    if (typeof (message) === 'object') {
        for (item in message) {
            if (message.hasOwnProperty(item)) {
                plugins.util.log(plugins.util.colors.yellow(message[item]));
            }
        }
    } else {
        plugins.util.log(plugins.util.colors.yellow(message));
    }
}

function watch(files, task) {
    log('Watching ' + files + ' for ' + task);
    gulp.watch(files, [task]);
}
