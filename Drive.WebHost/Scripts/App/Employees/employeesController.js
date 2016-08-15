(function () {
    "use strict";

    angular.module("driveApp")
        .controller("EmployeesController", EmployeesController);

    function EmployeesController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Employees";
        }
    }
}());