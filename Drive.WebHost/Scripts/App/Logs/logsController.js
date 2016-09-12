(function () {
    'use strict';

    angular.module('driveApp')
        .controller('LogsController', LogsController);

    LogsController.$inject = ['LogsService'];

    function LogsController(logsService) {
        var vm = this;
        vm.logs = [];
        vm.sort = {
            sortType: 'Date',
            sortReverse: true
        }
        vm.paginate = {
            currentPage: 1,
            pageSize: 15
        }
        activate();

        function activate() {
            return logs();
        }

        function logs() {
            return logsService.getAllLogs()
                .then(function(data) {
                    vm.logs = data;
                    return vm.logs;
                });
        }
    }
})();
