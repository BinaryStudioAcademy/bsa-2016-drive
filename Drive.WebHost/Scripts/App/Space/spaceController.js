(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SpaceController", SpaceController);

    SpaceController.$inject = ['SpaceService', 'FolderService', 'FileService', '$uibModal'];

    function SpaceController(spaceService, folderService, fileService, $uibModal) {
        var vm = this;

        vm.view = "fa fa-th";
        vm.showTable = true;
        vm.showGrid = false;

        vm.changeView = changeView;
        vm.activateTableView = activateTableView;
        vm.activateGridView = activateGridView;

        // vm.getAllFolders = getAllFolders;
        vm.getFolder = getFolder;
        vm.deleteFolder = deleteFolder;
        vm.openFolderWindow = openFolderWindow;

        vm.getFile = getFile;
        vm.deleteFile = deleteFile;
        vm.openFileWindow = openFileWindow;

        vm.findById = findById;

        activate();

        function activate() {
            spaceService.getSpace(3, function (data) {
                vm.space = data;
            });
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


        vm.folderMenuOptions = [
            [
                'Share', function ($itemScope) {
                    console.log($itemScope.folder.id);
                }
            ],
            [
                'Edit', function ($itemScope) {
                    vm.folder = $itemScope.folder;
                    vm.openFolderWindow();
                }
            ],
            [
                'Delete', function ($itemScope) {
                    return deleteFolder($itemScope.folder.id);
                }
            ]
        ];

        vm.fileMenuOptions = [
    [
        'Share', function ($itemScope) {
            console.log($itemScope.file.id);
        }
    ],
    [
        'Edit', function ($itemScope) {
            vm.file = $itemScope.file;
            vm.openFileWindow();
        }
    ],
    [
        'Delete', function ($itemScope) {
            return deleteFile($itemScope.file.id);
        }
    ]
        ];

        vm.createOption = [
            [
                'Create', function ($itemScope) {
                },
                [
                    [
                        'Folder', function () {
                            vm.folder = { spaceid : vm.space.id};
                            vm.openFolderWindow();
                        }
                    ],
                    [
                        'File', function ($itemScope) {
                            vm.file = { spaceid: vm.space.id };
                            vm.openFileWindow();
                        }
                    ]
                ]
            ]
        ];

        function openFolderWindow(size) {

            var folderModalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'Scripts/App/Folder/CreateUpdateFolderForm.html',
                windowTemplateUrl: 'Scripts/App/Folder/Modal.html',
                controller: 'FolderModalCtrl',
                controllerAs: 'folderModalCtrl',
                size: size,
                resolve: {
                    items: function () {
                        return vm.folder;
                    }
                }
            });

            folderModalInstance.result.then(function (folder) {
                console.log(folder);
                var index = findById(vm.space.folders, folder.id);
                if (index == -1) {
                    vm.space.folders.push(folder);
                } else {
                    vm.space.folders[index] = folder;
                }
            }, function () {
                console.log('Modal dismissed');
            });
        };

        function openFileWindow(size) {

            var fileModalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'Scripts/App/File/FileForm.html',
                windowTemplateUrl: 'Scripts/App/File/Modal.html',
                controller: 'FileModalCtrl',
                controllerAs: 'fileModalCtrl',
                size: size,
                resolve: {
                    items: function() {
                        return vm.file;
                    }
                }
            });

            fileModalInstance.result.then(function (file) {
                console.log(file);
                var index = findById(vm.space.files, file.id);
                if (index == -1) {
                    vm.space.files.push(file);
                } else {
                    vm.space.files[index] = file;
                }
            }, function () {
                console.log('Modal dismissed');
            });
        }

        function getFolder(id) {
            folderService.get(id, function (folder) {
                vm.folder = folder;
            });
        }

        //function getAllFolders() {
        //    folderService.getAll(function (folders) {
        //        vm.folders = folders;
        //    });
        //}

        function findById(data, id) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].id === id) {
                    return i;
                }
            }
            return -1;
        }

        function deleteFolder(id) {
            folderService.deleteFolder(id, function () {
                var index = findById(vm.space.folders, id);
                vm.space.folders.splice(index, 1);
            });
        }

        function getFile(id) {
            fileService.getFile(id, function (file) {
                vm.file = file;
            });
        }

        function deleteFile(id) {
            fileService.deleteFile(id, function () {
                var index = findById(vm.space.files, id);
                vm.space.files.splice(index, 1);
            });
        }
    }
 }());