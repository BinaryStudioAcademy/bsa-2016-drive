(function () {
    "use strict";

    angular.module("driveApp")
        .controller("adminPanelController", adminPanelController);

    function adminPanelController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Admin Panel";
        }
    }
}());