(function() {
    angular
        .module('driveApp.events')
        .factory('ContentTypeService', ContentTypeService);

    function ContentTypeService() {
            var service = {
                none: 0,
                text: 1,
                photo: 2,
                video: 3,
                link: 4
            };

            return service;
        }
})();