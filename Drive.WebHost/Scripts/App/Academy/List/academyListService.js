(function() {
    'use strict';

    angular
        .module('driveApp.academyPro')
        .factory('AcademyListService', AcademyListService);

    AcademyListService.$inject = ['$http'];

    function AcademyListService($http) {
        var service = {
            getAcademies: getAcademies,
            pushData: pushData
        }

        return service;

        function getAcademies() {
            return $http.get('/api/academies')
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
            return $http.post('/api/academies', data)
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