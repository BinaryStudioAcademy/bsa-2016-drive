(function () {
    'use strict';

    angular
        .module('driveApp.events')
        .filter('contentType', function () {
            return function (input, uppercase) {
                switch (input) {
                    case 0:
                        return 'None';
                    case 1:
                        return 'Text';
                    case 2:
                        return 'Photo';
                    case 3:
                        return 'Video';
                    case 4:
                        return 'Link';
                    default:
                        return '';
                }
            }
        });
})();