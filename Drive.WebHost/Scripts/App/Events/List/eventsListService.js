(function () {
    'use strict';

    angular
    .module('driveApp.events')
    .factory('EventsListService', EventsListService);

    EventsListService.$inject = [
        '$http'
    ];

    function EventsListService($http) {
        var service = {
            getEvents: getEvents
        }

        return service;

        function getEvents() {
            return $http.get('/api/events')
            .then(getEventsCompleted)
            .catch(getEventsFailed);

            function getEventsCompleted(response) {
                return response.data;
            }

            function getEventsFailed(error) {
                console.log('XHR Failed for getEvents.' + error.data);
            }
        }
    }
})