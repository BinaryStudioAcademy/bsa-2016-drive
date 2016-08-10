(function () {
    'use strict';

    angular.module('driveApp')
        .controller('LogsController', LogsController);

    LogsController.$inject = ['LogsService'];

    function LogsController(logsService) {
        var vm = this;

        activate();

        function activate() {
            vm.logs = logsService.getLogs();
            vm.sort = {
                sortType: 'date',
                sortReverse: true
            }
        }
    }
})();
