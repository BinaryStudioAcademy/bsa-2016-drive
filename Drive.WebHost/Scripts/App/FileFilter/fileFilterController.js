(function () {
    "use strict";

    angular.module("driveApp")
        .controller("FileFilterController", FileFilterController);

    FileFilterController.$inject = ['FileService', '$uibModal', '$routeParams'];

    function FileFilterController(fileService, $uibModal, $routeParams) {
        var vm = this;

        vm.changeView = changeView;
        vm.chooseIcon = chooseIcon;
        vm.orderByColumn = orderByColumn;

        vm.search = search;
        //vm.cancelSearch = cancelSearch;

        vm.openDocument = openDocument;
        vm.openFileWindow = openFileWindow;
        vm.deleteFile = deleteFile;

        activate();

        function activate() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.columnForOrder = 'name';
            vm.searchText = '';
            vm.iconHeight = 30;

            vm.spaces = [];

            setFileData();
            getFiles();
        }
        function getFiles() {
            fileService.getFilesApp(vm.filesType, function (data) {
                vm.spaces = data;
            });
        }

        vm.fileMenuOptions = [
            [
                'Share //Add', function ($itemScope) {

                }
            ],
            null,
            [
                'Edit', function ($itemScope) {
                    vm.file = $itemScope.file;
                    vm.file.parentId = $itemScope.file.parentId;
                    vm.file.spaceId = $itemScope.spaceId;
                    vm.openFileWindow();
                }
            ],
            null,
            [
                'Delete', function ($itemScope) {
                    return deleteFile($itemScope.file.id);
                }
            ]
        ];

        function openFileWindow(size) {

            var fileModalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'Scripts/App/File/FileForm.html',
                windowTemplateUrl: 'Scripts/App/File/Modal.html',
                controller: 'FileModalCtrl',
                controllerAs: 'fileModalCtrl',
                size: size,
                resolve: {
                    items: function () {
                        return vm.file;
                    }
                }
            });

            fileModalInstance.result.then(function (response) {
                console.log(response);
                getFiles();
            }, function () {
                console.log('Modal dismissed');
            });
        }

        function deleteFile(id) {
            fileService.deleteFile(id, function () {
                getFiles();
            });
        }

        function search() {
            fileService.searchFiles(vm.filesType, vm.searchText, function (data) {
                vm.spaces = data;
            });
            vm.searchText = '';
        }

        /*
        function cancelSearch() {         
            getFiles();
            vm.searchText = '';
        }
        */

        function setFileData() {
            switch ($routeParams.appName) {
                // not defined types => update Enum
                case 'academy':
                    vm.filesType = 'AcademyPro';
                    vm.icon = 'fa fa-graduation-cap fa-lg';
                    break;
                case 'events':
                    vm.filesType = 'Events';
                    vm.icon = 'fa fa-calendar fa-lg';
                    break;
                case 'employees':
                    vm.filesType = 'Employees';
                    vm.icon = 'fa fa-users fa-lg';
                    break;
                case 'checklist':
                    vm.filesType = 'Checklist';
                    vm.icon = 'fa fa-check-square-o fa-lg';
                    break;
                //
                case 'trello':
                    vm.filesType = 'Trello';
                    vm.icon = 'fa fa-trello fa-lg';
                    break;
                case 'docs':
                    vm.filesType = 'Document';
                    vm.icon = 'fa fa-file-text fa-lg';
                    break;
                case 'sheets':
                    vm.filesType = 'Sheets';
                    vm.icon = 'fa fa-table fa-lg';
                    break;
                case 'slides':
                    vm.filesType = 'Slides';
                    vm.icon = 'fa fa-file-powerpoint-o fa-lg';
                    break;
            }
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
            vm.showGrid = false;
        }
        function activateGridView() {
            vm.view = "fa fa-list";
            vm.showTable = false;
            vm.showGrid = true;
        }

        function orderByColumn(column) {
            vm.columnForOrder = fileService.orderByColumn(column, vm.columnForOrder);
        }

        function openDocument(url) {
            fileService.openFile(url);
        }

        function chooseIcon(type) {
            vm.iconSrc = fileService.chooseIcon(type);
            return vm.iconSrc;
        }
    }
}());