angular.module('driveApp',
    ['ngRoute'])

 .config(['$routeProvider',
     function ($routeProvider) {

         $routeProvider
        .when('/', // Home Page
        {
            templateUrl: '/Scripts/App/Home/Home.html',
            controller: 'HomeController',
            controllerAs: 'homeCtrl'
        })
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
<<<<<<< HEAD
         .when('/Folders', // Space settings Page
         {
             templateUrl: '/Scripts/App/Folder/Folder.html',
             controller: 'FolderController',
             controllerAs: 'folderCtrl'
=======
         .when('/', // Space settings Page
         {
             templateUrl: '/Scripts/App/Folders/Create.html',
             controller: 'FoldersController',
             controllerAs: 'foldersCtrl'
>>>>>>> 5ad2191391030f04f1cee8bdd70ad72d37c892a3
         })
         .otherwise({ // This is when any route not matched - error
             controller: 'ErrorController'
         });
 }]);