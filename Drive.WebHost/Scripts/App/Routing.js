var app = angular.module('driveApp', ['ngRoute']);

app.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        }).hashPrefix('!');

        $routeProvider
            .when('/', // Home Page
            {
                templateUrl: '/Scripts/App/Home/Home.html',
                controller: 'HomeController'
            })
            .when('/Home/Index', // Home Page
            { 
                templateUrl: '/Scripts/App/Home/Home.html',
                controller: 'HomeController'
            })
            .otherwise({ // This is when any route not matched - error
                controller: 'ErrorController'
        })
    }]);
