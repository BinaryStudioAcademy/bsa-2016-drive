angular.module('driveApp',
    ['ngRoute'])

 .config(['$routeProvider',
     function ($routeProvider) {

         $routeProvider
         //.when('/', // Home Page
         //{
         //    templateUrl: '/Scripts/App/Space/Space.html',
         //    controller: 'HomeController',
         //    controllerAs: 'homeCtrl'
         //})
         .when('/', // Space page
         {
             templateUrl: '/Scripts/App/Space/Space.html',
             controller: 'SpaceController',
             controllerAs: 'spaceCtrl'
         })
         //.when('/', // Space settings Page
         //{
         //    templateUrl: '/Scripts/App/Space/Settings.html',
         //    controller: 'SettingsController',
         //    controllerAs: 'settingsCtrl'
         //})
         .when('/Logs', // Logs page
         {
             templateUrl: '/Scripts/App/Logs/Logs.html',
             controller: 'LogsController',
             controllerAs: 'logsCtrl'
         })
         .whet('/file_list',
         {
             templateUrl: '/Scripts/App/File/FileList.html',
             controller: 'FileController',
             controllerAs: 'fileCtrl'
         })
         .when('/create_file',
         {
             templateUrl: '/Scripts/App/File/FileForm.html',
             controller: 'FileController',
             controllerAs: 'fileCtrl'
         })
         .when('/edit/:id',
         {
             templateUrl: '/Scripts/App/File/FileForm.html',
             controller: 'FileController',
             controllerAs: 'fileCtrl'
         })
         .otherwise({ // This is when any route not matched - error
             controller: 'ErrorController'
         });
     }]);