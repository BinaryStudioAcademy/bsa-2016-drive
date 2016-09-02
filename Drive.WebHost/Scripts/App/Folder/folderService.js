(function() {
    'use strict';

    angular
        .module('driveApp')
        .factory('FolderService', FolderService);

    FolderService.$inject = ['$http', 'BaseUrl'];

    function FolderService($http, baseUrl) {
        var service = {
            getAll: getAll,
            getAllByParentId: getAllByParentId,
            get: get,
            getDeleted: getDeleted,
            create: create,
            updateFolder: updateFolder,
            updateDeleted: updateDeleted,
            createCopy: createCopy,
            deleteFolder: deleteFolder,
            getContent: getContent,
            getFolderContentTotal: getFolderContentTotal
        };

        function getAll(callBack) {
            $http.get(baseUrl + '/api/folders')
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting folder!');
                    });
        }

        function getAllByParentId(spaceId, parentId, callBack) {
            $http.get(baseUrl + '/api/folders/parent?spaceId=' + spaceId + '&parentId=' + parentId)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting folder!');
                    });
        }

        function get(id, currentPage, pageSize, callBack) {
            $http.get(baseUrl + '/api/folders/' + id)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting folder!');
                    });
        }

        function getDeleted(id, callBack) {
            $http.get(baseUrl + '/api/folders/deleted/' + id)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting folder!');
                    });
        }

        function create(data, callBack) {
            $http.post(baseUrl + '/api/folders', data)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting folder!');
                    });
        }

        function updateFolder(data, callback) {
            $http.put(baseUrl + '/api/folders/' + data.id, data)
                .then(function(response) {
                        if (callback) {
                            callback(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting folder!');
                    });
        }

        function updateDeleted(id, oldParentId, folder, callBack) {
            $http.put(baseUrl + '/api/folders/deleted/' + id + '?oldParentId=' + oldParentId, folder)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting file!');
                    });
        }

        function createCopy(id, folder, callBack) {
            $http.put(baseUrl + '/api/folders/copied/' + id, folder)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting folder!');
                    });
        }

        function deleteFolder(id, callback) {
            $http.delete(baseUrl + '/api/folders/' + id)
                .then(function(response) {
                    if (callback)
                        callback(response);
                });
        }

        function getContent(id, currentPage, pageSize, sort, callback) {
            $http.get(baseUrl + '/api/content/' + id, {
                params: {
                    page: currentPage,
                    count: pageSize,
                    sort: sort
                }
            }).then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function () {
                console.log('Error getContent folderService!');
            });
        }

        function getFolderContentTotal(id, callback) {
            $http.get(baseUrl + '/api/content/' + id + '/total')
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                }, function () {
                    console.log('Error getFolderContentTotal folderService!');
            });
        }

        return service;
    }
})();