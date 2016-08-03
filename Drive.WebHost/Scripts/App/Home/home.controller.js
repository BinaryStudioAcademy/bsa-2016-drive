(function() {
    "use strict";

    angular.module("driveApp")
        .controller("homeController", homeController);

    function homeController() {
        var vm = this;

        vm.title = "Binary.Drive";
        vm.message = "Coming soon... Stay tuned...";
        
    }
}());