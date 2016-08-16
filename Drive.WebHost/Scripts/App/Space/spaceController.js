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

        vm.folderList = [];
        vm.addElem = addElem;
        vm.deleteElems = deleteElems;
        vm.spaceId = 0;
        vm.parentId = 0;

        vm.changeView = changeView;
        vm.activateTableView = activateTableView;
        vm.activateGridView = activateGridView;

        // vm.getAllFolders = getAllFolders;
        vm.getFolder = getFolder;
        vm.deleteFolder = deleteFolder;
        vm.openFolderWindow = openFolderWindow;
        vm.getFolderContent = getFolderContent;

        vm.getFile = getFile;
        vm.deleteFile = deleteFile;
        vm.openFileWindow = openFileWindow;

        vm.findById = findById;

        vm.getSpace = getSpace;

        activate();

        function activate() {
            spaceService.getSpace(1, function (data) {
                vm.space = data;
                vm.spaceId = data.id;
            });
        }

        function getSpace() {
            spaceService.getSpace(1, function (data) {
                vm.space = data;
                vm.spaceId = data.id;
            });
            vm.folderList = [];
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
                'Create folder', function () {
                    vm.folder = { parentId: vm.parentId, spaceId: vm.spaceId };
                    vm.openFolderWindow();
                }
            ],
            [
                'Create file', function ($itemScope) {
                },
                [
                    [
                        'Document', function () {
                            vm.file = { type: 1, parentId: vm.parentId, spaceId: vm.spaceId };
                            vm.openFileWindow();
                        }
                    ],
                    [
                        'Sheets', function ($itemScope) {
                            vm.file = { type: 2, parentId: vm.parentId, spaceId: vm.spaceId };
                            vm.openFileWindow();
                        }
                    ],
                    [
                        'Slides', function ($itemScope) {
                            vm.file = { type: 3, parentId: vm.parentId, spaceId: vm.spaceId };
                            vm.openFileWindow();
                        }
                    ],
                    [
                        'Trello', function ($itemScope) {
                            vm.file = { type: 4, parentId: vm.parentId, spaceId: vm.spaceId };
                            vm.openFileWindow();
                        }
                    ],
                    [
                        'Link', function ($itemScope) {
                            vm.file = { type: 5, parentId: vm.parentId, spaceId: vm.spaceId };
                            vm.openFileWindow();
                        }
                    ],
                    [
                        'Upload file', function ($itemScope) {
                            vm.file = { type: 6, parentId: vm.parentId, spaceId: vm.spaceId };
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
                    items: function () {
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

        function getFolderContent(id) {
            vm.parentId = id;
            folderService.getContent(id, function (data) {
                vm.space.folders = data.folders;
                vm.space.files = data.files;
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

        function addElem(folder) {
            vm.folderList.push(folder);
        }

        function deleteElems(folder) {
            for (var i = vm.folderList.length - 1; i > -1; i--) {
                if (vm.folderList[i] === folder) {
                    break;
                }
                vm.folderList.splice(i, 1);
            }
        }
    }
}());