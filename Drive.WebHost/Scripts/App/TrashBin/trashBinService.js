(function () {
    'use strict';

    angular
        .module('driveApp')
        .factory('TrashBinService', TrashBinService);

    TrashBinService.$inject = ['$http', 'BaseUrl'];

    function TrashBinService($http, baseUrl) {
        var service = {
            getTrashBinContent: getTrashBinContent,
            deleteFilePermanently: deleteFilePermanently,
            restoreFile: restoreFile,
            orderByColumn: orderByColumn
        };

        function getTrashBinContent(callBack) {
            $http.get(baseUrl + '/api/trashbin')
                .then(function (response) {
                    if (callBack) {
                        callBack(response.data);
                    }
                });
        }

        function deleteFilePermanently(id, callBack) {
            $http.delete(baseUrl + '/api/trashbin/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        function restoreFile(id, callBack) {
            $http.put(baseUrl + '/api/trashbin/' + id)
            .then(function (response) {
                if (callBack) {
                    callBack(response.data);
                }
            });
        }

        function orderByColumn(columnClicked, columnCurrent) {
            var pos = columnCurrent.indexOf(columnClicked);
            if (pos == 0) { return '-' + columnClicked; }
            return columnClicked;
        }

        return service;
    }
})();