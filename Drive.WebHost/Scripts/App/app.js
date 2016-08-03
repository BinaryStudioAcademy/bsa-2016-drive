angular.module('driveApp',
    ['ngRoute'])

 .config(['$routeProvider',
     function ($routeProvider) {

         $routeProvider
         .when('/', // Home Page
         {
             templateUrl: '/Scripts/App/Home/Home.html',
             controller: 'homeController',
             controllerAs: 'vm'
         })
         .when('/Home/Index', // Home Page
         {
             templateUrl: '/Scripts/App/Home/Home.html',
             controller: 'homeController',
             controllerAs: 'vm'
         })
         .otherwise({ // This is when any route not matched - error
             controller: 'ErrorController'
         });
 }]);