(function() {
    'use strict';

    var controllerId = 'AddPageCtrl';
    var moduleDependencies = ['codemwnci.markdown-edit-preview'];
    var controllerDependencies = ['$scope', addPage];

    angular
        .module('app', moduleDependencies)
        .controller(controllerId, controllerDependencies);

    function addPage($scope) {

        $scope.liveedit = 'I am **ready** to be edited!';
    }
})();
