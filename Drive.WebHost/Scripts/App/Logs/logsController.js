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

        vm.search = function () {

            vm.searchType = angular.copy(vm.searchFields);
        }
        vm.cancelSearch = function () {
            vm.searchFields.callerName = '';
            vm.searchFields.exception = '';
            vm.searchFields.exception = '';
            vm.searchFields.message = '';
            vm.searchFields.level = '';
            vm.searchFields.logged = '';
            vm.searchType = angular.copy(vm.searchFields);
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
