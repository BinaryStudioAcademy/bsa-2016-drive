(function () {
    angular.module('driveApp.academyPro', [
        'youtube-embed'
    ])
        .config([
            '$routeProvider', function config($routeProvider) {
                var baseUrl = window.globalVars.baseUrl;
                $routeProvider
                    .when('/apps/academy',
                    {
                        templateUrl: baseUrl + '/Scripts/App/Academy/List/AcademyList.html',
                        controller: 'AcademyListController',
                        controllerAs: 'academyListCtrl'
                    })
                    .when('/apps/academy/:id',
                    {
                        templateUrl: baseUrl + '/Scripts/App/Academy/Single/Academy.html',
                        controller: 'AcademyController',
                        controllerAs: 'academyCtrl'
                    })
                    .when('/apps/academy/:id/lecture/:lectureId',
                    {
                        templateUrl: baseUrl + '/Scripts/App/Academy/Lecture/Lecture.html',
                        controller: 'LectureController',
                        controllerAs: 'lectureCtrl'
                    });
            }
        ]);
})();