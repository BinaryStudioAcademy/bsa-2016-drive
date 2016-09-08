(function () {
    angular.module('driveApp.academyPro', [
        'youtube-embed',
        'ui.bootstrap',
        'ui.bootstrap.datetimepicker',
        'ngTagsInput'
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
                    .when('/apps/academies/:tagName',
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
                    .when('/apps/academy/:id/newlecture',
                    {
                        templateUrl: baseUrl + '/Scripts/App/Academy/Lecture/Create.html',
                        controller: 'LectureCreateController',
                        controllerAs: 'lectureCreateCtrl'
                    })
                    .when('/apps/academy/:id/lecture/:lectureId',
                    {
                        templateUrl: baseUrl + '/Scripts/App/Academy/Lecture/Lecture.html',
                        controller: 'LectureController',
                        controllerAs: 'lectureCtrl'
                    });
            }
        ])
    .constant('uiDatetimePickerConfig', {
        dateFormat: 'yyyy-MM-dd HH:mm',
        defaultTime: '10:00:00',
        html5Types: {
            date: 'yyyy-MM-dd',
            'datetime-local': 'yyyy-MM-dd HH:mm',
            'month': 'yyyy-MM'
        },
        initialPicker: 'date',
        reOpenDefault: false,
        enableDate: true,
        enableTime: true,
        buttonBar: {
            show: true,
            now: {
                show: true,
                text: 'Now'
            },
            today: {
                show: true,
                text: 'Today'
            },
            clear: {
                show: true,
                text: 'Clear'
            },
            date: {
                show: true,
                text: 'Date'
            },
            time: {
                show: true,
                text: 'Time'
            },
            close: {
                show: true,
                text: 'Close'
            }
        },
        closeOnDateSelection: true,
        closeOnTimeNow: true,
        appendToBody: false,
        altInputFormats: [],
        ngModelOptions: {},
        saveAs: 'ISO',
        readAs: 'ISO'
    });
})();