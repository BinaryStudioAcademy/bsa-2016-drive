(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('TrashBinService', TrashBinService);

    TrashBinService.$inject = ['$http', 'BaseUrl'];

    function TrashBinService($http, baseUrl) {
        var service = {
            getTrashBinContent: getTrashBinContent
        };

        function getTrashBinContent(callBack) {
            $http.get(baseUrl + '/api/trashbin')
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                });
        }

        return service;
    }
})();