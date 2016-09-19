(function() {
    'use strict';

    angular
        .module('driveApp.academyPro')
        .factory('AcademyListService', AcademyListService);

    AcademyListService.$inject = [
        '$http',
        'BaseUrl'
    ];

    function AcademyListService($http, baseUrl) {
        var service = {
            getAcademies: getAcademies,
            pushData: pushData,
            putData: putData,
            deleteData: deleteData,
            getAllUsers: getAllUsers,
            searchCourses: searchCourses,
            orderCoursesByColumn: orderCoursesByColumn,
            getAllTags: getAllTags
        }

        return service;

        function getAcademies() {
            return $http.get(baseUrl + '/api/academies')
                .then(getAcademiesCompleted)
                .catch(getAcademiesFailed);

            function getAcademiesCompleted(response) {
                return response.data;
            }

            function getAcademiesFailed(error) {
                console.log('XHR Failed for getAcademies.' + error.data);
            }
        }

        function pushData(data, callback) {
            return $http.post(baseUrl + '/api/academies', data)
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                }
            },
                function () {
                    console.log('Error while course creation!');
                });
        }

        function putData(id, data, callback) {
            $http.put(baseUrl + '/api/academies/' + id, data)
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                },
                    function () {
                        console.log('Error while course editing!');
                    });
        }

        function deleteData(id, callback) {
            $http.delete(baseUrl + '/api/academies/' + id)
                .then(function(response) {
                    if(callback) {
                        callback(response);
                    }
                },
                function () {
                        console.log('Error while course deletion!');
                    });
        }

        function getAllUsers(callback) {
            $http.get(baseUrl + '/api/users')
                .then(function(response) {
                        if (callback) {
                            callback(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting all users!');
                    });
        }

        function searchCourses(text, callback) {
            $http.get(baseUrl + '/api/academies/search',
                {
                    params: {
                        text: text
                    }
                })
                .then(function(response) {
                        if (callback) {
                            callback(response.data);
                        }
                    },
                    function errorCallback(response) {
                        console.log('Error in searchCourses Method! Code:' + response.status);
                        if (response.status === 404 && callback) {
                            callback(response.data);
                        }
                    });
        }

        function orderCoursesByColumn(columnClicked, columnCurrent) {
            var pos = columnCurrent.indexOf(columnClicked);
            if (pos == 0) { return '-' + columnClicked; }
            return columnClicked;
        }

        function getAllTags(callback) {
            $http.get(baseUrl + '/api/tags')
                .then(function(response) {
                        if (callback) {
                            callback(response.data);
                        }
                    },
                    function() {
                        console.log('Error while getting all tags!');
                    });
        }
    }
})();