(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyListController', AcademyListController);

    function AcademyListController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Academy Pro";
        }
    }
}());