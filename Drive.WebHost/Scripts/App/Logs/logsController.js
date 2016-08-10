(function() {
    'use strict';

    angular.module('driveApp')
        .controller('LogsController', LogsController);

    LogsController.$inject = ['LogsService'];

    function LogsController() {
        var vm = this;

        activate();

        function activate() {
            
        }
    }
})();