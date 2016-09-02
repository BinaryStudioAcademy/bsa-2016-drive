/// <reference path="fileService.js" />
(function() {
    'use strict';

    angular
        .module('driveApp')
        .factory('FileService', FileService);

    FileService.$inject = ['$http', 'BaseUrl'];

    function FileService($http, baseUrl) {

        var service = {
            createFile: createFile,
            updateFile: updateFile,
            updateDeletedFile: updateDeletedFile,
            deleteFile: deleteFile,
            getFile: getFile,
            getDeletedFile: getDeletedFile,
            createCopyFile: createCopyFile,
            getAllFiles: getAllFiles,
            getFilesApp: getFilesApp,
            getAllByParentId: getAllByParentId,
            orderByColumn: orderByColumn,
            searchFiles: searchFiles,
            openFile: openFile,
            chooseIcon: chooseIcon,
            uploadFile: uploadFile
        };

        function getAllFiles(callBack) {
            $http.get(baseUrl + '/api/files')
                .success(function(response) {
                    if (callBack) {
                        callBack(response);
                    }
                });
        }

        function getAllByParentId(spaceId, parentId, callBack) {
            $http.get(baseUrl + '/api/files/parent?spaceId=' + spaceId + '&parentId=' + parentId)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting files!');
                    });
        }

        function getFilesApp(fileType, callBack) {
            $http.get(baseUrl + '/api/files/apps/' + fileType)
                .then(function(response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                });
        }

        function searchFiles(fileType, text, callback) {
            $http.get(baseUrl + '/api/files/apps/' + fileType + '/search', {
                params: {
                    fileType: fileType,
                    text: text
                }
            })
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            }, function errorCallback(response) {
                console.log('Error in searchFiles Method! Code:' + response.status);
                if (response.status == 404 && callback) {
                    callback(response.data)
                }
            });
        }

        function getFile(id, callBack) {
            $http.get(baseUrl + '/api/files/' + id)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting file!');
                    });
        }

        function getDeletedFile(id, callBack) {
            $http.get(baseUrl + '/api/files/deleted/' + id)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting file!');
                    });
        }

        function createFile(file, callBack) {
            $http.post(baseUrl + '/api/files', file)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting file!');
                    });
        }

        function uploadFile(spaceId, parentId, file, callBack) {

            var fd = new FormData();
            fd.append('file', file);

            $http.post('api/files/' + spaceId, fd, {
                params:
                    {
                        parentId: parentId
                    },
                withCredentials: false,
                headers: {
                    'Content-Type': undefined
                },
                transformRequest: angular.identity
            })
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            },
            function () {
                console.log('Error while uploading file!');
            });
        }

        function updateFile(id, file, callBack) {
            $http.put(baseUrl + '/api/files/' + id, file)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting file!');
                    });
        }

        function updateDeletedFile(id, oldParentId, file, callBack) {
            $http.put(baseUrl + '/api/files/deleted/' + id + '?oldParentId=' + oldParentId, file)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting file!');
                    });
        }

        function createCopyFile(id, file, callBack) {
            $http.put(baseUrl + '/api/files/copied/' + id, file)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting file!');
                    });
        }

        function deleteFile(id, callBack) {
            $http.delete(baseUrl + '/api/files/' + id)
                .then(function(response) {
                        if (callBack) {
                            callBack(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting file!');
                    });
        }

        function orderByColumn(columnClicked, columnCurrent) {
            var pos = columnCurrent.indexOf(columnClicked);
            if (pos == 0) { return '-' + columnClicked; }
            return columnClicked;
        }

        function openFile(url) {
            window.open(url, '_blank');
        }

        function chooseIcon(type) {
            switch (type) {
                case 0:
                    return 'Undefined';
                case 1:
                    return "./Content/Icons/doc.svg";
                case 2:
                    return "./Content/Icons/xls.svg";
                case 3:
                    return "./Content/Icons/ppt.svg";
                case 4:
                    return "./Content/Icons/trello.svg";
                case 5:
                    return "./Content/Icons/link.svg";
                case 6:
                    return "";
                default:
                    return "./Content/Icons/folder.svg";
            }
        }

        return service;
    }
})();