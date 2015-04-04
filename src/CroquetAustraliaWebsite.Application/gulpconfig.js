/*global module */

module.exports = function () {
    'use strict';

    var clientDirectory = '.';

    var appJsFiles = './app/**/*.js';
    var layoutDirectory = './Layouts/Shared';
    // todo: var cssDirectory = './app/css';
    // todo: var lessDirectory = './app/less';

    var config = {

        // Files
        // files are arrays for possible future use.
        appJsFiles: [appJsFiles],
        jsFiles: ['./*.js', appJsFiles],
        javaScriptLayoutFile: layoutDirectory + '/AfterRenderBody.cshtml',

        // Directories
        layoutDirectory: layoutDirectory,

        // Options
        injectOptions: {
            ignorePath: clientDirectory.substr(1)   // clientDirectory expect for leading .
        },

        wiredepOptions: {
            bowerJson: require('./bower.json'),
            directory: clientDirectory + '/bower_components/',
            ignorePath: '../..'
        }
    };

    return config;
};
