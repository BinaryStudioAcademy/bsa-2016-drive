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
            uploadFile: uploadFile,
            downloadFile: downloadFile,
            getFileNameFromHeader: getFileNameFromHeader,
            getFileName: getFileName,
            getFileExtension: getFileExtension, 
            checkFileSize: checkFileSize,
            checkFilesValidationProperty: checkFilesValidationProperty,
            findCourse: findCourse,
            findEvent: findEvent,
            fileTextReader: fileTextReader,
            getImage: getImage
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
                },
                function errorCallback(response) {
                    console.log('Error in getFilesApp Method! Code:' + response.status);
                    if (response.status === 404 && callBack) {
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

        function uploadFile(spaceId, parentId, files, filedata, callBack) {

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append("file" + i, files[i]);
                data.append("data" + i, JSON.stringify(filedata));
            }

            $http.post(baseUrl + '/api/files/upload?spaceId=' + spaceId + '&parentId=' + parentId, data,
                {
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
            function (response) {
                if (callBack) {
                    console.log('Error while uploading file! Error: ' + response.data.message);
                }
            });
        }
        function downloadFile(fileId, callBack) {
            $http({
                    method: 'GET',
                    url: baseUrl + '/api/files/download?fileId=' + fileId,
                    responseType: 'arraybuffer'
                })
                .success(function(data, status, headers) {
                    headers = headers();
                    var fileName = getFileNameFromHeader(headers['content-disposition']);
                    var contentType = headers['content-type'];

                    try {
                        console.log("Trying save file:");
                        console.log(fileName);
                        var blob = new Blob([data], { type: contentType });
                        var url = URL.createObjectURL(blob);
                        var a = document.createElement('a');
                        a.href = url;
                        a.download = fileName;
                        a.target = '_blank';
                        a.click();
                        console.log("File saving succeeded");
                        success = true;
                    } catch (ex) {
                        console.log("Fale saving failed with the following exception:");
                        console.log(ex);
                    }
                });
        }
        function getFileName(fileName) {
            var name = fileName.substr(0, fileName.lastIndexOf('.'));
            return name;
        }
        function getFileExtension(fileName) {
            var extension = fileName.substr(fileName.lastIndexOf('.'));
            return extension;
        }
        function checkFileSize(size, maxSize) {
            if ((size / 1024) / 1024 > maxSize) {
                return false;
            }
            else return true;
        }
        function checkFilesValidationProperty(array) {
            for (var i = 0; i < array.length; i++) {
                if (!array[i].isValid) {
                    return false;
                }
            }
            return true;
        }

        function fileTextReader(fileId, callBack) {
            $http({
                method: 'GET',
                url: baseUrl + '/api/files/download?fileId=' + fileId,
                responseType: 'arraybuffer'
            })
                            .then(function (response) {
                                if (callBack) {
                                    callBack(response);
                                }
                            });
        }

        function getImage(fileId, callBack) {
            $http({
                method: 'GET',
                url: baseUrl + '/api/files/download?fileId=' + fileId,
                responseType: 'arraybuffer'
            })
                            .then(function (response) {
                                if (callBack) {
                                    callBack(response);
                                }
                            });
        }

        function getFileNameFromHeader(header) {
            var result = header.split(';')[1].trim().split('=')[1];

            return result.replace(/"/g, '');
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
                        console.log('Error while deleting file!');
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

        function findCourse(id, callBack) {
            $http.get(baseUrl + '/api/files/apps/findcourse/' + id)
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting course!');
                    });
        }

        function findEvent(id, callback) {
            $http.get(baseUrl + '/api/files/apps/findevent/' + id)
            .then(function (response) {
                if (callback) {
                    callback(response.data);
                }
            },
            function () {
                console.log('Error while getting event!');
            });
        }

        function chooseIcon(type) {
            switch (type) {
                case 0:
                    return "./Content/Icons/link.svg";
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
                    return "./Content/Icons/physical-file.svg";
                case 7:
                    return "./Content/Icons/academyPro.svg";
                case 8:
                    return "./Content/Icons/image.svg";
                case 9:
                    return "./Content/Icons/event.svg";
                case 10:
                    return "./Content/Icons/video.svg";
                default:
                    return "./Content/Icons/link.svg";
            }
        }

        return service;
    }
})();