(function() {
    'use strict';

    angular
        .module('driveApp')
        .factory('HomeService', HomeService);

    HomeService.$inject = [];

    function HomeService() {
        var service = {
            getMessage: getMessage
        };

        function getMessage() {
            return "Coming soon... Stay tuned...";
        }

        return service;
    }
})();