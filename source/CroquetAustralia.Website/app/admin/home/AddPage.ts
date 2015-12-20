(function() {
    'use strict';

    angular
        .module('App.AddPage')
        .controller('AddPage', [AddPage]);

    function AddPage() {

        var vm = this;

        vm.content = '';
    }
})();
