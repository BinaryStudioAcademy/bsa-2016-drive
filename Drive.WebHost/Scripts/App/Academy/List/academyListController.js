(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyListController', AcademyListController);

    AcademyListController.$inject = [
        'AcademyListService',
        '$location',
        '$uibModal'
    ];

    function AcademyListController(academyListService, $location, $uibModal) {
        var vm = this;
        vm.openCourse = openCourse;
        vm.changeView = changeView;
        vm.openNewCourseWindow = openNewCourseWindow;
        

        activate();

        function activate() {
            vm.academiesList = [];
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

        function openNewCourseWindow(size) {

            var courseModalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'Scripts/App/Academy/List/Create.html',
                windowTemplateUrl: 'Scripts/App/Academy/List/Modal.html',
                controller: 'CourseCreateController',
                controllerAs: 'courseCreateCtrl',
                size: size
            });

            courseModalInstance.result.then(function () {
                getAcademies();
            }, function () {
                getAcademies();
                console.log('Modal dismissed');
            });
        };
    }
}());