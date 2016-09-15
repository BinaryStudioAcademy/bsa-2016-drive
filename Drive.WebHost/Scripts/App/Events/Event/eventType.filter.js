(function () {
    'use strict';

    angular
        .module('driveApp.events')
        .filter('eventType', function () {
            return function (input, uppercase) {
                switch (input) {
                    case 0:
                        return 'None';
                    case 1:
                        return 'Ceremonial';
                    case 2:
                        return 'Educational';
                    case 3:
                        return 'NetWorking';
                    case 4:
                        return 'Entertainment';
                    default:
                        return '';
                }
            }
        });
})();