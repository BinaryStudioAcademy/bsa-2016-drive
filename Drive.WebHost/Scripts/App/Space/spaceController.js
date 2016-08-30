(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SpaceController", SpaceController);

    SpaceController.$inject = ['SpaceService', 'FolderService', 'FileService', '$uibModal', 'localStorageService', '$routeParams', '$location'];

    function SpaceController(spaceService,
        folderService,
        fileService,
        $uibModal,
        localStorageService,
        $routeParams,
        $location) {
        var vm = this;

        vm.folderList = [];
        vm.addElem = addElem;
        vm.deleteElems = deleteElems;
        vm.spaceId = 0;
        vm.parentId = null;
        vm.selectedSpace = currentSpaceId();

        vm.space = {
            folders: [],
            files: []
        }

        // vm.getAllFolders = getAllFolders;
        vm.getFolder = getFolder;
        vm.deleteFolder = deleteFolder;
        vm.openFolderWindow = openFolderWindow;
        vm.getFolderContent = getFolderContent;

        vm.getFile = getFile;
        vm.deleteFile = deleteFile;
        vm.openFileWindow = openFileWindow;
        vm.openDocument = openDocument;
        vm.openSharedFileWindow = openSharedFileWindow;
        vm.sharedFile = sharedFile;

        vm.findById = findById;
        vm.getSpace = getSpace;
        vm.getSpaceByButton = getSpaceByButton;

        vm.createNewFolder = createNewFolder;
        vm.createNewFile = createNewFile;

        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.searchText = '';

        vm.orderByColumn = orderByColumn;
        vm.chooseIcon = chooseIcon;

        vm.redirectToSpaceSettings = redirectToSpaceSettings;

        vm.paginate = {
            currentPage: 1,
            pageSize: 15,
            numberOfItems: 0,
            getContent: null
        }

        vm.pageChanged = function (pageNumber) {
            vm.paginate.currentPage = pageNumber;
            vm.paginate.getContent();
        }


        activate();

        function activate() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.changeView = changeView;
            vm.columnForOrder = 'name';
            vm.iconHeight = 30;

            spaceService.getSpace(vm.selectedSpace,
                vm.paginate.currentPage,
                vm.paginate.pageSize,
                vm.sortByDate,
                function (data) {
                vm.space = data;
                vm.spaceId = data.id;

                if (localStorageService.get('spaceId') !== vm.spaceId) {
                    localStorageService.set('spaceId', vm.spaceId);
                    localStorageService.set('current', null);
                        vm.parentId = null;
                    localStorageService.set('list', null);
                }

                if (localStorageService.get('list') != null)
                    vm.folderList = localStorageService.get('list');

                if (localStorageService.get('current') != null) {
                    vm.parentId = localStorageService.get('current');
                    getFolderContent(vm.parentId);
                } else {
                    getSpace();
                }

            });
        }

        function currentSpaceId() {
            if ($routeParams.id) {
                return $routeParams.id;
            }
            return 1;
        }

        function getSpace() {
            vm.searchText = '';
            vm.parentId = null;
            vm.paginate.currentPage = 1;
            getSpaceContent();
            getSpaceTotal();
            vm.paginate.getContent = getSpaceContent;
        }

        function getSpaceContent() {
            spaceService.getSpace(vm.selectedSpace,
                vm.paginate.currentPage,
                vm.paginate.pageSize,
                vm.sortByDate,
                function (data) {
                vm.space = data;
                vm.spaceId = data.id;
            });
        }

        function getSpaceByButton() {
            getSpace();
            localStorageService.set('list', []);
            vm.folderList = localStorageService.get('list');
            vm.parentId = null;
            localStorageService.set('current', null);
        }

        function getSpaceTotal() {
            spaceService.getSpaceTotal(vm.selectedSpace,
                function (data) {
                vm.paginate.numberOfItems = data;
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
            null,
            ['New', function () {
               
            },  [
            ['New folder', function () { vm.createNewFolder(); }],
            ['New link', function () { vm.createNewFile(); }],
            null,
            ['Upload file', function () { }]
            ]
            ],
            null,
            [
                'Edit', function ($itemScope) {
                    vm.folder = $itemScope.folder;
                    vm.folder.parentId = vm.parentId;
                    vm.folder.spaceId = vm.spaceId;
                    vm.openFolderWindow();
                }
            ],
             [
                'Copy', function ($itemScope) {
                    localStorageService.set('copy', { id: $itemScope.folder.id, file: false });
                }
             ],
            [
                'Cut', function ($itemScope) {
                    localStorageService.set('cut-out', { id: $itemScope.folder.id, file: false });
                    localStorageService.set('oldParentId', vm.parentId);
                    deleteFolder($itemScope.folder.id);
                }
            ],
            [
            'Paste', function () {
                if (localStorageService.get('cut-out') != null) {
                    if (localStorageService.get('cut-out').file) {
                        fileService.getDeletedFile(localStorageService.get('cut-out').id, function (data) {
                            var file = data;
                            file.isDeleted = false;
                            file.spaceId = vm.spaceId;
                            file.parentId = vm.parentId;

                            fileService.updateDeletedFile(file.id, localStorageService.get('oldParentId'), file, function () {
                                if (vm.parentId == null) {
                                    vm.getSpace();
                                } else {
                                    vm.getFolderContent(vm.parentId);
                                }
                            });
                        });
                    } else {

                        folderService.getDeleted(localStorageService.get('cut-out').id, function (data) {
                            var folder = data;
                            folder.isDeleted = false;
                            folder.spaceId = vm.spaceId;
                            folder.parentId = vm.parentId;

                            folderService.updateDeleted(folder.id, localStorageService.get('oldParentId'), folder, function () {
                                if (vm.parentId == null) {
                                    vm.getSpace();
                                } else {
                                    vm.getFolderContent(vm.parentId);
                                }
                            });
                        });
                    }
                    localStorageService.set('cut-out', null);
                }
                if (localStorageService.get('copy') != null) {
                    if (localStorageService.get('copy').file) {
                        var file = {};
                        file.spaceId = vm.spaceId;
                        file.parentId = vm.parentId;

                        fileService.createCopyFile(localStorageService.get('copy').id, file, function () {
                            if (vm.parentId == null) {
                                vm.getSpace();
                            } else {
                                vm.getFolderContent(vm.parentId);
                            }
                        });
                    } else {
                        var folder = {};
                        folder.spaceId = vm.spaceId;
                        folder.parentId = vm.parentId;

                        folderService.createCopy(localStorageService.get('copy').id, folder, function () {
                            if (vm.parentId == null) {
                                vm.getSpace();
                            } else {
                                vm.getFolderContent(vm.parentId);
                            }
                        });
                    }
                    localStorageService.set('copy', null);
                }
            }
            ],
            null,
            [
                'Delete', function ($itemScope) {
                    return deleteFolder($itemScope.folder.id);
                }
            ]
        ];

        vm.fileMenuOptions = [
            [
                'Share', function ($itemScope) {
                    vm.fileSharedId = $itemScope.file.id;
                    console.log(vm.fileSharedId);
                    vm.sharedFile();
                }
            ],
            null,
            ['New', function () {

            }, [
            ['New folder', function () { vm.createNewFolder(); }],
            ['New link', function () { vm.createNewFile(); }],
            null,
            ['Upload file', function () { }]
            ]
            ],
            [
                'Edit', function ($itemScope) {
                    vm.file = $itemScope.file;
                    vm.file.parentId = vm.parentId;
                    vm.file.spaceId = vm.spaceId;
                    vm.openFileWindow();
                }
            ],
            [
                'Copy', function ($itemScope) {
                    localStorageService.set('copy', { id: $itemScope.file.id, file: true });
                }
            ],
            [
                'Cut', function ($itemScope) {
                    localStorageService.set('cut-out', { id: $itemScope.file.id, file: true });
                    localStorageService.set('oldParentId', vm.parentId);
                    deleteFile($itemScope.file.id);
                }
            ],
            [
            'Paste', function () {
                if (localStorageService.get('cut-out') != null) {
                    if (localStorageService.get('cut-out').file) {
                        fileService.getDeletedFile(localStorageService.get('cut-out').id, function (data) {
                            var file = data;
                            file.isDeleted = false;
                            file.spaceId = vm.spaceId;
                            file.parentId = vm.parentId;

                            fileService.updateDeletedFile(file.id, localStorageService.get('oldParentId'), file, function () {
                                if (vm.parentId == null) {
                                    vm.getSpace();
                                } else {
                                    vm.getFolderContent(vm.parentId);
                                }
                            });
                        });
                    } else {

                        folderService.getDeleted(localStorageService.get('cut-out').id, function (data) {
                            var folder = data;
                            folder.isDeleted = false;
                            folder.spaceId = vm.spaceId;
                            folder.parentId = vm.parentId;

                            folderService.updateDeleted(folder.id, localStorageService.get('oldParentId'), folder, function () {
                                if (vm.parentId == null) {
                                    vm.getSpace();
                                } else {
                                    vm.getFolderContent(vm.parentId);
                                }
                            });
                        });
                    }
                    localStorageService.set('cut-out', null);
                }
                if (localStorageService.get('copy') != null) {
                    if (localStorageService.get('copy').file) {
                        var file = {};
                        file.spaceId = vm.spaceId;
                        file.parentId = vm.parentId;

                        fileService.createCopyFile(localStorageService.get('copy').id, file, function () {
                            if (vm.parentId == null) {
                                vm.getSpace();
                            } else {
                                vm.getFolderContent(vm.parentId);
                            }
                        });
                    } else {
                        var folder = {};
                        folder.spaceId = vm.spaceId;
                        folder.parentId = vm.parentId;

                        folderService.createCopy(localStorageService.get('copy').id, folder, function () {
                            if (vm.parentId == null) {
                                vm.getSpace();
                            } else {
                                vm.getFolderContent(vm.parentId);
                            }
                        });
                    }
                    localStorageService.set('copy', null);
                }
            }
            ],
            null,
            [
                'Delete', function ($itemScope) {
                    return deleteFile($itemScope.file.id);
                }
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

            folderModalInstance.result.then(function (response) {
                console.log(response);
                if (response.operation == 'create') {
                    if (vm.parentId == null) {
                        vm.getSpace();
                    }
                    else {
                        vm.getFolderContent(vm.parentId);
                    }
                }
                if (response.operation == 'update') {
                    var index = findById(vm.space.folders, response.item.id);
                    if (index != -1) {
                        vm.space.folders[index] = response.item;
                }
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

            fileModalInstance.result.then(function (response) {
                console.log(response);
                if (response.operation == 'create') {
                    if (vm.parentId == null) {
                        vm.getSpace();
                    }
                    else {
                        vm.getFolderContent(vm.parentId);
                    }
                }
                if (response.operation == 'update') {
                    var index = findById(vm.space.files, response.item.id);
                    if (index != -1) {
                        vm.space.files[index] = response.item;
                    }
                }
            }, function () {
                console.log('Modal dismissed');
            });
        }

        function openSharedFileWindow(size) {

            var fileModalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'Scripts/App/SharedFile/SharedFileForm.html',
                windowTemplateUrl: 'Scripts/App/SharedFile/Modal.html',
                controller: 'SharedFileModalCtrl',
                controllerAs: 'sharedFileModalCtrl',
                size: size,
                resolve: {
                    items: function () {
                        return vm.fileSharedId;
                    }
                }
            });

            fileModalInstance.result.then(function (response) {
                console.log(response);
            }, function () {
                console.log('Modal dismissed');
            });
        }

        function sharedFile() {
            vm.fileId = { parentId: vm.parentId, spaceId: vm.spaceId };
            vm.openSharedFileWindow();
        }

        function createNewFolder() {
            vm.folder = { parentId: vm.parentId, spaceId: vm.spaceId };
            vm.openFolderWindow();
        }

        function createNewFile() {
            vm.file = { parentId: vm.parentId, spaceId: vm.spaceId };
            vm.openFileWindow();
        }

        function redirectToSpaceSettings(id) {
            $location.url("/spaces/" + id + "/settings/");
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
                vm.paginate.getContent();
                if (vm.parentId == null) {
                    getSpaceTotal();
                }
                else {
                    getFolderContentTotal(vm.parentId);
                }
            });
        }

        function getFolderContent(id) {
            vm.paginate.getContent = getFolderContentFromApi;
            vm.searchText = '';
            vm.paginate.currentPage = 1;
            vm.parentId = id;
            getFolderContentFromApi();
            getFolderContentTotal(id);
            localStorageService.set('current', id);
        }

        function getFolderContentFromApi() {
            vm.searchText = '';
            folderService.getContent(vm.parentId, vm.paginate.currentPage, vm.paginate.pageSize, vm.sortByDate, function (data) {
                vm.space.folders = data.folders;
                vm.space.files = data.files;
            });
        }

        function getFolderContentTotal(id) {
            folderService.getFolderContentTotal(id, function (data) {
                vm.paginate.numberOfItems = data;
            });
        }

        function getFile(id) {
            fileService.getFile(id, function (file) {
                vm.file = file;
            });
        }

        function deleteFile(id) {
            fileService.deleteFile(id, function () {
                vm.paginate.getContent();
                if (vm.parentId == null) {
                    getSpaceTotal();
                }
                else {
                    getFolderContentTotal(vm.parentId);
                }
            });
            localStorageService.set('files', vm.space.files);
        }

        function addElem(folder) {
            vm.folderList.push(folder);
            localStorageService.set('list', vm.folderList);
        }

        function deleteElems(folder) {
            for (var i = vm.folderList.length - 1; i > -1; i--) {
                if (vm.folderList[i] === folder) {
                    break;
                }
                vm.folderList.splice(i, 1);
            }

            localStorageService.set('list', vm.folderList);
        }

        function search() {
            vm.paginate.currentPage = 1;
            vm.paginate.getContent = getResultSearchFoldersAndFiles;
            getResultSearchFoldersAndFiles();
            getNumberOfResultSearch();
        }

        function cancelSearch() {
            if (vm.searchText.length >= 1) {
                vm.searchText = '';
                vm.paginate.currentPage = 1;
                vm.paginate.getContent = getResultSearchFoldersAndFiles;
                getResultSearchFoldersAndFiles();
                getNumberOfResultSearch();
            }
        }

        function getResultSearchFoldersAndFiles() {
            spaceService.searchFoldersAndFiles(vm.spaceId, vm.parentId, vm.searchText, vm.paginate.currentPage, vm.paginate.pageSize, function (data) {
                vm.space.folders = data.folders;
                vm.space.files = data.files;
            });
        }

        function getNumberOfResultSearch() {
            spaceService.getNumberOfResultSearchFoldersAndFiles(vm.spaceId, vm.parentId, vm.searchText, function (data) {
                vm.paginate.numberOfItems = data;
            });
        }

        function openDocument(url) {
            fileService.openFile(url);
        }

        function orderByColumn(column) {
            vm.columnForOrder = fileService.orderByColumn(column, vm.columnForOrder);
        }

        function chooseIcon(type) {
            vm.iconSrc = fileService.chooseIcon(type);
            return vm.iconSrc;
        }
    }
}());