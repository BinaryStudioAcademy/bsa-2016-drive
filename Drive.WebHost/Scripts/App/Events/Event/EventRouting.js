(function () {
    angular.module('driveApp.events', [
        'youtube-embed',
        'ui.bootstrap',
        'ui.bootstrap.datetimepicker'
    ])
        .config([
            '$routeProvider', function config($routeProvider) {
                var baseUrl = window.globalVars.baseUrl;
                $routeProvider
                    .when('/apps/event',
                    {
                        templateUrl: baseUrl + '/Scripts/App/Event/EventList.html',
                        controller: 'EventListController',
                        controllerAs: 'eventListCtrl'
                    })
                    .when('/apps/events/:id',
                    {
                        templateUrl: baseUrl + '/Scripts/App/Event/Event.html',
                        controller: 'EventController',
                        controllerAs: 'eventCtrl'
                    })
                    .when('/apps/event/newevent',
                    {
                        templateUrl: baseUrl + '/Scripts/App/Event/EventContent/CreateEvent.html',
                        controller: 'EventCreateController',
                        controllerAs: 'eventCreateCtrl'
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