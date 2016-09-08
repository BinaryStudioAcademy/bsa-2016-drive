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
            searchCourses: searchCourses
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

        function pushData(data) {
            return $http.post(baseUrl + '/api/academies', data)
                .then(pushDataCompleted)
                .catch(pushDataFailed);

            function pushDataCompleted(response) {
                return response.data;
            }

            function pushDataFailed(error) {
                console.log('XHR Failed for pushData.' + error.data);
            }
        }

        function putData(id, data) {
            $http.put(baseUrl + '/api/academies/' + id, data)
                .then(putDataCompleted)
                .catch(putDataFailed);

            function putDataCompleted() {
            }

            function putDataFailed(error) {
                console.log('XHR Failed for putData.' + error.data);
            }
        }

        function deleteData(id) {
            $http.delete(baseUrl + '/api/academies/' + id)
                .then(deleteDataCompleted)
                .catch(deleteDataFailed);

            function deleteDataCompleted() {
            }

            function deleteDataFailed(error) {
                console.log('XHR Failed for putData.' + error.data);
            }
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
    }
})();