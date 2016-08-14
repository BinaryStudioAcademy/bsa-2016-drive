(function () {
    "use strict";

    angular.module("driveApp")
        .controller("SlidesController", SlidesController);

    function SlidesController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Slides";
        }
    }
}());