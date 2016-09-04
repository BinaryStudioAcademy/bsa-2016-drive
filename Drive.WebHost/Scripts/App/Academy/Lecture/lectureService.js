(function () {
    'use strict';

    angular
        .module('driveApp.academyPro')
        .factory('LectureService', LectureService);

    LectureService.$inject = [
        '$http',
        'BaseUrl'
    ];

    function LectureService($http, baseUrl) {
        var service = {
            getLecture: getLecture,
            pushData: pushData
        }

        return service;

        function getLecture(id) {
            return $http.get(baseUrl + '/api/lectures/' + id)
                .then(getLectureCompleted)
                .catch(getLectureFailed);

            function getLectureCompleted(response) {
                return response.data;
            }

            function getLectureFailed(error) {
                console.log('XHR Failed for getLecture.' + error.data);
            }
        }

        function pushData(data) {
            return $http.post(baseUrl + '/api/lectures', data)
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