(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('LogsService', HomeService);

    HomeService.$inject = [];

    function HomeService() {
        var service = {
            getLogs: getLogs
        };

        function getLogs() {
            return [
            {
                id: 1,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Error',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 2,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Info',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 3,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Warn',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 4,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Info',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 5,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Debug',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 6,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Trace',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 7,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Error',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 8,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Info',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 9,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Warn',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 10,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Info',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 11,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Debug',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 12,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Trace',
                message: 'Error message 1',
                exception: 'Exception message 1'
            }];
        }

        return service;
    }
})();