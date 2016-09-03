(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('TrashBinService', TrashBinService);

    TrashBinService.$inject = ['$http', 'BaseUrl'];

    function TrashBinService($http, baseUrl) {
        var service = {
            getTrashBinContent: getTrashBinContent,
            searchTrashBin: searchTrashBin,
            deleteFilePermanently: deleteFilePermanently,
            deleteFolderPermanently: deleteFolderPermanently,
            restoreFile: restoreFile,
            restoreFolder: restoreFolder,
            restoreSpaces: restoreSpaces,
            orderByColumn: orderByColumn
        };

        function getTrashBinContent(callBack) {
            $http.get(baseUrl + '/api/trashbin/')
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                });
        }

        function searchTrashBin(text, callBack) {
            $http.get(baseUrl + '/api/trashbin/', {
                params: {
                    text: text
                }
            })
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                });
        }

        function deleteFilePermanently(id, callBack) {
            $http.delete(baseUrl + '/api/trashbin/file/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        function deleteFolderPermanently(id, callBack) {
            $http.delete(baseUrl + '/api/trashbin/folder/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        function restoreFile(id, callBack) {
            $http.put(baseUrl + '/api/trashbin/file/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        function restoreFolder(id, callBack) {
            $http.put(baseUrl + '/api/trashbin/folder/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        function restoreSpaces(spaces, callBack) {
            $http.put(baseUrl + '/api/trashbin/spaces', spaces)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        function orderByColumn(columnClicked, columnCurrent) {
            var pos = columnCurrent.indexOf(columnClicked);
            if (pos === 0) { return '-' + columnClicked; }
            return columnClicked;
        }

        return service;
    }
})();