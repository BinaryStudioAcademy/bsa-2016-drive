(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('SharedSpaceService', SharedSpaceService);

    SharedSpaceService.$inject = ['$http', "BaseUrl"];

    function SharedSpaceService($http, baseUrl) {
        var service = {
            getSharedData: getSharedData,
            getSharedDataTotal: getSpaceTotal,
            getAllUsers: getAllUsers,
            deleteSharedContent: deleteSharedContent,
            search: search,
            searchTotal: searchTotal,
            getPermissions: getPermissions,
            createOrUpdatePermission: createOrUpdatePermission
        };

        function getSharedData(currentPage, pageSize, sort, folderId, rootFolderId, callback) {
            $http.get(baseUrl + '/api/sharedspace', {
                params: {
                    page: currentPage,
                    count: pageSize,
                    sort: sort,
                    folderId: folderId,
                    rootFolderId: rootFolderId
                }
            })
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                },
                function errorCallback(response) {
                    console.log('Error in getSharedData sharedSpaceService!' + response.status);
                    if (response.status == 404 && callback) {
                        console.log(response.status + ' ' + response.data);
                        callback(response.data)
                    }
                });
        }
        
        function getSpaceTotal( callback) {
            $http.get(baseUrl + '/api/sharedspace/total')
               .then(function (response) {
                   if (callback) {
                       callback(response.data);
                   }
               },
               function errorCallback(response) {
                   console.log('Error in getSharedDataTotal sharedSpaceService!' + response.status);
                   if (response.status == 404 && callback) {
                       console.log(response.status + ' ' + response.data);
                       callback(response.data)
                   }
               });

        }

        function search(text, currentPage, pageSize, callback) {
            $http.get(baseUrl + '/api/sharedspace/search', {
                params: {
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
                console.log('Error in search sharedSpaceService! Code:' + response.status);
                if (response.status == 404 && callback) {
                    callback(response.data)
                }
            });
        }

        function searchTotal(text, callback) {
            $http.get(baseUrl + '/api/sharedspace/searchtotal', {
                params: {
                    text: text
                }
            })
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function errorCallback(response) {
                console.log('Error in searchTotal sharedSpaceService! Code:' + response.status);
                if (response.status == 404 && callback) {
                    callback(response.data)
                }
            });
        }

        function deleteSharedContent(id, callback) {
            $http.delete(baseUrl + '/api/sharedspace', {
                params: {
                    id: id
                }
            })
            .then(function (response) {
                if (callback)
                    callback();
                console.log('Deleted permissions successful. method: deleteSharedContent')
            }, function errorCallback(response) {
                console.log('Error in deleteSharedContent sharedSpaceService! Code: ' + response.status);
            });
        }

        function getPermissions(id, callback) {
            $http.get(baseUrl + '/api/sharedspace/permission', {
                params: {
                    id: id
                }
            })
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function errorCallback(response) {
                console.log('Error in getPermissions sharedSpaceService! Code:' + response.status);
                if (response.status == 404 && callback) {
                    callback(response.data)
                }
            });
        }

        function createOrUpdatePermission(users, id, callback) {
            $http.post(baseUrl + '/api/sharedspace/permission?id=' + id, users)
            .then(function (response) {
                if (callback) {
                    callback(response.data)
                }
            }, function errorCallback(response) {
                console.log('Error in createOrUpdatePermission sharedSpaceService! Code:' + response.status);
            });
        }

        function getAllUsers(callback) {
            $http.get(baseUrl + '/api/users/withoutcurrent')
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error while getting all users!');
            });
        }

        return service;
    }
})();