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
        vm.changeView = changeView;
        vm.createLecture = createLecture;

        activate();

        function activate() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.columnForOrder = 'name';
            vm.searchText = '';
            vm.iconHeight = 30;
            vm.showTable = true;
            vm.icon = "./Content/Icons/presentation.svg";
            vm.iconHeight = 30;

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

        function createLecture() {
            $location.url('/apps/academy/' + vm.currentAcademyId + '/newlecture');
        }

        function changeView(view) {
            if (view === "fa fa-th") {
                activateGridView();
            } else {
                activateTableView();
            }
        }
        function activateTableView() {
            vm.view = "fa fa-th";
            vm.showTable = true;
        }
        function activateGridView() {
            vm.view = "fa fa-list";
            vm.showTable = false;
        }
    }
}());