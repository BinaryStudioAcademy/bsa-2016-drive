(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('LogsService', LogsService);

    LogsService.$inject = ['$http'];

    function LogsService($http) {
        var service = {
            getAllLogs: getAllLogs
        };

        function getAllLogs() {
            return $http.get('/api/logs')
                .then(getLogsComplete)
                .catch(getLogsFailed);

            function getLogsComplete(response) {
                return response.data;
            }

            function getLogsFailed(error) {
                console.log('XHR Failed for getLogs.' + error.data);
            }
        }

        return service;
    }
})();