(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('ShareByLinkService', ShareByLinkService);

    ShareByLinkService.$inject = ['$http', "BaseUrl"];

    function ShareByLinkService($http, baseUrl) {
        var service = {
            setShareLink: setShareLink,
            getLink: getLink
        };

        function setShareLink(content, callback) {
            $http.put(baseUrl + '/api/share/setlink', content)
            .then(function (response) {
                if (callback) {
                    callback(response.data)
                }
            }, function errorCallback(response) {
                console.log('Error in setShaerLink shareByLinkService! Code:' + response.status);
            });
        }

        function getLink(shareLinkId, callback) {
            $http.get(baseUrl + '/api/share/getlink/' + shareLinkId)
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function errorCallback(response) {
                console.log('Error in getPermissions sharedSpaceService! Code:' + response.status);
            });
        }

        return service;
    }
})();