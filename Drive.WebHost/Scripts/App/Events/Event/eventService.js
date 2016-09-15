(function () {
    'use strict';

    angular
        .module('driveApp.events')
        .factory('EventService', EventService);

    EventService.$inject = [
        '$http',
        'BaseUrl'
    ];

    function EventService($http, baseUrl) {
        var service = {
            getEvent: getEvent,
            pushData: pushData,
            putData: putData,
            getEventTypes: getEventTypes
        };

        return service;

        function getEvent(id) {
            return $http.get(baseUrl + '/api/events/' + id)
                   .then(getEventCompleted)
                   .catch(getEventFailed);

            function getEventCompleted(response) {
                return response.data;
            }

            function getEventFailed(error) {
                console.log('XHR Failed for getEvent.' + error.data);
            }
        }

        function pushData(data) {
            return $http.post(baseUrl + '/api/events', data)
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
            return $http.put(baseUrl + '/api/events/' + id, data)
                .then(putDataCompleted)
                .catch(putDataFailed);

            function putDataCompleted(response) {
                return response.data;
            }

            function putDataFailed(error) {
                console.log('XHR Failed for putData.' + error.data);
            }
        }

        function getEventTypes(callback) {
            return $http.get(baseUrl + '/api/events/eventtypes')
                   .then(function (response) {
                       if (callback) {
                           callback(response.data);
                       }
                   },
                     function () {
                         console.log('Error while getting all users!');
                     });
        }
    }

})();