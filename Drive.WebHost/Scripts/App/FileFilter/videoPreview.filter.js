(function() {
    'use strict';

    angular
        .module('driveApp')
        .filter('getVideoId',
            function() {
                return function(input) {

                    var output = input.substr(input.indexOf("=") + 1);

                    return output;
                }
            });
})();
