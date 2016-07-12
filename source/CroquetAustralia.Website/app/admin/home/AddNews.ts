(function() {
    'use strict';

    angular
        .module('App.AddNews')
        .controller('AddNews', [AddNews]);

    function AddNews() {

        const vm = this;

        vm.content = '';
    }
})();