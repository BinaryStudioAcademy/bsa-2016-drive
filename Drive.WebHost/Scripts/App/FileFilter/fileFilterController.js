(function () {
    "use strict";

    angular.module("driveApp")
        .controller("FileFilterController", FileFilterController);

    FileFilterController.$inject = ['FileService', '$uibModal', '$routeParams', 'Lightbox'];

    function FileFilterController(fileService, $uibModal, $routeParams, Lightbox) {
        var vm = this;

        vm.changeView = changeView;
        vm.chooseIcon = chooseIcon;
        vm.orderByColumn = orderByColumn;

        vm.search = search;
        vm.cancelSearch = cancelSearch;

        vm.openDocument = openDocument;
        vm.openFileWindow = openFileWindow;
        vm.deleteFile = deleteFile;
        vm.openSharedContentWindow = openSharedContentWindow;
        vm.sharedContent = sharedContent;
        vm.getBinarySpaceId = getBinarySpaceId;
        vm.openLightboxModal = openLightboxModal;

        activate();

        function activate() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.columnForOrder = 'name';
            vm.searchText = '';
            vm.iconHeight = 30;
            vm.binarySpaceId = 0;

            vm.spaces = [];

            vm.images = [];

            setFileData();
            getFiles();
        }

        function getFiles() {
            fileService.getFilesApp(vm.filesType, function (data) {
                vm.spaces = data;

                for (var i = 0; i < vm.spaces.length; i++) {
                    for (var k = 0; k < vm.spaces[i].files.length; k++) {
                        var file = vm.spaces[i].files[k];
                        vm.images.push({
                            url: file.link,
                            caption: file.name,
                            thumbUrl: file.link,
                            fileType: file.fileType,
                            created: file.createdAt
                        });
                    }
                }

                getBinarySpaceId(vm.spaces);
            });
        }

        vm.fileMenuOptions = [
            [
                'Share', function ($itemScope) {
                vm.contentSharedId = $itemScope.file.id;
                    vm.sharedContent();
                },
                function ($itemScope) {
                    if ($itemScope.file.spaceId == vm.binarySpaceId) {
                        return false;
                    }
                    return true;
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
                animation: true,
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

        function openSharedContentWindow(size) {

            var fileModalInstance = $uibModal.open({
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

            fileModalInstance.result.then(function (response) {
                console.log(response);
            }, function () {
                console.log('Modal dismissed');
            });
        }

        function sharedContent() {
            vm.openSharedContentWindow();
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
        }


        function cancelSearch() {         
            if (vm.searchText.length >= 1) {
                vm.searchText = '';
                getFiles();
            }
        }

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
                case 'links':
                    vm.filesType = 'Links';
                    vm.icon = 'fa fa-link fa-lg';
                    break;
                case 'uploaded':
                    vm.filesType = 'Uploaded';
                    vm.icon = 'fa fa-cloud-upload fa-lg';
                    break;
                case 'images':
                    vm.filesType = 'Images';
                    activateGridView();
                    vm.icon = 'fa fa-file-image-o fa-lg';
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

        function openDocument(file) {
            if (file.fileType === 6 || file.fileType === 8) {
                fileService.downloadFile(file.link);
            }
            else {
                fileService.openFile(file.link);
            }
        }

        function openLightboxModal(index) {
            Lightbox.openModal(vm.images, index);
        };

        function chooseIcon(type) {
            vm.iconSrc = fileService.chooseIcon(type);
            return vm.iconSrc;
        }

        function getBinarySpaceId(list) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].spaceType === 0) {
                    vm.binarySpaceId = list[i].spaceId;
                    return vm.binarySpaceId;
                }
            }
            return 0;
        }

    }
}());