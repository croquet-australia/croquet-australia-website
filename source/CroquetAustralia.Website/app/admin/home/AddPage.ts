(function() {
    'use strict';

    angular
        .module('App.AddPage')
        .controller('AddPage', [AddPage]);

    function AddPage() {

        const vm = this;

        vm.content = '';
    }
})();