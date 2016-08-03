angular.module('driveApp',
    ['ngRoute'])

 .config(['$routeProvider',
     function ($routeProvider) {

         $routeProvider
         .when('/', // Home Page
         {
             templateUrl: '/Scripts/App/Home/Home.html',
             controller: 'homeController'
         })
         .when('/Home/Index', // Home Page
         {
             templateUrl: '/Scripts/App/Home/Home.html',
             controller: 'homeController'
         })
         .otherwise({ // This is when any route not matched - error
             controller: 'ErrorController'
         });
 }]);