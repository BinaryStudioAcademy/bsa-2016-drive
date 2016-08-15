(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SpaceController", SpaceController);

    SpaceController.$inject = ['SpaceService', 'FolderService', '$uibModal'];

    function SpaceController(spaceService, folderService, $uibModal) {
        var vm = this;

        vm.view = "fa fa-th";
        vm.showTable = true;
        vm.showGrid = false;

        vm.changeView = changeView;

        vm.folder = {
            id: 0,
            isDeleted: false,
            name: '',
            description: '',
            createdAt: '',
            lastModified: ''
        };


        activate();

        function activate() {
            spaceService.getSpace(1, function (data) {
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

//        vm.getAllFolders = getAllFolders;
        vm.getFolder = getFolder;
        vm.deleteFolder = deleteFolder;
        vm.openFolderWindow = openFolderWindow;


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

        vm.createOption = [
            [
                'Create', function ($itemScope) {
                },
                [
                    [
                        'Folder', function () {
                            vm.folder = {}
                            vm.openFolderWindow();
                        }
                    ],
                    [
                        'File', function ($itemScope) {

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
                controller: 'ModalInstanceCtrl',
                controllerAs: 'modalCtrl',
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
    }
 }());