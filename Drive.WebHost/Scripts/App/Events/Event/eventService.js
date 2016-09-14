(function() {
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
            getEvent: getEvent
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
    }
})();