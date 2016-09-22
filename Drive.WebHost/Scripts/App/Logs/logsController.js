(function () {
    'use strict';

    angular.module('driveApp')
        .controller('LogsController', LogsController);

    LogsController.$inject = ['LogsService', '$location', 'JwtService'];

    function LogsController(logsService, $location, jwtService) {
        var vm = this;
        vm.checkUserIsAdmin = checkUserIsAdmin;

        vm.logs = [];

        vm.sort = {
            sortType: 'Date',
            sortReverse: true
        }
        vm.paginate = {
            currentPage: 1,
            pageSize: 15
        }

        vm.isAdmin = jwtService.isAdmin;

        activate();

        function activate() {
            checkUserIsAdmin();

            return logs();
        }

        vm.search = function() {
            vm.searchType = angular.copy(vm.searchFields);
        }

        vm.cancelSearch = function() {
            vm.searchFields.callerName = '';
            vm.searchFields.exception = '';
            vm.searchFields.exception = '';
            vm.searchFields.message = '';
            vm.searchFields.level = '';
            vm.searchFields.logged = '';
            vm.searchType = angular.copy(vm.searchFields);
        }

        function checkUserIsAdmin() {
            if (vm.isAdmin) {
                $location.url("/binaryspace");
            }
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
