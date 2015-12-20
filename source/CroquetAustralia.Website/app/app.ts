'use strict';

angular.module('App', modules());

function modules() {
    return [

        // 3rd Party Modules
        'angularUUID2',
        'codemwnci.markdown-edit-preview',
        'ngFileUpload',

        // Application Features
        'App.AddNews',
        'App.AddPage',
        'App.FileUpload'
    ];
}
