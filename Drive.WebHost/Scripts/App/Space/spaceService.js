(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('SpaceService', SpaceService);

    SpaceService.$inject = ['$http', "BaseUrl"];

    function SpaceService($http, baseUrl) {
        var service = {
            getSpace: getSpace,
            getSpaceByType: getSpaceByType,
            getAllSpaces: getAllSpaces,
            getAllUsers: getAllUsers,
            pushData: pushData,
            searchFoldersAndFiles: searchFoldersAndFiles,
            getNumberOfResultSearchFoldersAndFiles: getNumberOfResultSearchFoldersAndFiles,
            getSpaceTotal: getSpaceTotal,
            getAllRoles: getAllRoles,
            moveContent: moveContent,
            copyContent: copyContent
        };
        function getAllRoles(callback) {
            $http.get(baseUrl + '/api/roles')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting roles!');
            })
        }

        function getSpace(id, currentPage, pageSize, sort, callback) {
            $http.get(baseUrl + '/api/spaces/' + id, {
                params: {
                    page: currentPage,
                    count: pageSize,
                    sort: sort
                }
            })
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                },
                function errorCallback(response) {
                    console.log('Error getSpace spaceService!' + response.status);
                    if (response.status == 404 && callback) {
                        console.log(response.status + ' ' + response.data);
                        callback(response.data)
                    }
                });
        }

        function getSpaceByType(type, callback) {
            $http.get(baseUrl + '/api/spaces/spacetype/' + type)
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            },
            function errorCallback(response) {
                console.log('Error getSpaceByType spaceService!' + response.status);
                if (response.status == 404 && callback) {
                    console.log(response.status + ' ' + response.data);
                    callback(response.data)
                }
            });
        }

        function getSpaceTotal(id, callback) {
            $http.get(baseUrl + '/api/spaces/' + id + '/sptotal')
               .then(function (response) {
                   if (callback) {
                       callback(response.data);
                   }
               },
               function errorCallback(response) {
                   console.log('Error getSpaceTotal spaceService!' + response.status);
                   if (response.status == 404 && callback) {
                       console.log(response.status + ' ' + response.data);
                       callback(response.data)
                   }
               });

        }

        function getAllSpaces(callback) {
            $http.get(baseUrl + '/api/spaces')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting all spaces!');
            });
        }

        function getAllUsers(callback) {
            $http.get(baseUrl + '/api/users')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting all users!');
            });
        }

        function pushData(data) {
            $http.post(baseUrl + '/api/spaces', data)
                .then(function () {
                    console.log('Success!');
                }, function () {
                    console.log('Error while pushing data!');
                });
        }

        function searchFoldersAndFiles(spaceId, folderId, text, currentPage, pageSize, callback) {
            $http.get(baseUrl + '/api/spaces/' + spaceId + '/search', {
                params: {
                    folderId: folderId,
                    text: text,
                    page: currentPage,
                    count: pageSize
                }
            })
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function errorCallback(response) {
                console.log('Error in searchFoldersAndFiles Method! Code:' + response.status);
                if (response.status == 404 && callback) {
                    callback(response.data)
                }
            });
        }

        function getNumberOfResultSearchFoldersAndFiles(spaceId, folderId, text, callback) {
            $http.get(baseUrl + '/api/spaces/' + spaceId + '/total', {
                params: {
                    folderId: folderId,
                    text: text
                }
            })
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function errorCallback(response) {
                console.log('Error in getNumberOfResultSearchFoldersAndFiles Method! Code:' + response.status);
                if (response.status == 404 && callback) {
                    callback(response.data)
                }
            });
        }

        function moveContent(content, callBack) {
            $http.put(baseUrl + '/api/spaces/movecontent', content)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while moving data! [spaceService.moveContent()]');
                    });
        }

        function copyContent(content, callBack) {
            $http.put(baseUrl + '/api/spaces/copycontent', content)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while copying data! [spaceService.copyContent()]');
                    });
        }

        return service;
    }
})();