/*global module */

module.exports = function() {
    'use strict';

    const clientDirectory = '.';

    const appJsFiles = ['./app/**/*.js', '!./App/_references.js'];
    const layoutDirectory = './Layouts/Shared';
    // todo: var cssDirectory = './app/css';
    // todo: var lessDirectory = './app/less';

    const config = {

        // Files
        // files are arrays for possible future use.
        appJsFiles: appJsFiles,
        jsFiles: ['./*.js'].concat(appJsFiles),
        javaScriptLayoutFile: layoutDirectory + '/AfterRenderBody.cshtml',
        sassFiles: ['./app/styles/styles.scss'],

        // Directories
        layoutDirectory: layoutDirectory,

        // Options
        injectOptions: {
            ignorePath: clientDirectory.substr(1) // clientDirectory expect for leading .
        },

        wiredepOptions: {
            bowerJson: require('./bower.json'),
            directory: clientDirectory + '/bower_components/',
            ignorePath: '../..'
        }
    };

    return config;
};