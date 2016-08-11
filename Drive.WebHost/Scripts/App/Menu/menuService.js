(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('MenuService', MenuService);

    MenuService.$inject = ['$location'];

    function MenuService() {
        var service = {
            redirectToBinarySpace: redirectToBinarySpace,
            redirectToSpaceSettings: redirectToSpaceSettings,
            redirectToAddBinarySpace: redirectToAddBinarySpace,
            redirectToAddFile: redirectToAddFile,
            redirectToAddMySpace: redirectToAddMySpace
        };

        function redirectToBinarySpace(id) {
            $location.url('/api/spaces/' + id);
        };

        function redirectToSpaceSettings(id) {
            $location.url('/api/spaces/settings/' + id);
        };

        function redirectToAddBinarySpace() {
            $location.url('/api/spaces/');
        };

        function redirectToAddFile() {
            $location.url('api/files');
        };

        function redirectToAddMySpace() {
            $location.url('api/Space/spaces');
        };

        return service;
    }
})();