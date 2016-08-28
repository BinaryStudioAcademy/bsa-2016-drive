(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyListController', AcademyListController);

    AcademyListController.$inject = ['AcademyListService', '$location'];

    function AcademyListController(academyListService, $location) {
        var vm = this;
        vm.academiesList = [];
        vm.openCourse = openCourse;
        vm.changeView = changeView;
        

        activate();

        function activate() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.columnForOrder = 'name';
            vm.searchText = '';
            vm.iconHeight = 30;
            vm.showTable = true;
            vm.icon = "./Content/Icons/folder.svg";
            vm.iconHeight = 30;

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

        function changeView(view) {
            if (view == "fa fa-th") {
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