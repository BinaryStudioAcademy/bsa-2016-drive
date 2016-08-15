(function () {
    "use strict";

    angular.module("driveApp")
        .controller("ChecklistController", ChecklistController);

    function ChecklistController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Checklist";
        }
    }
}());