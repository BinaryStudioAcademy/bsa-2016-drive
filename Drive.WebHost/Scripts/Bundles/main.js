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
(function() {
    'use strict';

    angular
        .module('driveApp')
        .factory('HomeService', HomeService);

    HomeService.$inject = [];

    function HomeService() {
        var service = {
            getMessage: getMessage
        };

        function getMessage() {
            return "Coming soon... Stay tuned...";
        }

        return service;
    }
})();
(function() {
    "use strict";

    angular.module("driveApp")
        .controller("HomeController", HomeController);

    HomeController.$inject = ['HomeService'];

    function HomeController(homeService) {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Binary.Drive";
            vm.message = homeService.getMessage();
        }
    }
}());