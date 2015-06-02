(function() {
    'use strict';

    angular
        .module('app.AddPage')
        .controller('AddPage', [AddPage]);

    function AddPage() {

        var vm = this;

        vm.content = 'The **quick** brown fox';
    }
})();
