(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyListController', AcademyListController);

    AcademyListController.$inject = ['AcademyListService', '$location'];

    function AcademyListController(academyListService, $location) {
        var vm = this;
        vm.academiesList = [];
        vm.openCourse = openCourse;

        activate();

        function activate() {
            return getAcademies();
        }

        function getAcademies() {
            return academyListService.getAcademies()
                .then(function(data) {
                    vm.academiesList = data;
                    return vm.academiesList;
                });
        }

        function openCourse(id) {
            $location.url('/apps/academy/' + id);
        }
    }
}());