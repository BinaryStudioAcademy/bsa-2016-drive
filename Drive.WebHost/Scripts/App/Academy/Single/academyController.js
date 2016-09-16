(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyController', AcademyController);

    AcademyController.$inject = ['AcademyService', '$location', '$routeParams', 'localStorageService'];

    function AcademyController(academyService, $location, $routeParams, localStorageService) {
        var vm = this;
        vm.currentAcademyId = $routeParams.id;
        vm.academy = null;
        vm.openLecture = openLecture;
        vm.changeView = changeView;
        vm.createLecture = createLecture;
        vm.getCourseList = getCourseList;
        vm.searchByTag = searchByTag;

        activate();

        function activate() {
            var view = localStorageService.get('view')
            if (view == undefined) {
                vm.showTable = true;
                vm.showGrid = false;
                vm.view = "fa fa-th";
            }
            else if (view.showTable) {
                vm.showTable = true;
                vm.showGrid = false;
                vm.view = "fa fa-th";
            }
            else {
                vm.showGrid = true;
                vm.showTable = false;
                vm.view = "fa fa-list";
            }
            vm.columnForOrder = 'name';
            vm.searchText = '';
            vm.iconHeight = 30;
            vm.icon = "./Content/Icons/lecture.svg";
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
                localStorageService.set('view', { showTable: false });
            } else {
                activateTableView();
                localStorageService.set('view', { showTable: true });
            }
        }
        function activateTableView() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
        }
        function activateGridView() {
            vm.view = "fa fa-list";
            vm.showTable = false;
            vm.showGrid = true;
        }
        function getCourseList() {
            $location.url('/apps/academy/');
        }
        function searchByTag(tag) {
            $location.url('/apps/academies/' + tag);
        }
    }
}());