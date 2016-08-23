(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('LogsService', LogsService);

    LogsService.$inject = ['$http', 'BaseUrl'];

    function LogsService($http, baseUrl) {
        var service = {
            getAllLogs: getAllLogs
        };

        function getAllLogs() {
            return $http.get(baseUrl + '/api/logs')
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