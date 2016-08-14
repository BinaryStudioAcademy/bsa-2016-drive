angular.module('driveApp',
    ['ngRoute', 'ui.bootstrap.contextMenu', 'ui.bootstrap'])
    .config([
        '$routeProvider',
        function ($routeProvider) {

            $routeProvider
            .when('/', // Home Page
            {
                templateUrl: '/Scripts/App/Space/Space.html',
                controller: 'HomeController',
                controllerAs: 'homeCtrl'
            })
            //.when('/', // Space page
            //{
            //    templateUrl: '/Scripts/App/Folder/Folder.html',
            //    controller: 'FolderController',
            //    controllerAs: 'folderCtrl'
            //})
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
            .otherwise({ // This is when any route not matched - error
                controller: 'ErrorController'
            });
        }]);

