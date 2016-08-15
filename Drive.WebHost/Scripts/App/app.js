﻿angular.module('driveApp',
    ['ngRoute', 'ui.bootstrap.contextMenu', 'ui.bootstrap'])
    .config([
        '$routeProvider',
        function ($routeProvider) {

            $routeProvider
            .when('/', // Space page
            {
                templateUrl: '/Scripts/App/Space/Space.html',
                controller: 'SpaceController',
                controllerAs: 'spaceCtrl'
            })
            .when('/settings/', // Space settings Page
            {
                templateUrl: '/Scripts/App/Space/Settings.html',
                controller: 'SettingsController',
                controllerAs: 'settingsCtrl'
            })
            .when('/Logs', // Logs page
            {
                templateUrl: '/Scripts/App/Logs/Logs.html',
                controller: 'LogsController',
                controllerAs: 'logsCtrl'
            })
            .when('/Folders', // Folders page
            {
                templateUrl: '/Scripts/App/Folder/Folder.html',
                controller: 'FolderController',
                controllerAs: 'folderCtrl'
            })
            .when('/apps/academy', // Academy Pro Page
            {
                templateUrl: '/Scripts/App/Academy/Academy.html',
                controller: 'AcademyController',
                controllerAs: 'academyCtrl'
            })
            .when('/apps/events', // Events Page
            {
                templateUrl: '/Scripts/App/Events/Events.html',
                controller: 'EventsController',
                controllerAs: 'eventsCtrl'
            })
             .when('/apps/employees', // Employees Page
            {
                templateUrl: '/Scripts/App/Employees/Employees.html',
                controller: 'EmployeesController',
                controllerAs: 'employeesCtrl'
            })
            .when('/apps/checklist', // Checklist Page
            {
                templateUrl: '/Scripts/App/Checklist/Checklist.html',
                controller: 'ChecklistController',
                controllerAs: 'checklistCtrl'
            })
            .when('/apps/trello', // Trello Page
            {
                templateUrl: '/Scripts/App/Trello/Trello.html',
                controller: 'TrelloController',
                controllerAs: 'trelloCtrl'
            })
            .when('/apps/docs', // Docs Page
            {
                templateUrl: '/Scripts/App/Docs/Docs.html',
                controller: 'DocsController',
                controllerAs: 'docsCtrl'
            })
            .when('/apps/sheets', // Sheets Page
            {
                templateUrl: '/Scripts/App/Sheets/Sheets.html',
                controller: 'SheetsController',
                controllerAs: 'sheetsCtrl'
            })
            .when('/apps/slides', // Slides Page
            {
                templateUrl: '/Scripts/App/Slides/Slides.html',
                controller: 'SlidesController',
                controllerAs: 'slidesCtrl'
            })
            .otherwise({ // This is when any route not matched - error
                controller: 'ErrorController'
            });
        }]);

