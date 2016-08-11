(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('LogsService', LogsService);

    LogsService.$inject = ['$http'];

    function LogsService($http) {
        var service = {
            getAllLogs: getAllLogs,
            getLogsRange: getLogsRange
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

        //function getLogsRange(from, to, callback) {
        //    return $http.get('/api/logs/' + from + '&' + to)
        //        .then(getLogsRangeComplete)
        //        .catch(getLogsRangeFailed);

        //    function getLogsRangeComplete(response) {
        //        return response.data;
        //    }

        //    function getLogsRangeFailed(error) {
        //        console.log('XHR Failed for getLogsRange.' + error.data);
        //    }
        //}

        return service;
    }
})();