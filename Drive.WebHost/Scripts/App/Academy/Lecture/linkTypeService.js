(function() {
    angular
        .module('driveApp.academyPro')
        .factory('LinkTypeService', LinkTypeService);

        function LinkTypeService() {
            var service = {
                none: 0,
                video: 1,
                slide: 2,
                sample: 3,
                useful: 4,
                repository: 5
            };

            return service;
        }
})();