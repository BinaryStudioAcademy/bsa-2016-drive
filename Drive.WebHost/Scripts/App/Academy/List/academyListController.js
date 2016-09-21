(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('AcademyListController', AcademyListController);

    AcademyListController.$inject = [
        'AcademyListService',
        '$location',
        '$uibModal',
        '$routeParams',
        'localStorageService'
    ];

    function AcademyListController(academyListService, $location, $uibModal, $routeParams, localStorageService) {
        var vm = this;
        vm.columnForOrder = 'name';
        vm.openCourse = openCourse;
        vm.changeView = changeView;
        vm.openNewCourseWindow = openNewCourseWindow;
        vm.deleteCourse = deleteCourse;
        vm.createNewCourse = createNewCourse;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.orderCourseByColumn = orderCourseByColumn;
        vm.openShareCourseContentWindow = openShareCourseContentWindow;
        vm.sharedCourseContent = sharedCourseContent;
        vm.getBinarySpaceIdent = getBinarySpaceIdent;

        vm.courseMenuOptions = [
        [
            'Share', function ($itemScope) {
                vm.contentSharedId = $itemScope.academy.fileUnit.id;
                sharedCourseContent();
            },
                function ($itemScope) {
                    if ($itemScope.academy.fileUnit.spaceId == vm.binarySpaceId) {
                        return false;
                    }
                    return true;
                }
        ],
        null,
        [
            'Edit', function ($itemScope) {
                vm.academy = $itemScope.academy;
                vm.openNewCourseWindow();
            },
                function ($itemScope) {
                    if ($itemScope.academy.fileUnit.canModify == false) {
                        return false;
                    }
                    return true;
                }
        ],
        null,
        [
            'Delete', function ($itemScope) {
                deleteCourse($itemScope.academy.id);
            },
                function ($itemScope) {
                    if ($itemScope.academy.fileUnit.canModify == false) {
                        return false;
                    }
                    return true;
                }
        ]
        ];
        
        activate();

        function activate() {
            vm.academiesList = [];
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
            vm.courseColumnForOrder = 'name';
            vm.searchText = $routeParams.tagName;
            vm.iconHeight = 30;
            vm.binarySpaceId = 0;
            vm.icon = "./Content/Icons/academyPro.svg";
            search();         
            
        }

        function getAcademies() {
            return academyListService.getAcademies()
                .then(function(data) {
                    vm.academiesList = data;
                    getBinarySpaceIdent(vm.academiesList);
                    return vm.academiesList;
                });
        }

        function openCourse(id) {
            $location.url('/apps/academy/' + id);
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
                getBinarySpaceIdent(vm.academiesList);
            });
        }

        function cancelSearch() {
            vm.searchText = "";
            search();
        }

        function orderCourseByColumn(column) {
            vm.courseColumnForOrder = academyListService.orderCoursesByColumn(column, vm.courseColumnForOrder);
        }

        function openShareCourseContentWindow(size) {

            var shareModalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'Scripts/App/SharedContent/SharedContentForm.html',
                windowTemplateUrl: 'Scripts/App/SharedContent/Modal.html',
                controller: 'SharedContentModalCtrl',
                controllerAs: 'sharedContentModalCtrl',
                size: size,
                resolve: {
                    items: function () {
                        var sharedContInfo = {
                            contentId: vm.contentSharedId,
                            title: 'Shared file'
                        }
                        return sharedContInfo;
                    }
                }
            });

            shareModalInstance.result.then(function (response) {
                console.log(response);
            }, function () {
                console.log('Modal dismissed');
            });
        }

        function sharedCourseContent() {
             openShareCourseContentWindow();
        }

        function getBinarySpaceIdent(list) {
            for (var i = 0; i <list.length; i++) {
                if (list[i].spaceType === 0) {
                    vm.binarySpaceId = list[i].spaceId;
                    return vm.binarySpaceId;
                }
            }
            return 0;
        }
    }
}());