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
            pushData: pushData,
            getEventTypes: getEventTypes
        }

        return service;

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

        function getEventTypes(callback) {
            return $http.get(baseUrl + '/api/events/eventtypes')
                  .then(function(response) {
                      if (callback) {
                         callback(response.data);
                         }
                    },
                    function() {
                        console.log('Error while getting all users!');
                    });        
        }
    }
})();