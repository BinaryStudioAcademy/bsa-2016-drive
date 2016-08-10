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
         //.when('/', // Space settings Page
         //{
         //    templateUrl: '/Scripts/App/Space/Settings.html',
         //    controller: 'SettingsController',
         //    controllerAs: 'settingsCtrl'
         //})
         .when('/', // Space settings Page
         {
             templateUrl: '/Scripts/App/Folders/Get.html',
             controller: 'FoldersController',
             controllerAs: 'foldersCtrl'
         })
         .otherwise({ // This is when any route not matched - error
             controller: 'ErrorController'
         });
 }]);