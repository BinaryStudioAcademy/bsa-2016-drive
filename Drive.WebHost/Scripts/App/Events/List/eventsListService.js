(function() {
    'use strict';

    angular
        .module('driveApp.events')
        .factory('EventsListService', EventsListService);

    EventsListService.$inject = [
        '$http',
        'BaseUrl'
    ];

    function EventsListService($http, baseUrl) {
        var service = {
            getEvents: getEvents,
            searchEvents: searchEvents,
            deleteEvent: deleteEvent
        }

        return service;

        function getEvents() {
            return $http.get(baseUrl + '/api/events')
                .then(getEventsCompleted)
                .catch(getEventsFailed);

            function getEventsCompleted(response) {
                return response.data;
            }

            function getEventsFailed(error) {
                console.log('XHR Failed for getEvents.' + error.data);
            }
        }

        function searchEvents(text, callback) {
            $http.get(baseUrl + '/api/events/search',
                {
                    params: {
                        text: text
                    }
                })
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                },
                    function errorCallback(response) {
                        console.log('Error in searchEvents Method! Code:' + response.status);
                        if (response.status === 404 && callback) {
                            callback(response.data);
                        }
                    });
        }

        function deleteEvent(id, callback) {
            $http.delete(baseUrl + '/api/events/' + id)
                .then(function (response) {
                    if (callback) {
                        callback(response);
                    }
                },
                function () {
                    console.log('Error while event deleting!');
                });
        }
    }
})();