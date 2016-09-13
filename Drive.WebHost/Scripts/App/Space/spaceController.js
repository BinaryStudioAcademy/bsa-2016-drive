(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SpaceController", SpaceController);

    SpaceController.$inject = ['SpaceService', 'FolderService', 'FileService', '$uibModal', 'localStorageService', '$routeParams', '$location', 'toastr', '$scope', 'hotkeys'];

    function SpaceController(spaceService,
        folderService,
        fileService,
        $uibModal,
        localStorageService,
        $routeParams,
        $location,
        toastr,
        $scope,
        hotkeys) {
        var vm = this;

        vm.folderList = [];
        vm.addElem = addElem;
        vm.deleteElems = deleteElems;
        vm.spaceId = 0;
        vm.parentId = null;

        // vm.getAllFolders = getAllFolders;
        vm.getFolder = getFolder;
        vm.deleteFolder = deleteFolder;
        vm.openFolderWindow = openFolderWindow;
        vm.getFolderContent = getFolderContent;

        vm.getFile = getFile;
        vm.deleteFile = deleteFile;
        vm.openFileWindow = openFileWindow;
        vm.openFileUploadWindow = openFileUploadWindow;
        vm.openDocument = openDocument;
        vm.openNewCourseWindow = openNewCourseWindow;
        vm.createNewAP = createNewAP;
        vm.openSharedContentWindow = openSharedContentWindow;

        vm.sharedContent = sharedContent;

        vm.findById = findById;
        vm.getSpace = getSpace;
        vm.getSpaceByButton = getSpaceByButton;

        vm.createNewFolder = createNewFolder;
        vm.createNewFile = createNewFile;
        vm.uploadFile = uploadFile;

        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.searchText = '';

        vm.orderByColumn = orderByColumn;
        vm.chooseIcon = chooseIcon;

        vm.redirectToSpaceSettings = redirectToSpaceSettings;

        // Drag and Drop
        vm.onDrop = onDrop;
        vm.dropValidate = dropValidate;
        vm.dndMoveContent = dndMoveContent;
        vm.dndCopyContent = dndCopyContent;
        vm.getDragImageId = getDragImageId;

        // Multiselect
        vm.selectItems = selectItems;
        vm.selectItemsForDrag = selectItemsForDrag;
        vm.rightClickSelection = rightClickSelection;
        vm.initSelection = initSelection;
        vm.getSelectedItems = getSelectedItems;

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
        vm.folderMenuOptionShareShow = true;
        vm.fileMenuOptionShareShow = true;
        vm.sharedModalWindowTitle = null;

        vm.activeRow = activeRow;
        vm.copyByHotkeys = copyByHotkeys;
        vm.pasteByHotkeys = pasteByHotkeys;
        vm.cutByHotkeys = cutByHotkeys;
        vm.deleteByHotkeys = deleteByHotkeys;
        vm.undoByHotkeys = undoByHotkeys;
        vm.lastActionType = undefined;
        vm.lastItemId = undefined;


        activate();

        hotkeys.bindTo($scope)
            .add({
                combo: 'ctrl+x',
                callback: function () {
                    cutByHotkeys();
                }
            })
            .add({
                combo: 'ctrl+c',
                callback: function () {
                    copyByHotkeys();
                }
            })
            .add({
                combo: 'ctrl+v',
                callback: function () {
                    pasteByHotkeys();
                }
            })
            .add({
                combo: 'del',
                callback: function () {
                    deleteByHotkeys();
                }
            })
            .add({
                combo: 'ctrl+z',
                callback: function () {
                    undoByHotkeys();
                }
            })

        function undoByHotkeys() {
            if (vm.lastActionType == 'deleteFile') {
                fileService.getDeletedFile(vm.lastItemId,
        function (data) {
            var file = data;
            file.isDeleted = false;
            file.spaceId = vm.spaceId;
            file.parentId = vm.parentId;

            fileService.updateDeletedFile(file.id,
                localStorageService.get('oldParentId'),
                file,
                function () {
                    if (vm.parentId == null) {
                        vm.getSpace();
                    } else {
                        vm.getFolderContent(vm.parentId);
                    }
                });
        });
            }
            else if (vm.lastActionType == 'deleteFolder') {
                folderService.getDeleted(vm.lastItemId,
    function (data) {
        var folder = data;
        folder.isDeleted = false;
        folder.spaceId = vm.spaceId;
        folder.parentId = vm.parentId;

        folderService.updateDeleted(folder.id,
            localStorageService.get('oldParentId'),
            folder,
            function () {
                if (vm.parentId == null) {
                    vm.getSpace();
                } else {
                    vm.getFolderContent(vm.parentId);
                }
            });
    });
            }
            vm.lastActionType = undefined;
            vm.lastItemId = undefined;
        }

        function deleteByHotkeys() {
            if (vm.row != undefined) {
                if (!vm.condition) {
                    vm.lastActionType = 'deleteFolder';
                    vm.lastItemId = vm.row;
                    return deleteFolder(vm.row);
                } else {
                    vm.lastActionType = 'deleteFile';
                    vm.lastItemId = vm.row;
                    return deleteFile(vm.row);
                }
            }
        }

        function activeRow(id, data) {
            for (var i = 0; i < vm.space.folders.length; i++) {
                vm.space.folders[i].selected = false;
            }
            for (var i = 0; i < vm.space.files.length; i++) {
                vm.space.files[i].selected = false;
            }
            if (id != undefined) {
                vm.row = id;
            }
            if (data == 'true') {
                vm.condition = true;
                var pos = vm.space.files.map(function (e) { return e.id; }).indexOf(id);
                vm.space.files[pos].selected = true;
            } else {
                vm.condition = false;
                var pos = vm.space.folders.map(function (e) { return e.id; }).indexOf(id);
                vm.space.folders[pos].selected = true;
            }
        }

        function copyByHotkeys() {
            for (var i = 0; i < vm.space.folders.length; i++) {
                vm.space.folders[i].cutted = false;
            }
            for (var i = 0; i < vm.space.files.length; i++) {
                vm.space.files[i].cutted = false;
            }
            if (vm.row != undefined) {
                localStorageService.clearAll();
                localStorageService.set('copy', { id: vm.row, file: vm.condition });
                toastr.info(
                    'Item has been copied to the clipboard.',
                    'Space',
                    {
                        closeButton: true,
                        timeOut: 5000
                    });
            }
        }

        function cutByHotkeys() {
            for (var i = 0; i < vm.space.folders.length; i++) {
                vm.space.folders[i].cutted = false;
            }
            for (var i = 0; i < vm.space.files.length; i++) {
                vm.space.files[i].cutted = false;
            }
            if (vm.row != undefined) {
                localStorageService.clearAll();
                localStorageService.set('cut-out', { id: vm.row, file: vm.condition });
                console.log('cutByHotkeys');
                console.log(JSON.parse(JSON.stringify(localStorageService.get('cut-out'))));
                localStorageService.set('oldParentId', vm.parentId);
                toastr.info(
                    'Item has been copied to the clipboard.',
                    'Space',
                    {
                        closeButton: true,
                        timeOut: 5000
                    });
                vm.cuttedRow = vm.row;
                vm.cuttedCondition = vm.condition;
                if (vm.condition) {
                    var pos = vm.space.files.map(function (e) { return e.id; }).indexOf(vm.row);
                    vm.space.files[pos].cutted = true;
                }
                else {
                    var pos = vm.space.folders.map(function (e) { return e.id; }).indexOf(vm.row);
                    vm.space.folders[pos].cutted = true;
                }
            }
        }

        function pasteByHotkeys() {
            if (localStorageService.get('cut-out') != null) {
                var item = localStorageService.get('cut-out');
                if (item.file) {
                    deleteFile(vm.cuttedRow,
                        function () {
                            fileService.getDeletedFile(item.id,
                                function (data) {
                                    var file = data;
                                    file.isDeleted = false;
                                    file.spaceId = vm.spaceId;
                                    file.parentId = vm.parentId;

                                    fileService.updateDeletedFile(file.id,
                                        localStorageService.get('oldParentId'),
                                        file,
                                        function () {
                                            if (vm.parentId == null) {
                                                vm.getSpace();
                                            } else {
                                                vm.getFolderContent(vm.parentId);
                                            }
                                        });
                                });
                        });
                } else {
                    deleteFolder(vm.cuttedRow,
                        function () {
                            folderService.getDeleted(item.id,
                                function (data) {
                                    var folder = data;
                                    folder.isDeleted = false;
                                    folder.spaceId = vm.spaceId;
                                    folder.parentId = vm.parentId;

                                    folderService.updateDeleted(folder.id,
                                        localStorageService.get('oldParentId'),
                                        folder,
                                        function () {
                                            if (vm.parentId == null) {
                                                vm.getSpace();
                                            } else {
                                                vm.getFolderContent(vm.parentId);
                                            }
                                        });
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

                    fileService.createCopyFile(localStorageService.get('copy').id,
                        file,
                        function () {
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

                    folderService.createCopy(localStorageService.get('copy').id,
                        folder,
                        function () {
                            if (vm.parentId == null) {
                                vm.getSpace();
                            } else {
                                vm.getFolderContent(vm.parentId);
                            }
                        });
                }
                localStorageService.set('copy', null);
            }
            vm.cuttedRow = undefined;
            vm.cuttedCondition = undefined;
        }

        function activate() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.changeView = changeView;
            vm.columnForOrder = 'name';
            vm.reverseSort = false;
            vm.iconHeight = 30;

            vm.space = {
                folders: [],
                files: []
            }

            if ($routeParams.id) {
                vm.spaceId = $routeParams.id;
                pagination();
                vm.showSettingsBtn = true;

            }
            if ($routeParams.spaceType) {
                spaceService.getSpaceByType($routeParams.spaceType,
                    function (data) {
                        vm.spaceId = data;
                        pagination();
                        // Hide settings space button for Binary and My space
                        if ($routeParams.spaceType == 'binaryspace' || $routeParams.spaceType == 'myspace') {
                            vm.showSettingsBtn = false;
                        }
                    });
                if ($routeParams.spaceType == 'binaryspace') {
                    vm.folderMenuOptionShareShow = false;
                    vm.fileMenuOptionShareShow = false;
                }
            }
        }

        function pagination() {
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
        }

        function currentSpaceId() {
            if ($routeParams.id) {
                return $routeParams.id;
            }
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
            spaceService.getSpace(vm.spaceId,
                vm.paginate.currentPage,
                vm.paginate.pageSize,
                vm.sortByDate,
                function (data) {
                    vm.space = data;
                    vm.spaceId = data.id;
                    vm.initSelection();
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
            spaceService.getSpaceTotal(vm.spaceId,
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
                'Edit', function ($itemScope) {
                    vm.folder = $itemScope.folder;
                    vm.folder.parentId = vm.parentId;
                    vm.folder.spaceId = vm.spaceId;
                    vm.openFolderWindow();
                }
            ],
            null,
            [
                'Copy', function ($itemScope) {
                    for (var i = 0; i < vm.space.folders.length; i++) {
                        vm.space.folders[i].cutted = false;
                    }
                    for (var i = 0; i < vm.space.files.length; i++) {
                        vm.space.files[i].cutted = false;
                    }
                    localStorageService.clearAll();
                    localStorageService.set('copy', { id: $itemScope.folder.id, file: false });
                }
            ],
            [
                'Cut', function ($itemScope) {
                    for (var i = 0; i < vm.space.folders.length; i++) {
                        vm.space.folders[i].cutted = false;
                    }
                    for (var i = 0; i < vm.space.files.length; i++) {
                        vm.space.files[i].cutted = false;
                    }
                    localStorageService.clearAll();
                    localStorageService.set('cut-out', { id: $itemScope.folder.id, file: false });
                    localStorageService.set('oldParentId', vm.parentId);
                    vm.cuttedRow = $itemScope.folder.id;
                    vm.cuttedCondition = false;
                    var pos = vm.space.folders.map(function (e) { return e.id; }).indexOf(vm.cuttedRow);
                    vm.space.folders[pos].cutted = true;
                }
            ],
            null,
            [
                'Share', function ($itemScope) {
                    vm.contentSharedId = $itemScope.folder.id;
                    vm.sharedModalWindowTitle = 'Shared folder';
                    console.log(vm.contentSharedId);
                    vm.sharedContent();
                }, function ($itemScope) { return vm.fileMenuOptionShareShow }
            ],
            [
                'Delete', function ($itemScope) {
                    return deleteFolder($itemScope.folder.id);
                }
            ]
        ];

        vm.fileMenuOptions = [
            [
                'Edit', function ($itemScope) {
                    vm.file = $itemScope.file;
                    vm.file.parentId = vm.parentId;
                    vm.file.spaceId = vm.spaceId;
                    if (vm.file.fileType !== 7) {
                        vm.openFileWindow();
                    }
                    else {
                        fileService.findCourse(vm.file.id, function (response) {
                            vm.course = response;
                            vm.openNewCourseWindow('lg');
                        });
                    }
                }
            ],
            null,
            [
                'Copy', function ($itemScope) {
                    for (var i = 0; i < vm.space.folders.length; i++) {
                        vm.space.folders[i].cutted = false;
                    }
                    for (var i = 0; i < vm.space.files.length; i++) {
                        vm.space.files[i].cutted = false;
                    }
                    localStorageService.clearAll();
                    localStorageService.set('copy', { id: $itemScope.file.id, file: true });
                }
            ],
            [
                'Cut', function ($itemScope) {
                    for (var i = 0; i < vm.space.folders.length; i++) {
                        vm.space.folders[i].cutted = false;
                    }
                    for (var i = 0; i < vm.space.files.length; i++) {
                        vm.space.files[i].cutted = false;
                    }
                    localStorageService.clearAll();
                    localStorageService.set('cut-out', { id: $itemScope.file.id, file: true });
                    localStorageService.set('oldParentId', vm.parentId);
                    vm.cuttedRow = $itemScope.file.id;
                    vm.cuttedCondition = true;
                    var pos = vm.space.files.map(function (e) { return e.id; }).indexOf(vm.cuttedRow);
                    vm.space.files[pos].cutted = true;
                }
            ],
            null,
            [
                'Share', function ($itemScope) {
                    vm.sharedModalWindowTitle = 'Shared File';
                    vm.contentSharedId = $itemScope.file.id;
                    console.log(vm.contentSharedId);
                    vm.sharedContent();
                }, function ($itemScope) { return vm.fileMenuOptionShareShow }
            ],
            [
                'Delete', function ($itemScope) {
                    return deleteFile($itemScope.file.id);
                }
            ]
        ];

        vm.containerMenuOptions = [
            ['New Folder', function () { vm.createNewFolder(); }],
            ['New File', function () { vm.createNewFile(); }],
            ['New Academy Pro', function () { vm.createNewAP(); }],
            null,
            [
                'Upload File', function ($itemScope) {
                    vm.file = { fileType: 6, parentId: vm.parentId, spaceId: vm.spaceId };
                    vm.openFileUploadWindow('lg');
                }
            ],
            null,
            [
                'Paste', function () {
                    if (localStorageService.get('cut-out') != null) {
                        var item = localStorageService.get('cut-out');
                        if (item.file) {
                            deleteFile(vm.cuttedRow, function () {
                                fileService.getDeletedFile(item.id,
                                    function (data) {
                                        var file = data;
                                        file.isDeleted = false;
                                        file.spaceId = vm.spaceId;
                                        file.parentId = vm.parentId;

                                        fileService.updateDeletedFile(file.id,
                                            localStorageService.get('oldParentId'),
                                            file,
                                            function () {
                                                if (vm.parentId == null) {
                                                    vm.getSpace();
                                                } else {
                                                    vm.getFolderContent(vm.parentId);
                                                }
                                            });
                                    });
                            });
                        } else {
                            deleteFolder(vm.cuttedRow, function () {
                                folderService.getDeleted(item.id,
                                    function (data) {
                                        var folder = data;
                                        folder.isDeleted = false;
                                        folder.spaceId = vm.spaceId;
                                        folder.parentId = vm.parentId;

                                        folderService.updateDeleted(folder.id,
                                            localStorageService.get('oldParentId'),
                                            folder,
                                            function () {
                                                if (vm.parentId == null) {
                                                    vm.getSpace();
                                                } else {
                                                    vm.getFolderContent(vm.parentId);
                                                }
                                            });
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

                            fileService.createCopyFile(localStorageService.get('copy').id,
                                file,
                                function () {
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

                            folderService.createCopy(localStorageService.get('copy').id,
                                folder,
                                function () {
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
            ]
        ];


        function openFolderWindow(size) {

            var folderModalInstance = $uibModal.open({
                animation: true,
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
                    } else {
                        vm.getFolderContent(vm.parentId);
                    }
                }
                if (response.operation == 'update') {
                    var index = findById(vm.space.folders, response.item.id);
                    if (index != -1) {
                        vm.space.folders[index] = response.item;
                    }
                }
            },
                function () {
                    console.log('Modal dismissed');
                });
        };

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
                if (response.operation == 'create') {
                    if (vm.parentId == null) {
                        vm.getSpace();
                    } else {
                        vm.getFolderContent(vm.parentId);
                    }
                }
                if (response.operation == 'update') {
                    var index = findById(vm.space.files, response.item.id);
                    if (index != -1) {
                        vm.space.files[index] = response.item;
                    }
                }
            },
                function () {
                    console.log('Modal dismissed');
                });
        }

        function openFileUploadWindow(size) {
            var fileModalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'Scripts/App/File/UploadFile.html',
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
                if (vm.parentId == null) {
                    vm.getSpace();
                }
                else {
                    vm.getFolderContent(vm.parentId);
                }
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
                            title: vm.sharedModalWindowTitle
                        }
                        return sharedContInfo;
                    }
                }
            });

            fileModalInstance.result.then(function (response) {
                console.log(response);
            },
                function () {
                    console.log('Modal dismissed');
                });
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
                        return vm.course;
                    }
                }
            });

            courseModalInstance.result.then(function (response) {
                $location.url('/apps/academy/' + response.id);
            },
                function () {
                    console.log('Modal dismissed');
                });
        };

        function sharedContent() {
            vm.fileId = { parentId: vm.parentId, spaceId: vm.spaceId };
            vm.openSharedContentWindow();
        }

        function createNewFolder() {
            vm.folder = { parentId: vm.parentId, spaceId: vm.spaceId };
            vm.openFolderWindow();
        }

        function createNewFile() {
            vm.file = { parentId: vm.parentId, spaceId: vm.spaceId };
            vm.openFileWindow();
        }

        function createNewAP() {
            vm.course = {
                fileUnit: { parentId: vm.parentId, spaceId: vm.spaceId }
            };
            vm.openNewCourseWindow('lg');
        }

        function uploadFile() {
            vm.file = { parentId: vm.parentId, spaceId: vm.spaceId };
            vm.openFileUploadWindow('lg');
        }

        function redirectToSpaceSettings(id) {
            $location.url("/spaces/" + id + "/settings/");
        };

        function getFolder(id) {
            folderService.get(id,
                function (folder) {
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

        function deleteFolder(id, callback) {
            folderService.deleteFolder(id,
                function () {
                    vm.paginate.getContent();
                    if (vm.parentId == null) {
                        getSpaceTotal();
                    } else {
                        getFolderContentTotal(vm.parentId);
                    }
                    if (callback) {
                        callback();
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
            folderService.getContent(vm.parentId,
                vm.paginate.currentPage,
                vm.paginate.pageSize,
                vm.sortByDate,
                function (data) {
                    vm.space.folders = data.folders;
                    vm.space.files = data.files;
                    vm.initSelection();
                });
        }

        function getFolderContentTotal(id) {
            folderService.getFolderContentTotal(id,
                function (data) {
                    vm.paginate.numberOfItems = data;
                });
        }

        function getFile(id) {
            fileService.getFile(id,
                function (file) {
                    vm.file = file;
                });
        }

        function deleteFile(id, callback) {
            fileService.deleteFile(id,
                function () {
                    vm.paginate.getContent();
                    if (vm.parentId == null) {
                        getSpaceTotal();
                    } else {
                        getFolderContentTotal(vm.parentId);
                    }
                    localStorageService.set('files', vm.space.files);
                    if (callback) {
                        callback();
                    }
                });
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
            spaceService.searchFoldersAndFiles(vm.spaceId,
                vm.parentId,
                vm.searchText,
                vm.paginate.currentPage,
                vm.paginate.pageSize,
                function (data) {
                    vm.space.folders = data.folders;
                    vm.space.files = data.files;
                });
        }

        function getNumberOfResultSearch() {
            spaceService.getNumberOfResultSearchFoldersAndFiles(vm.spaceId,
                vm.parentId,
                vm.searchText,
                function (data) {
                    vm.paginate.numberOfItems = data;
                });
        }

        function openDocument(file) {
            if (file.fileType !== 7) {
                if (file.fileType == 6) {
                    fileService.downloadFile(file.link);
                }
                else {
                    fileService.openFile(file.link);
                }
            } else {
                fileService.findCourse(file.id,
                    function (data) {
                        if (data !== undefined) {
                            $location.url('/apps/academy/' + data.id);
                        }
                    });
            }
        }

        function orderByColumn(column, reverse) {
            var comparator;
            switch (column) {
                case 'name':
                    comparator = function (a, b) {
                        if (a.name > b.name) { return !reverse ? 1 : -1; }
                        else { return reverse ? 1 : -1; }
                    }
                    vm.columnForOrder = 'name';
                    break;
                case 'description':
                    comparator = function (a, b) {
                        if (a.description > b.description) { return !reverse ? 1 : -1; }
                        else { return reverse ? 1 : -1; }
                    }
                    vm.columnForOrder = 'description';
                    break;
                case 'author.name':
                    comparator = function (a, b) {
                        if (a.author.name > b.author.name) { return !reverse ? 1 : -1; }
                        else { return reverse ? 1 : -1; }
                    }
                    vm.columnForOrder = 'author.name';
                    break;
                case 'createdAt':
                    comparator = function (a, b) {
                        if (a.createdAt > b.createdAt) { return !reverse ? 1 : -1; }
                        else { return reverse ? 1 : -1; }
                    }
                    vm.columnForOrder = 'createdAt';
                    break;
                case 'fileType':
                    comparator = function (a, b) {
                        if (a.fileType > b.fileType) { return !reverse ? 1 : -1; }
                        else { return reverse ? 1 : -1; }
                    }
                    vm.columnForOrder = 'fileType';
                    break;
            }
            vm.space.folders.sort(comparator);
            vm.space.files.sort(comparator);
        }

        function chooseIcon(type) {
            vm.iconSrc = fileService.chooseIcon(type);
            return vm.iconSrc;
        }

        //Drag'n'Drop
        function onDrop(event, channel, targetId, source) {
            if (event.shiftKey || event.ctrlKey) {
                vm.dndCopyContent(targetId, source);
            }
            else {
                vm.dndMoveContent(targetId, source);
            }
            console.log(source);

        }

        function dropValidate(target, source) {
            return !source.foldersId.some(
                function (id) {
                    return id == target.id;
                });
        }

        function dndMoveContent(folderId, selectedContent) {
            selectedContent.newParentId = folderId;
            selectedContent.spaceId = vm.space.id;
            spaceService.moveContent(selectedContent, function () {
                if (vm.parentId == null) {
                    vm.getSpace();
                } else {
                    vm.getFolderContent(vm.parentId);
                }
                toastr.success(
                    'Selected items were successfully moved!', 'Moving data',
                    {
                        closeButton: true, timeOut: 5000
                    });
            });
        }

        function dndCopyContent(folderId, selectedContent) {
            selectedContent.newParentId = folderId;
            selectedContent.spaceId = vm.space.id;
            spaceService.copyContent(selectedContent, function () {
                toastr.success(
                    'Selected items were successfully copied!', 'Copying data',
                    {
                        closeButton: true, timeOut: 5000
                    });
            });
        }

        function getDragImageId() {
            return 'customDrag1';
        }
        //Drag'n'Drop end 

        //Selection: select-multiselect
        function selectItems(event, item, isFile) {
            if (event.shiftKey) {
                resetSelection();
                if (vm.previousSelect.isFile && isFile) {// file & file
                    var prevIndex = vm.space.files.indexOf(vm.previousSelect.data);
                    var currIndex = vm.space.files.indexOf(item);
                    var start = (prevIndex < currIndex) ? prevIndex : currIndex;
                    var end = (prevIndex > currIndex) ? prevIndex : currIndex;
                    for (var i = start; i <= end; i++) {
                        vm.space.files[i].selected = true;
                    }
                }
                else {
                    if (!vm.previousSelect.isFile && !isFile) {// folder & folder
                        var prevIndex = vm.space.folders.indexOf(vm.previousSelect.data);
                        var currIndex = vm.space.folders.indexOf(item);
                        var start = (prevIndex < currIndex) ? prevIndex : currIndex;
                        var end = (prevIndex > currIndex) ? prevIndex : currIndex;
                        for (var i = start; i <= end; i++) {
                            vm.space.folders[i].selected = true;
                        }
                    }
                    else {// file & folder
                        if (isFile) {
                            var startFolder = vm.space.folders.indexOf(vm.previousSelect.data);
                            var startFile = vm.space.files.indexOf(item);
                        }
                        else {
                            var startFolder = vm.space.folders.indexOf(item);
                            var startFile = vm.space.files.indexOf(vm.previousSelect.data);
                        }
                        for (var i = startFolder; i < vm.space.folders.length; i++) {
                            vm.space.folders[i].selected = true;
                        }
                        for (var i = 0; i <= startFile; i++) {
                            vm.space.files[i].selected = true;
                        }
                    }
                }
            }
            else if (event.ctrlKey) {
                item.selected = !item.selected;
                vm.previousSelect = { data: item, isFile: isFile };
            }
            else {
                resetSelection();
                item.selected = true;
                vm.previousSelect = { data: item, isFile: isFile };
            }
        }

        function selectItemsForDrag(event, item, isFile) {
            if (event.which === 1 && !item.selected && !event.shiftKey && !event.ctrlKey) {
                resetSelection();
                item.selected = true;
                vm.previousSelect = { data: item, isFile: isFile };
            }
        }

        function rightClickSelection(item, isFile) {
            if (!item.selected) {
                resetSelection();
                item.selected = true;
                vm.previousSelect = { data: item, isFile: isFile };
            }
        }

        function initSelection() {
            if (vm.space.folders.length > 0) {
                vm.previousSelect = { data: vm.space.folders[0], isFile: false };
            }
            else if (vm.space.files.length > 0) {
                vm.previousSelect = { data: vm.space.files[0], isFile: true };
            }
        }

        function resetSelection() {
            vm.space.folders.forEach(function (item) { item.selected = false; })
            vm.space.files.forEach(function (item) { item.selected = false; });
        }

        function getSelectedItems() {
            var selected = { filesId: [], foldersId: [] };
            vm.space.files.forEach(function (f) { if (f.selected) selected.filesId.push(f.id) });
            vm.space.folders.forEach(function (f) { if (f.selected) selected.foldersId.push(f.id) });
            return selected;
        }
        //Selection end

    }
}());

(function () {
    "use strict";
    angular.module('driveApp')
    .directive('ngRightClick', function ($parse) {
        return function (scope, element, attrs) {
            var fn = $parse(attrs.ngRightClick);
            element.bind('contextmenu', function (event) {
                scope.$apply(function () {
                    event.preventDefault();
                    fn(scope, { $event: event });
                });
            });
        };
    });
})();