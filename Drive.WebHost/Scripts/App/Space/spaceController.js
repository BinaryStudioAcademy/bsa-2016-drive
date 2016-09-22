(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SpaceController", SpaceController);

    SpaceController.$inject = ['SpaceService', 'FolderService', 'FileService', 'TrashBinService', '$uibModal', 'localStorageService', '$routeParams', '$location', 'toastr', '$scope', 'hotkeys', 'Lightbox', '$cookies'];

    function SpaceController(spaceService,
        folderService,
        fileService,
        trashBinService,
        $uibModal,
        localStorageService,
        $routeParams,
        $location,
        toastr,
        $scope,
        hotkeys,
        Lightbox,
        $cookies) {
        var vm = this;

        vm.folderList = [];
        vm.addElem = addElem;
        vm.deleteElems = deleteElems;
        vm.parentId = null;

        // vm.getAllFolders = getAllFolders;
        vm.changeView = changeView;
        vm.getFolder = getFolder;
        vm.openFolderWindow = openFolderWindow;
        vm.getFolderContent = getFolderContent;

        vm.getFile = getFile;
        vm.openFileWindow = openFileWindow;
        vm.openFileUploadWindow = openFileUploadWindow;
        vm.openDocument = openDocument;
        vm.openNewCourseWindow = openNewCourseWindow;
        vm.createNewAP = createNewAP;
        vm.openSharedContentWindow = openSharedContentWindow;
        vm.openShareByLinkWindow = openShareByLinkWindow;
        vm.openTextFileReader = openTextFileReader;

        vm.sharedContent = sharedContent;

        vm.findById = findById;
        vm.getSpace = getSpace;
        vm.getSpaceByButton = getSpaceByButton;
        vm.resetPath = resetPath;

        vm.createNewFolder = createNewFolder;
        vm.createNewFile = createNewFile;
        vm.uploadFile = uploadFile;
        vm.createNewEvent = createNewEvent;

        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.searchText = '';

        vm.orderByColumn = orderByColumn;
        vm.chooseIcon = chooseIcon;

        vm.redirectToSpaceSettings = redirectToSpaceSettings;

        ////// Copy-Cut-Paste-Delelte operations
        vm.pushToClipboard = pushToClipboard;
        vm.pasteFromClipboard = pasteFromClipboard;
        vm.clearClipboard = clearClipboard;
        vm.deleteContent = deleteContent;

        vm.copyByHotkeys = copyByHotkeys;
        vm.pasteByHotkeys = pasteByHotkeys;
        vm.cutByHotkeys = cutByHotkeys;
        vm.deleteByHotkeys = deleteByHotkeys;
        vm.undoByHotkeys = undoByHotkeys;
        vm.selectAllByHotkeys = selectAllByHotkeys;

        ////// Drag and Drop
        vm.onDrop = onDrop;
        vm.dropValidate = dropValidate;
        vm.dndMoveContent = dndMoveContent;
        vm.dndCopyContent = dndCopyContent;
        vm.getDragImageId = getDragImageId;
        vm.clearDragImage = clearDragImage;

        ////// Multiselect
        vm.selectItems = selectItems;
        vm.selectItemsForDrag = selectItemsForDrag;
        vm.rightClickSelection = rightClickSelection;
        vm.initSelection = initSelection;
        vm.getSelectedItems = getSelectedItems;

        vm.openLightboxModal = openLightboxModal;
        vm.checkFileType = checkFileType;

        vm.paginate = {
            currentPage: 1,
            pageSize: 15,
            numberOfItems: 0,
            getContent: null
        }

        vm.folderMenuOptionShareShow = true;
        vm.fileMenuOptionShareShow = true;
        vm.sharedModalWindowTitle = null;

        vm.reloadContent = reloadContent;

        activate();

        function activate() {
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
            vm.changeView = changeView;
            vm.columnForOrder = 'name';
            vm.reverseSort = false;
            vm.iconHeight = 30;

            vm.space = {
                id: null,
                name: '',
                folders: [],
                files: [],
                canModifySpace: null
            }

            vm.images = [];

            if ($routeParams.id) {
                vm.space.id = $routeParams.id;
                resetPath();
                pagination();
                vm.showSettingsBtn = true;

            }
            if ($routeParams.spaceType) {
                spaceService.getSpaceByType($routeParams.spaceType, function (data) {
                        vm.space.id = data;
                        pagination();
                        // Hide settings space button for Binary and My space
                        if ($routeParams.spaceType === 'binaryspace' || $routeParams.spaceType === 'myspace') {
                            vm.showSettingsBtn = false;
                        }
                    });
                if ($routeParams.spaceType == 'binaryspace') {
                    vm.folderMenuOptionShareShow = false;
                    vm.fileMenuOptionShareShow = false;
                }
                resetPath();
            }
        }

        function pagination() {
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
            spaceService.getSpace(vm.space.id,
                vm.paginate.currentPage,
                vm.paginate.pageSize,
                vm.sortByDate,
                function(data) {
                    vm.space = data;
                    localStorageService.set('space',
                        { spaceId: data.id, name: data.name, ownerId: data.owner.globalId });

                    for (var k = 0; k < vm.space.files.length; k++) {
                        var file = vm.space.files[k];

                        if (file.fileType === 8) {
                            file.thumbUrl = file.link;
                            vm.images.push({
                                url: file.link,
                                link: file.link,
                                caption: file.name,
                                fileType: file.fileType,
                                created: file.createdAt,
                                fileId: file.id
                            });
                        } else if (file.fileType === 10) {
                            vm.images.push({
                                url: file.link,
                                link: file.link,
                                caption: file.name,
                                thumbUrl: file.link,
                                fileType: file.fileType,
                                created: file.createdAt,
                                fileId: file.id,
                                type: 'video'
                            });
                        }
                    }

                    vm.initSelection();
                });
        }

        function openLightboxModal(fileId) {
            var i;
            for (i = 0; i < vm.images.length; i++) {
                if (vm.images[i].fileId === fileId) {
                    break;
                }
            }

            if (vm.images[i].link.indexOf('http') === -1) {
                fileService.getImage(vm.images[i].link,
                    function(response) {
                        var fileData = response.data;
                        var fileHeader = response.headers();
                        var contentType = fileHeader['content-type'];
                        var blob = new Blob([fileData], { type: contentType });
                        var url = URL.createObjectURL(blob);
                        vm.images[i].url = url;
                        if (blob instanceof Blob) {
                            Lightbox.openModal(vm.images, i);
                        }
                    });
            } else {
                Lightbox.openModal(vm.images, i);
            }
        }

        function getSpaceByButton() {
            getSpace();
            localStorageService.set('list', []);
            vm.folderList = localStorageService.get('list');
            vm.parentId = null;
            localStorageService.set('current', null);
        }

        function resetPath() {
            if ($location.path() != localStorageService.get('location')) {
                localStorageService.set('location', $location.path());
                localStorageService.set('current', null);
                vm.parentId = null;
                localStorageService.set('list', null);
            }
            else {
                vm.space.name = localStorageService.get('space').name;
                vm.space.owner = { globalId: localStorageService.get('space').ownerId };
            }
        }

        function checkFileType(fileType) {
            if (fileType === 8 || fileType === 10) {
                return false;
            } else {
                return true;
            }
        }

        function getSpaceTotal() {
            spaceService.getSpaceTotal(vm.space.id, function (data) {
                    vm.paginate.numberOfItems = data;
                });
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


        vm.folderMenuOptions = [
            [
                'Edit', function ($itemScope) {
                    vm.folder = $itemScope.folder;
                    vm.folder.parentId = vm.parentId;
                    vm.folder.spaceId = vm.space.id;
                    vm.openFolderWindow();
                }, function ($itemScope) { return $itemScope.folder.canModify }
            ],
            null,
            [
                'Copy', function ($itemScope) {
                    var content = getSelectedItems($itemScope.folder, false);
                    pushToClipboard(content, true);
                }, function ($itemScope) { return $itemScope.folder.canModify }
            ],
            [
                'Cut', function ($itemScope) {
                    var content = getSelectedItems($itemScope.folder, false);
                    pushToClipboard(content, false);
                }, function ($itemScope) { return $itemScope.folder.canModify }
            ],
            null,
            [
                'Share', function ($itemScope) {
                    vm.contentShared = $itemScope.folder;
                    vm.sharedModalWindowTitle = 'Shared folder';
                    console.log(vm.contentShared);
                    vm.sharedContent();
                }, function ($itemScope) { return vm.fileMenuOptionShareShow  && $itemScope.folder.canModify }
            ],
            [
                'Share by link', function ($itemScope) {
                    vm.sharedContent('byLink');
                }, function ($itemScope) { return vm.fileMenuOptionShareShow && $itemScope.folder.canModify; }
            ],
            [
                'Delete', function ($itemScope) {
                    var content = getSelectedItems($itemScope.folder, false);
                    deleteContent(content);
                }, function ($itemScope) { return $itemScope.folder.canModify }
            ]
        ];

        vm.fileMenuOptions = [
            [
                'Edit', function ($itemScope) {
                    vm.file = $itemScope.file;
                    vm.file.parentId = vm.parentId;
                    vm.file.spaceId = vm.space.id;
                    if (vm.file.fileType === 7) {
                        fileService.findCourse(vm.file.id, function (response) {
                            vm.course = response;
                            vm.openNewCourseWindow('lg');
                        });
                        
                    }
                    else {
                        if (vm.file.fileType === 9) {
                            fileService.findEvent(vm.file.id, function (response) {
                                vm.event = response;
                                localStorageService.set('container', 'space');
                                $location.url('/apps/events/' + vm.event.id + '/edit');
                            });
                        }
                        else {
                            vm.openFileWindow();
                        }
                    }
                }, function ($itemScope) {
                    if ($itemScope.file.fileType == 7 || $itemScope.file.fileType == 9) {
                        if ($cookies.get('serverUID') == $itemScope.file.author.globalId) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    return $itemScope.file.canModify;
                }
            ],
            null,
            [
                'Copy', function ($itemScope) {
                    var content = getSelectedItems($itemScope.file, true);
                    pushToClipboard(content, true);
                }, function ($itemScope) { return $itemScope.file.canModify; }
            ],
            [
                'Cut', function ($itemScope) {
                    var content = getSelectedItems($itemScope.file, true);
                    pushToClipboard(content, false);
                }, function ($itemScope) {
                    if ($itemScope.file.fileType == 7 || $itemScope.file.fileType == 9) {
                        if ($cookies.get('serverUID') == $itemScope.file.author.globalId) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    return $itemScope.file.canModify;
                }
            ],
            null,
            [
                'Share', function ($itemScope) {
                    vm.sharedModalWindowTitle = 'Shared File';
                    vm.contentShared = $itemScope.file;
                    vm.sharedContent();
                }, function ($itemScope) { return vm.fileMenuOptionShareShow && $itemScope.file.canModify; }
            ],
            [
                'Share by link', function ($itemScope) {
                   vm.sharedContent('byLink');
                }, function ($itemScope) { return vm.fileMenuOptionShareShow && $itemScope.file.canModify; }
            ],
            [
                'Delete', function ($itemScope) {
                    var content = getSelectedItems($itemScope.file, true);
                    deleteContent(content);
                }, function ($itemScope) {
                    if ($itemScope.file.fileType == 7 || $itemScope.file.fileType == 9) {
                        if ($cookies.get('serverUID') == $itemScope.file.author.globalId) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                    return $itemScope.file.canModify;
                }
            ]
        ];

        vm.containerMenuOptions = [
            ['New Folder', function () { vm.createNewFolder(); }, function () { return vm.space.canModifySpace; }],
            ['New File', function () { vm.createNewFile(); }, function () { return vm.space.canModifySpace; }],
            ['New Academy Pro', function () { vm.createNewAP(); }, function () { return vm.space.canModifySpace; }], ,
            ['New Event', function () { vm.createNewEvent(); }, function () { return vm.space.canModifySpace;}],
            null,
            ['Upload File',
                function ($itemScope) {
                    vm.file = { fileType: 6, parentId: vm.parentId, spaceId: vm.space.id, maxSize: vm.space.maxFileSize };
                    vm.openFileUploadWindow('lg');
                }, function () { return vm.space.canModifySpace; }
            ],
            null,
            [
                'Paste', function () {
                    var data = localStorageService.get('clipboard');
                    var isCopy = localStorageService.get('isCopy');
                    pasteFromClipboard(data, isCopy);
                }, function ($itemScope) {
                    var data = localStorageService.get('clipboard');
                    return data != null && vm.space.canModifySpace;
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
                            content: vm.contentShared,
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

        function openShareByLinkWindow(size) {

            var fileModalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'Scripts/App/ShareByLink/ShareByLinkForm.html',
                windowTemplateUrl: 'Scripts/App/ShareByLink/Modal.html',
                controller: 'ShareByLinkController',
                controllerAs: 'shareByLinkCtrl',
                size: size,
                resolve: {
                    items: function () {
                        return vm.sharedByLinkContent;
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

        function openTextFileReader(size, file) {

            var fileReaderModalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'Scripts/App/File/TextFileReader/TextFileReader.html',
                windowTemplateUrl: 'Scripts/App/File/TextFileReader/Modal.html',
                controller: 'TextFileReaderCtrl',
                controllerAs: 'textFileReaderCtrl',
                size: size,
                resolve: {
                    items: function () {
                        return file;
                    }
                }
            });

            fileReaderModalInstance.result.then(function (response) {
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
                console.log(response);
                if (vm.parentId == null) {
                    vm.getSpace();
                }
                else {
                    vm.getFolderContent(vm.parentId);
                }
            },
                function () {
                    console.log('Modal dismissed');
                });
        };

        function sharedContent(shareType) {
            if (shareType == 'byLink') {
                var content = { folders: [], files: [] };
                vm.space.files.forEach(function (f) { if (f.selected) content.files.push(f) });
                vm.space.folders.forEach(function (f) { if (f.selected) content.folders.push(f) });
                vm.sharedByLinkContent = content;
                vm.openShareByLinkWindow();
            }
            else {
                vm.fileId = { parentId: vm.parentId, spaceId: vm.space.id };
                vm.openSharedContentWindow();
            }
        }

        function createNewFolder() {
            vm.folder = { parentId: vm.parentId, spaceId: vm.space.id };
            vm.openFolderWindow();
        }

        function createNewFile() {
            vm.file = { parentId: vm.parentId, spaceId: vm.space.id };
            vm.openFileWindow();
        }

        function createNewAP() {
            vm.course = {
                fileUnit: { parentId: vm.parentId, spaceId: vm.space.id }
            };
            vm.openNewCourseWindow('lg');
        }

        function createNewEvent() {
            vm.event = {
                fileUnit: { parentId: vm.parentId, spaceId: vm.space.id }
            };
            localStorageService.set('event', vm.event);
            $location.url('/apps/events/newevent');

        }

        function uploadFile() {
            vm.file = { parentId: vm.parentId, spaceId: vm.space.id, maxSize: vm.space.maxFileSize };
            vm.openFileUploadWindow('lg');
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
                    vm.space.canModifySpace = data.canModifySpace;
                    vm.initSelection();
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
            spaceService.searchFoldersAndFiles(vm.space.id,
                vm.parentId,
                vm.searchText,
                vm.paginate.currentPage,
                vm.paginate.pageSize,
                function (data) {
                    vm.space.folders = data.folders;
                    vm.space.files = data.files;
                    vm.space.canModifySpace = data.canModifySpace;
                });
        }

        function getNumberOfResultSearch() {
            spaceService.getNumberOfResultSearchFoldersAndFiles(vm.space.id,
                vm.parentId,
                vm.searchText,
                function (data) {
                    vm.paginate.numberOfItems = data;
                });
        }

        function openDocument(file) {
            if (file.fileType !== 7) {
                if (file.fileType == 6) {
                    var fileExtantion = file.name.slice(file.name.lastIndexOf("."));
                    if (fileExtantion == '.pdf' ||
                        fileExtantion == '.txt' ||
                        fileExtantion == '.cs' ||
                        fileExtantion == '.js' ||
                        fileExtantion == '.html' ||
                        fileExtantion == '.css') {
                        vm.openTextFileReader('lg', file);
                    } else {
                        fileService.downloadFile(file.link);
                    }
                } else if (file.fileType === 9) {
                    fileService.findEvent(file.id,
                        function(data) {
                            if (data != undefined) {
                                $location.url('/apps/events/' + data.id);
                            }
                        });
                } else {
                    fileService.openFile(file.link);
                }
            } else {
                fileService.findCourse(file.id,
                    function(data) {
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

        ////// Copy-Cut-Paste-Delelte operations
        function pushToClipboard(data, isCopy) {
            data.parentId = vm.parentId;
            data.spaceId = vm.space.id;
            localStorageService.set('clipboard', data);
            localStorageService.set('isCopy', isCopy);
            if (isCopy) {
                unmarkAsCutted();
                toastr.success(
                    'Data copied to clipboard!', 'Copy',
                    {
                        closeButton: true, timeOut: 5000
                    });
            }
            else {
                markSelectedAsCutted();
                toastr.success(
                    'Data cutted to clipboard!', 'Cut',
                    {
                        closeButton: true, timeOut: 5000
                    });
            }
        }

        function pasteFromClipboard(data, isCopy) {
            if (data != null && isCopy != null) {
                if (isCopy) {
                    data.parentId = vm.parentId;
                    data.spaceId = vm.space.id;
                    spaceService.copyContent(data, function () {
                        if (vm.parentId == null) {
                            vm.getSpace();
                        } else {
                            vm.getFolderContent(vm.parentId);
                        }
                        toastr.success(
                            'Data successfully copied!', 'Paste',
                            {
                                closeButton: true, timeOut: 5000
                            });
                    });
                }
                else {
                    if (data.parentId != vm.parentId || data.spaceId != vm.space.id && vm.parentId == null) {
                        data.parentId = vm.parentId;
                        data.spaceId = vm.space.id;
                        spaceService.moveContent(data, function () {
                            if (vm.parentId == null) {
                                vm.getSpace();
                            } else {
                                vm.getFolderContent(vm.parentId);
                            }
                            toastr.success(
                                'Data successfully moved!', 'Paste',
                                {
                                    closeButton: true, timeOut: 5000
                                });
                        });
                    }
                    else {
                        unmarkAsCutted();
                    }
                    clearClipboard();
                }
            }
            else {
                // clipboard is empty
            }
        }

        function clearClipboard() {
            localStorageService.remove('clipboard', 'isCopy');
        }

        function deleteContent(content) {
            content.parentId = vm.parentId;
            content.spaceId = vm.space.id;
            var data = localStorageService.get('clipboard');
            if (data) {
                content.foldersId.forEach(function (f) {
                    if (data.foldersId.some(function (d) { return d == f })) {
                        clearClipboard();
                        unmarkAsCutted();
                    }
                });
                content.filesId.forEach(function (f) {
                    if (data.filesId.some(function (d) { return d == f })) {
                        clearClipboard();
                        unmarkAsCutted();
                    }
                });
            }
            spaceService.deleteContent(content, function () {
                vm.paginate.getContent();
                if (vm.parentId == null) {
                    getSpaceTotal();
                } else {
                    getFolderContentTotal(vm.parentId);
                }
                vm.deletedContent = content;
                toastr.success(
                    'Data moved to trash bin!', 'Delete',
                    {
                        closeButton: true, timeOut: 5000
                    });
            });
        }

        ////// HotKeys
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
        .add({
            combo: 'ctrl+a',
            callback: function (event, hotkey) {
                event.preventDefault();
                selectAllByHotkeys();
            }
        })

        function undoByHotkeys() {
            if (vm.deletedContent != null) {
                trashBinService.restoreContent(vm.deletedContent, function () {
                    vm.deletedContent = null;
                    if (vm.parentId == null) {
                        vm.getSpace();
                    } else {
                        vm.getFolderContent(vm.parentId);
                    }
                    toastr.success(
                        'Data was successfully restored!', 'Undo',
                        {
                            closeButton: true, timeOut: 5000
                        });
                });
            }
        }

        function deleteByHotkeys() {
            if (vm.space.canModifySpace) {
                var content = getSelectedItems();
                if (content) {
                    deleteContent(content);
                }
            }
        }

        function copyByHotkeys() {
            if (vm.space.canModifySpace) {
                var content = getSelectedItems();
                if (content) {
                    pushToClipboard(content, true);
                }
            }
        }

        function cutByHotkeys() {
            if (vm.space.canModifySpace) {
                var content = getSelectedItems();
                if (content) {
                    pushToClipboard(content, false);
                }
            }
        }

        function pasteByHotkeys() {
            if (vm.space.canModifySpace) {
                var data = localStorageService.get('clipboard');
                var isCopy = localStorageService.get('isCopy');
                pasteFromClipboard(data, isCopy);
            }
        }

        function selectAllByHotkeys() {
            vm.space.files.forEach(function (f) { f.selected = true; });
            vm.space.folders.forEach(function (f) { f.selected = true; });
        }


        ////// Drag'n'Drop
        function onDrop(event, channel, targetId, source) {
            if (event.shiftKey || event.ctrlKey) {
                vm.dndCopyContent(targetId, source);
            }
            else {
                vm.dndMoveContent(targetId, source);
            }
        }

        $scope.handleDragStart = function (event) {
            markAsDragging();
            if (!this.classList.contains('selected') && event.shiftKey || !this.classList.contains('selected') && event.ctrlKey) {
                resetSelection();
                unmarkAsDragging();
                this.classList.add('selected');
            }
        };

        $scope.handleDragEnd = function (event) {
            unmarkAsDragging();
        };

        function dropValidate(target, source) {
            return !source.foldersId.some(function (id) {
                    return id == target.id;
                });
        }

        function dndMoveContent(folderId, selectedContent) {
            selectedContent.parentId = folderId;
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
            selectedContent.parentId = folderId;
            selectedContent.spaceId = vm.space.id;
            spaceService.copyContent(selectedContent, function () {
                toastr.success(
                    'Selected items were successfully copied!', 'Copying data',
                    {
                        closeButton: true, timeOut: 5000
                    });
            });
        }

        function getDragImageId(event, item, isFile) {
            if (!item.selected && event.shiftKey || !item.selected && event.ctrlKey) {
                item.selected = true;
                item.cutted = true;
                vm.previousSelect = { data: item, isFile: isFile };
            }
            for (var i = 0; i < vm.space.folders.length; i++) {
                if (vm.space.folders[i].selected) {
                    var p = document.createElement("P");
                    var icon = document.createElement("IMG");
                    icon.setAttribute("src", "./Content/Icons/folder.svg");
                    icon.setAttribute("width", "20");
                    icon.setAttribute("width", "20");
                    var t = document.createTextNode(' ' + vm.space.folders[i].name);
                    p.appendChild(icon);
                    p.appendChild(t);
                    document.getElementById("dragPreview").appendChild(p);
                }
            }
            for (var i = 0; i < vm.space.files.length; i++) {
                if (vm.space.files[i].selected) {
                    var p = document.createElement("P");
                    var icon = document.createElement("IMG");
                    icon.setAttribute("src", chooseIcon(vm.space.files[i].fileType));
                    icon.setAttribute("width", "20");
                    icon.setAttribute("width", "20");
                    var t = document.createTextNode(' ' + vm.space.files[i].name);
                    p.appendChild(icon);
                    p.appendChild(t);
                    document.getElementById("dragPreview").appendChild(p);
                }
            }
            return 'dragPreview';
        }

        function clearDragImage() {
            var dragView = document.getElementById("dragPreview");

            while (dragView.hasChildNodes()) {
                dragView.removeChild(dragView.firstChild);
            }
        }
        //Drag'n'Drop end 


        ////// Selection: select-multiselect
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
            var data = localStorageService.get('clipboard');
            var isCopy = localStorageService.get('isCopy');
            if (data != null && !isCopy) {
                vm.space.files.forEach(function (f) {
                    f.selected = false;
                    f.cutted = (data.filesId.some(function(id){return f.id == id;})) ? true : false;
                });
                vm.space.folders.forEach(function (f) {
                    f.selected = false;
                    f.cutted = (data.foldersId.some(function (id) { return f.id == id; })) ? true : false;
                });
            }
            else {
                vm.space.files.forEach(function (f) { f.selected = false; f.cutted = false; });
                vm.space.folders.forEach(function (f) { f.selected = false; f.cutted = false; });
            }
        }

        function resetSelection() {
            vm.space.folders.forEach(function (item) { item.selected = false; })
            vm.space.files.forEach(function (item) { item.selected = false; });
        }

        function markSelectedAsCutted() {
            vm.space.folders.forEach(function (f) { f.cutted = f.selected });
            vm.space.files.forEach(function (f) { f.cutted = f.selected });
        }

        function unmarkAsCutted() {
            vm.space.folders.forEach(function (f) { f.cutted = false });
            vm.space.files.forEach(function (f) { f.cutted = false });
        }

        function markAsDragging() {
            vm.space.folders.forEach(function (f) { f.isDragging = f.selected });
            vm.space.files.forEach(function (f) { f.isDragging = f.selected });
        }

        function unmarkAsDragging() {
            vm.space.folders.forEach(function (f) { f.isDragging = false });
            vm.space.files.forEach(function (f) { f.isDragging = false });
        }

        function getSelectedItems(item, isFile) {
            var selected = { filesId: [], foldersId: [] };
            vm.space.files.forEach(function (f) { if (f.selected) selected.filesId.push(f.id) });
            vm.space.folders.forEach(function (f) { if (f.selected) selected.foldersId.push(f.id) });
            if (selected.filesId.length == 0 && selected.foldersId.length == 0) {
                if (!item) { return false; }
                if (isFile) { selected.filesId.push(item.id); }
                else { selected.foldersId.push(item.id); }
            }
            return selected;
        }
        //Selection end
        function reloadContent() {
            if (vm.parentId == null) {
                vm.getSpace();
            } else {
                vm.getFolderContent(vm.parentId);
    }
        }
    }
}());