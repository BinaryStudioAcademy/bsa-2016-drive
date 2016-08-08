(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('SettingsService', SettingsService);

    SettingsService.$inject = [];

    function SettingsService() {
        var service = {
            getTitle: getTitle,
            getSpaceName: getSpaceName,
            getDescription: getDescription,
            getMaxFilesQuantity: getMaxFilesQuantity,
            getMaxFileSize: getMaxFileSize,
            getUsers: getUsers,
            getSpaceUsers: getSpaceUsers

        };

        function getTitle() {
            return "Space title";
        }

        function getSpaceName() {
            return "Space #1";
        }

        function getDescription() {
            return "Sample space description";
        }

        function getMaxFilesQuantity() {
            return 100;
        }

        function getMaxFileSize() {
            return 1024;
        }

        function getUsers() {
            return [
                { 'name': 'Nikita Krasnov', 'id': 1 },
                { 'name': 'Alex Aza', 'id': 2 },
                { 'name': 'Anton Kumpan', 'id': 3 },
                { 'name': 'Artem Kostenko', 'id': 4 },
                { 'name': 'Irina Antonenko', 'id': 5 }
            ];
        }

        function getSpaceUsers() {
            return [
                { 'name': 'Alex Aza', 'id': 2 },
                { 'name': 'Artem Kostenko', 'id': 4 }
            ];
        }

        return service;
    }
})();