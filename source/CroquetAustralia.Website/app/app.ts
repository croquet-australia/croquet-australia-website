'use strict';

angular.module('App', modules())
    .config($locationProvider =>
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        })
    );

function modules() {
    return [

        // 3rd Party Modules
        'angularMoment',
        'angularUUID2',
        'codemwnci.markdown-edit-preview',
        'ngFileUpload',
        'ngAnimate',
        'ui.bootstrap',

        // Application Features
        'App.AddNews',
        'App.AddPage',
        'App.FileUpload'
    ];
}