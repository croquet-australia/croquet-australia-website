// todo: convert to true typescript
(function() {
    'use strict';

    angular
        .module('App.FileUpload')
        .controller('FileUpload', ['$scope', '$location', 'Upload', FileUpload]);

    // ReSharper disable once InconsistentNaming
    function FileUpload($scope, $location, Upload) {

        var vm = this;

        $scope.$watch('vm.files',
            function() {
                vm.upload(vm.files);
            });

        vm.log = '';

        vm.upload = function(files) {

            if (files && files.length) {

                vm.log = '';

                const directory = getDirectoryValueFromUrlParameter($location);

                for (let i = 0; i < files.length; i++) {

                    const file = files[i];

                    // todo: handle errors
                    Upload.upload({
                            url: '/admin/files/upload',
                            file: file,
                            fields: { directory: directory }

                        })
                        .progress(function(evt) {

                            const progressPercentage = parseInt((100.0 * evt.loaded / evt.total).toString());

                            if (progressPercentage < 100) {

                                if (vm.log.length > 0) {
                                    vm.log += '\n';
                                }

                                vm.log += `Uploading '${evt.config.file.name}', ${progressPercentage}%`;
                            }

                        })
                        .success(function(data, status, headers, config) {

                            vm.log += `\nUploaded '${config.file.name}' as '${data.savedAs}'.`;
                        });
                }
            }
        };
    }

    function getDirectoryValueFromUrlParameter($location) {

        const value = $location.search().directory;

        // When url parameter is ?directory then value is true. When url parameter is ?directory=x then value is x.
        if (value === true) {
            return '';
        }
        return value;
    }
})();