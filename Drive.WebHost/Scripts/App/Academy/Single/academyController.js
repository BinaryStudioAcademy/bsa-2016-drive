(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyController', AcademyController);

    AcademyController.$inject = ['AcademyService', '$location', '$routeParams'];

    function AcademyController(academyService, $location, $routeParams) {
        var vm = this;
        vm.currentAcademyId = $routeParams.id;
        vm.academy = null;
        vm.openLecture = openLecture;

        activate();

        function activate() {
            return getAcademy();
        }

        function getAcademy() {
            return academyService.getAcademy(vm.currentAcademyId)
                .then(function (data) {
                    vm.academy = data;
                    return vm.academy;
                });
        }

        function openLecture(id) {
            $location.url('/apps/academy/' + vm.currentAcademyId + '/lecture/' + id);
        }
    }
}());