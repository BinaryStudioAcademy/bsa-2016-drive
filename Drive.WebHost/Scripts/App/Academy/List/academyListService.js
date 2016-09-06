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
            putData : putData
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

        function putData(data, callback) {
            $http.put(baseUrl + '/api/academies/' + data.id, data)
                .then(function (response) {
                    if (callback) {
                        callback(response.data);
                    }
                },
                    function () {
                        console.log('Error while getting folder!');
                    });
        }
    }
})();