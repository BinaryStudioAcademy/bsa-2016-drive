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