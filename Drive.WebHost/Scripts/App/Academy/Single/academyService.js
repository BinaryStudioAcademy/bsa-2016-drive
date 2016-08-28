(function () {
    'use strict';

    angular
        .module('driveApp.academyPro')
        .factory('AcademyService', AcademyService);

    AcademyService.$inject = [
        '$http',
        'BaseUrl'
    ];

    function AcademyService($http, baseUrl) {
        var service = {
            getAcademy: getAcademy,
            pushData: pushData
        }

        return service;

        function getAcademy(id) {
            return $http.get(baseUrl + '/api/academies/' + id)
                .then(getAcademyCompleted)
                .catch(getAcademyFailed);

            function getAcademyCompleted(response) {
                return response.data;
            }

            function getAcademyFailed(error) {
                console.log('XHR Failed for getAcademy.' + error.data);
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
    }
})();