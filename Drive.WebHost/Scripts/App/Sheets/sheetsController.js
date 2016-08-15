(function () {
    "use strict";

    angular.module("driveApp")
        .controller("SheetsController", SheetsController);

    function SheetsController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Sheets";
        }
    }
}());