/// <binding ProjectOpened='wireDependencies, watch' />
/// <vs BeforeBuild='wireDependencies, bower' SolutionOpened='watch' />
/*global require, pipe, process */
'use strict';

var config = require('./gulpconfig')();
var del = require('del');
var gulp = require('gulp');
var plugins = require('gulp-load-plugins')();
var sass = require('gulp-sass');
var fs = require('fs');
var CacheBuster = require('gulp-cachebust');
var cachebust = new CacheBuster();

////////////////////////////////////////////////////
// Tasks
////////////////////////////////////////////////////

// ReSharper disable PossiblyUnassignedProperty

gulp.task('bower',
    function() {
        return plugins.bower();
    });

gulp.task('code-quality',
    function() {
        log('Analyzing source with JSHint and JSCS');

        return gulp
            .src(config.jsFiles)
            .pipe(plugins.jshint())
            .pipe(plugins.jshint.reporter('jshint-stylish', { verbose: true }))
            .pipe(plugins.jshint.reporter('fail'))
            .pipe(plugins.jscs());
    });

gulp.task('build', ['styles']);

gulp.task('watch',
    function() {
        watch('./app/styles/*.scss', ['styles']);
    });

gulp.task('wireDependencies', ['wireJavaScriptDependencies']);

gulp.task('wireJavaScriptDependencies',
    function() {
        log('Wiring the JavaScript dependencies into layout file.');

        var wiredep = require('wiredep').stream;

        return gulp.src(config.javaScriptLayoutFile)
            .pipe(wiredep(config.wiredepOptions))
            .pipe(plugins.inject(gulp.src(config.appJsFiles, { read: false }), config.injectOptions))
            .pipe(gulp.dest(config.layoutDirectory));
    });

gulp.task('styles',
    function() {
        return gulp
            .src(config.sassFiles)
            .pipe(sass().on('error', sass.logError))
            .pipe(gulp.dest('./app/styles'));
    });

gulp.task('before-kudusync', ['bust-caches']);

gulp.task('bust-caches',
    ['bust-caches-css', 'bust-caches-js'],
    function() {
        var sourceDirectory = get_Deployment_Temp_Directory();
        log(`Renaming references in ${sourceDirectory}.`);

        return gulp
            .src(sourceDirectory + '/**/*.*html')
            .pipe(cachebust.references())
            .pipe(gulp.dest(sourceDirectory));
    });

gulp.task('bust-caches-css',
    function() {
        var sourceDirectory = get_Deployment_Temp_Directory() + '/App';
        log(`Renaming css resources in ${sourceDirectory}.`);

        return gulp.src(sourceDirectory + '/**/*.css')
            .pipe(cachebust.resources())
            .pipe(gulp.dest(sourceDirectory));
    });

gulp.task('bust-caches-js',
    function() {
        var sourceDirectory = get_Deployment_Temp_Directory() + '/App';
        log(`Renaming js resources in ${sourceDirectory}.`);

        return gulp.src(sourceDirectory + '/**/*.js')
            .pipe(cachebust.resources())
            .pipe(gulp.dest(sourceDirectory));
    });

////////////////////////////////////////////////////
// Functions
////////////////////////////////////////////////////

function get_Deployment_Temp_Directory() {

    var directory = process.env.DEPLOYMENT_TEMP;

    if (!directory) {
        throw 'Cannot find environment variable \'DEPLOYMENT_TEMP\'.';
    }

    directory = directory.replace(/\\/g, '/');

    if (!fs.existsSync(directory)) {
        throw `Cannot find DEPLOYMENT_TEMP directoruy '${directory}'.`;
    }

    return directory;
}

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
    log(`Watching ${files} for ${task}`);
    gulp.watch(files, [task]);
}