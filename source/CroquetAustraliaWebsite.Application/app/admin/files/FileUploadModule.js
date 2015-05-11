(function() {
    'use strict';

    console.log('TRACE FileUploadModule.js');

    var controllerId = 'FileUploadCtrl';

    angular.module('app').controller(controllerId, ['$scope', '$location', 'Upload', fileUpload]);

    // ReSharper disable once InconsistentNaming
    function fileUpload($scope, $location, Upload) {

        console.log('TRACE FileUploadModule.fileUpload($scope, Upload)');

        $scope.$watch('files', function() {
            $scope.upload($scope.files);
        });
        $scope.log = '';

        $scope.upload = function(files) {

            if (files && files.length) {

                $scope.log = '';

                var directory = getDirectoryValueFromUrlParameter($location);

                for (var i = 0; i < files.length; i++) {

                    var file = files[i];

                    console.log('TRACE FileUploadModule.fileUpload($scope, Upload).upload ' + file.name);

                    // todo: handle errors
                    Upload.upload({
                        url: '/admin/files/upload',
                        file: file,
                        fields: { directory: directory }

                    }).progress(function(evt) {

                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);

                        if (progressPercentage < 100) {

                            if ($scope.log.length > 0) {
                                $scope.log += '\n';
                            }

                            $scope.log += 'Uploading \'' + evt.config.file.name + '\', ' + progressPercentage + '%';
                        }

                    }).success(function(data, status, headers, config) {

                        $scope.log += '\nUploaded \'' + config.file.name + '\' as \'' + data.savedAs + '\'.';
                        $scope.$apply();
                    });
                }
            }
        };
    }

    function getDirectoryValueFromUrlParameter($location) {

        var value = $location.search().directory;

        // When url parameter is ?directory then value is true. When url parameter is ?directory=x then value is x.
        if (value === true) {
            return '';
        }
        return value;
    }
})();
