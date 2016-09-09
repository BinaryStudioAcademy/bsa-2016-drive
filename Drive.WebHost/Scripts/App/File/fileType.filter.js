(function () {
    'use strict';

    angular
        .module('driveApp')
        .filter('typeOfFile', function () {
            return function (input, uppercase) {
                switch (input) {
                    case 0:
                        return 'Undefined';
                    case 1:
                        return 'Document';
                    case 2:
                        return 'Sheets';
                    case 3:
                        return 'Slides';
                    case 4:
                        return 'Trello';
                    case 5:
                        return 'Link';
                    case 6:
                        return 'Physical file';
                    case 7:
                        return "AcademyPro";
                    default:
                        return '';
                }
            }
        });
})();
        
