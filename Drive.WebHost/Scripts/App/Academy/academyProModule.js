(function () {
    angular.module('driveApp.academyPro', [])
        .config([
            '$routeProvider', function config($routeProvider) {
                $routeProvider
                    .when('/apps/academy',
                    {
                        templateUrl: '/Scripts/App/Academy/List/AcademyList.html',
                        controller: 'AcademyListController',
                        controllerAs: 'academyListCtrl'
                    })
                    .when('/apps/academy/:id',
                    {
                        templateUrl: '/Scripts/App/Academy/Single/Academy.html',
                        controller: 'AcademyController',
                        controllerAs: 'academyCtrl'
                    })
                    .when('/apps/academy/:id/lecture/:lectureId',
                    {
                        templateUrl: '/Scripts/App/Academy/Lecture/Academy.html',
                        controller: 'LectureController',
                        controllerAs: 'lectureCtrl'
                    });
            }
        ]);
})();