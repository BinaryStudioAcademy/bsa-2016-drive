angular.module('driveApp',
    ['ngRoute'])

 .config(['$routeProvider',
     function ($routeProvider) {

         $routeProvider
         .when('/', // Home Page
         {
             templateUrl: '/Scripts/App/Space/Space.html',
             controller: 'HomeController',
             controllerAs: 'homeCtrl'
         })
         .when('/Home/Index', // Home Page
         {
             templateUrl: '/Scripts/App/Space/Space.html',
             controller: 'HomeController',
             controllerAs: 'homeCtrl'
         })
         .otherwise({ // This is when any route not matched - error
             controller: 'ErrorController'
         });
 }]);