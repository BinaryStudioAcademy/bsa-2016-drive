var appMain = angular.module('driveApp');
app.controller('HomeController', ['$scope', '$routeParams', function ($scope) {
    $scope.Message = "Comming Soon...";
    $scope.title = "Main";
}]);