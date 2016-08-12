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
         //.when('/Home/Index', // Space page
         //{
         //    templateUrl: '/Scripts/App/Space/Space.html',
         //    controller: 'SpaceController',
         //    controllerAs: 'spaceCtrl'
         //})
         .when('/', // Space settings Page
         {
             templateUrl: '/Scripts/App/Space/Settings.html',
             controller: 'SettingsController',
             controllerAs: 'settingsCtrl'
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