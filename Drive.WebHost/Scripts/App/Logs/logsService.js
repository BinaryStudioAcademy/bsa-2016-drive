(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('LogsService', LogsService);

    LogsService.$inject = [];

    function LogsService() {
        var service = {
            getLogs: getLogs
        };

        function getLogs() {
            return [
            {
                id: 1,
                time: new Date(2016, 8, 6, 1, 8, 10),
                level: 'Error',
                message: 'Error message 1',
                exception: 'Exception message 1'
            },
            {
                id: 2,
                time: new Date(2016, 8, 6, 2, 8, 10),
                level: 'Info',
                message: 'Info message 1',
                exception: ''
            },
            {
                id: 3,
                time: new Date(2016, 8, 6, 3, 8, 10),
                level: 'Warn',
                message: 'Warn message 1',
                exception: ''
            },
            {
                id: 4,
                time: new Date(2016, 8, 6, 4, 8, 10),
                level: 'Info',
                message: 'Info message 2',
                exception: ''
            },
            {
                id: 5,
                time: new Date(2016, 8, 6, 5, 8, 10),
                level: 'Debug',
                message: 'Debug message 1',
                exception: ''
            },
            {
                id: 6,
                time: new Date(2016, 8, 6, 6, 8, 10),
                level: 'Trace',
                message: 'Trace message 1',
                exception: ''
            },
            {
                id: 7,
                time: new Date(2016, 8, 6, 7, 8, 10),
                level: 'Error',
                message: 'Error message 2',
                exception: 'Exception message 2'
            },
            {
                id: 8,
                time: new Date(2016, 8, 6, 8, 8, 10),
                level: 'Info',
                message: 'Info message 3',
                exception: ''
            },
            {
                id: 9,
                time: new Date(2016, 8, 6, 9, 8, 10),
                level: 'Warn',
                message: 'Warn message 2',
                exception: ''
            },
            {
                id: 10,
                time: new Date(2016, 8, 6, 10, 8, 10),
                level: 'Info',
                message: 'Info message 4',
                exception: ''
            },
            {
                id: 11,
                time: new Date(2016, 8, 6, 11, 8, 10),
                level: 'Debug',
                message: 'Debug message 2',
                exception: ''
            },
            {
                id: 12,
                time: new Date(2016, 8, 6, 12, 8, 10),
                level: 'Trace',
                message: 'Trace message 2',
                exception: ''
            }];
        }

        return service;
    }
})();