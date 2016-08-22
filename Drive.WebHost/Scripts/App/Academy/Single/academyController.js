(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyController', AcademyController);

    function AcademyController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Academy Pro";
        }
    }
}());