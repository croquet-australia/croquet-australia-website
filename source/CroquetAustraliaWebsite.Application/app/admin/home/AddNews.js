(function() {
    'use strict';

    angular
        .module('app.AddNews')
        .controller('AddNews', [AddNews]);

    function AddNews() {

        var vm = this;

        vm.content = '';
    }
})();
