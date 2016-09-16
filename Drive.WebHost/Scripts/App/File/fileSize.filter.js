(function () {
    'use strict';

    angular
        .module('driveApp')
        .filter('sizeOfFile', function () {
            return function (size) {
                if (isNaN(size))
                    size = 0;

                if (size < 1024)
                    return size + ' Bytes';

                size /= 1024;

                if (size < 1024)
                    return size.toFixed(2) + ' Kb';

                size /= 1024;

                if (size < 1024)
                    return size.toFixed(2) + ' Mb';
            }
        });
})();