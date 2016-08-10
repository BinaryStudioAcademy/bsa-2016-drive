(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('LogsService', HomeService);

    HomeService.$inject = [];

    function HomeService() {
        var service = {

        };

        return service;
    }
})();