var appMain = angular.module('driveApp', ['ngRoute']);
app.config([
  '$locationProvider', '$routeProvider',
  function ($locationProvider, $routeProvider) {
      $locationProvider.html5Mode({
          enabled: true,
          requireBase: false
      }).hashPrefix('!');
      $routeProvider
      .when('/Home', { //Home Page  
          templateUrl: '/Home/Home.html',
          controller: 'HomeController'
      })
  }]);