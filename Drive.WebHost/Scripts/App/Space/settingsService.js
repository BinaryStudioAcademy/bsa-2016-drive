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
                { 'name': 'Nikita Krasnov', 'amount': 1 },
                { 'name': 'Alex Aza', 'amount': 2 },
                { 'name': 'Anton Kumpan', 'amount': 3 },
                { 'name': 'Artem Kostenko', 'amount': 4 },
                { 'name': 'Irina Antonenko', 'amount': 5 }
            ];
        }

        function getSpaceUsers() {
            return [
                { 'name': 'Alex Aza', 'amount': 2 },
                { 'name': 'Artem Kostenko', 'amount': 4 }
            ];
        }

        return service;
    }
})();