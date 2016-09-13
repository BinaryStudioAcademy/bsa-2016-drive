(function () {
    'use strict';

    angular
        .module('driveApp.academyPro')
        .factory('EventContentService', EventContentService);

    EventContentService.$inject = [
        '$http',
        'BaseUrl'
    ];

    function EventContentService($http, baseUrl) {
        var service = {
            getEventContent: getEventContent,
            pushData: pushData,
            putData: putData
        }

        return service;

        function getEventContent(id) {
            return $http.get(baseUrl + '/api/eventscontent/' + id)
                .then(getEventContentCompleted)
                .catch(getEventContentFailed);

            function getEventContentCompleted(response) {
                return response.data;
            }

            function getEventContentFailed(error) {
                console.log('XHR Failed for getEventContent.' + error.data);
            }
        }

        function pushData(data) {
            return $http.post(baseUrl + '/api/eventscontent', data)
                .then(pushDataCompleted)
                .catch(pushDataFailed);

            function pushDataCompleted(response) {
                return response.data;
            }

            function pushDataFailed(error) {
                console.log('XHR Failed for pushData.' + error.data);
            }
        }

        function putData(id, data) {
            return $http.put(baseUrl + '/api/eventscontent/' + id, data)
                .then(putDataCompleted)
                .catch(putDataFailed);

            function putDataCompleted(response) {
                return response.data;
            }

            function putDataFailed(error) {
                console.log('XHR Failed for putData.' + error.data);
            }
        }
    }
})();