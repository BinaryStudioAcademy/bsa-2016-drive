(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('MenuService', MenuService);

    MenuService.$inject = [];

    function MenuService() {
        var service = {
            getMessage: getMessage
        };

        function getMessage() {
            return "...";
        }

        return service;
    }
})();