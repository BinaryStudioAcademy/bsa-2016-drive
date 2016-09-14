(function() {
    'use strict';

    angular
        .module('driveApp.academyPro')
        .factory('EventService', EventService);

    EventService.$inject = [
        '$http',
        'BaseUrl'
    ];

    function EventService($http, baseUrl) {
        var service = {
            
        }

        return service;
    }
})();