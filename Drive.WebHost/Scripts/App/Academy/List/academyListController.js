(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyListController', AcademyListController);

    AcademyListController.$inject = [
        'AcademyListService',
        '$location',
        '$uibModal',
        '$routeParams'
    ];

    function AcademyListController(academyListService, $location, $uibModal, $routeParams) {
        var vm = this;
        vm.openCourse = openCourse;
        vm.changeView = changeView;
        vm.openNewCourseWindow = openNewCourseWindow;
        vm.deleteCourse = deleteCourse;
        vm.createNewCourse = createNewCourse;
        vm.search = search;
        vm.cancelSearch = cancelSearch;

        vm.courseMenuOptions = [

        [
            'Edit', function ($itemScope) {
                vm.academy = $itemScope.academy;
                vm.openNewCourseWindow();
            }
        ],
        [
            'Delete', function ($itemScope) {
                deleteCourse($itemScope.academy.id);
            }
        ]
        ];
        
        activate();

        function activate() {
            vm.academiesList = [];
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.columnForOrder = 'name';
            vm.searchText = $routeParams.tagName;
            vm.iconHeight = 30;
            vm.icon = "./Content/Icons/academyPro.svg";
            search();         
            
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

        function openNewCourseWindow(size) {

            var courseModalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'Scripts/App/Academy/List/Create.html',
                windowTemplateUrl: 'Scripts/App/Academy/List/Modal.html',
                controller: 'CourseCreateController',
                controllerAs: 'courseCreateCtrl',
                size: size,
                resolve: {
                    items: function () {
                        return vm.academy;
                    }
                }
            });

            courseModalInstance.result.then(function () {
                search();
            }, function () {
                search();
                console.log('Modal dismissed');
            });
        };

        function createNewCourse()
        {
            vm.academy = {
                fileUnit: {}
            };
            vm.openNewCourseWindow('lg');
        }
 
        function deleteCourse(id) {
            academyListService.deleteData(id, function () {
                search();
            });
        }

        function search() {
            academyListService.searchCourses(vm.searchText, function (data) {
                vm.academiesList = data;
            });
        }
        function cancelSearch() {
            vm.searchText = "";
            search();
        }
    }
}());