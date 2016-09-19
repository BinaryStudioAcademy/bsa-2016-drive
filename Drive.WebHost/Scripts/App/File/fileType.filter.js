(function () {
    'use strict';

    angular
        .module('driveApp')
        .filter('typeOfFile', function () {
            return function (input, uppercase) {
                switch (input) {
                    //case 0:
                    //    return 'Link';
                    case 1:
                        return 'Document';
                    case 2:
                        return 'Sheet';
                    case 3:
                        return 'Slide';
                    case 4:
                        return 'Trello';
                    case 5:
                        return 'Link';
                    case 6:
                        return 'Physical file';
                    case 7:
                        return "AcademyPro";
                    case 8:
                        return "Image";
                    case 9:
                        return "Event";
                    case 10:
                        return "Video";
                    default:
                        return '';
                }
            }
        })
})();
        
