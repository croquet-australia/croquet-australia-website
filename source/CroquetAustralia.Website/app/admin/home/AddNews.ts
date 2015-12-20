(function() {
    'use strict';

    angular
        .module('App.AddNews')
        .controller('AddNews', [AddNews]);

    function AddNews() {

        var vm = this;

        vm.content = '';
    }
})();
