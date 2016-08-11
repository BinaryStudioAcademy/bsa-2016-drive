(function () {
    'use strict';

    angular.module('driveApp')
        .controller('LogsController', LogsController);

    LogsController.$inject = ['LogsService'];

    function LogsController(logsService) {
        var vm = this;
        vm.logs = [];
        //vm.logsRange = [];
        vm.sort = {
            sortType: 'Date',
            sortReverse: true
        }
        vm.logsPerPage = 10;
        vm.page = 1;

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

        //function logsRange() {
        //    var startIndex = vm.page * vm.logsPerPage - (vm.logsPerPage - 1);
        //    var endIndex = vm.page * vm.logsPerPage;
        //    return logsService.getLogsRange(startIndex, endIndex)
        //        .then(function(data) {
        //            vm.logsRange = data;
        //            return vm.logsRange;
        //        });
        //}
    }
})();
