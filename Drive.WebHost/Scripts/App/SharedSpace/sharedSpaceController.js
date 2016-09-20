(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SharedSpaceController", SharedSpaceController);

    SharedSpaceController.$inject = ['SharedSpaceService', 'FolderService', 'FileService', '$uibModal', 'localStorageService', '$routeParams', '$location', 'Lightbox'];

    function SharedSpaceController(sharedSpaceService, folderService, fileService, $uibModal, localStorageService, $routeParams, $location, Lightbox) {
        var vm = this;

        vm.folderList = [];
        vm.addElem = addElem;
        vm.deleteElems = deleteElems;

        vm.space = {
            name: 'Shared Space',
            rootFolderId: null,
            folderId: null,
            folders: [],
            files: []
        }

        vm.images = [];

        vm.getFolderContent = getFolderContent;
        vm.getFile = getFile;
        vm.getSharedData = getSharedData;
        vm.getSpaceByButton = getSpaceByButton;
        vm.openDocument = openDocument;
        vm.openFolderWindow = openFolderWindow;
        vm.openFileWindow = openFileWindow;
        vm.openNewCourseWindow = openNewCourseWindow;
        vm.findById = findById;

        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.searchText = '';

        vm.orderByColumn = orderByColumn;

        vm.openLightboxModal = openLightboxModal;
        vm.checkFileType = checkFileType;

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

        vm.classImageWrap = 'sp-gv-item-img-wrapper-image';
        vm.classThumbnail = 'img-thumbnail';

        vm.chooseIcon = chooseIcon;

        activate();

        // TODO change method 
        function activate() {
            vm.space.rootFolderId = null;
            vm.space.folderId = null;

            var view = localStorageService.get('view');
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
            vm.sortByDate = null;
            vm.reverse = false;
            vm.iconHeight = 30;
            vm.columnForOrder = 'name';

            getSharedData();
        }

        function getSharedData() {
            vm.searchText = '';
            vm.parentId = null;
            vm.paginate.currentPage = 1;
            getSharedContent();
            getSharedDataTotal();
            vm.paginate.getContent = getSharedContent;
        }

        function getSharedContent() {
            sharedSpaceService.getSharedData(vm.paginate.currentPage,
                vm.paginate.pageSize,
                vm.sortByDate,
                vm.space.folderId,
                vm.space.rootFolderId,
                function(data) {
                    vm.space.files = data.files;
                    vm.space.folders = data.folders;

                    for (var k = 0; k < vm.space.files.length; k++) {
                        var file = vm.space.files[k];

                        if (file.fileType === 8) {
                            file.thumbUrl = file.link;
                            if (file.link.indexOf('http') === -1) {
                                file.thumbUrl = chooseIcon(file.fileType);
                                vm.classImageWrap = 'sp-gv-item-img-wrapper';
                                vm.classThumbnail = '';
                            }
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
                    function (response) {
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
            vm.space.rootFolderId = null;
            vm.space.folderId = null;
            getSharedData();
            localStorageService.set('list', []);
            vm.folderList = localStorageService.get('list');
            localStorageService.set('current', null);
        }

        function getSharedDataTotal() {
            sharedSpaceService.getSharedDataTotal(vm.space.folderId, vm.space.rootFolderId, function (data) {
                vm.paginate.numberOfItems = data;
            });
        }

        function checkFileType(fileType) {
            if (fileType === 8 || fileType === 10) {
                return false;
            } else {
                return true;
            }
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
                }, function ($itemScope) {
                    if (vm.space.rootFolderId != null) {
                        return false;
                    }
                    return $itemScope.folder.canModify;
                }
            ],
            ['Delete', function ($itemScope) {
                sharedSpaceService.deleteSharedContent($itemScope.folder.id, function () {
                    if (vm.space.folders.lenght = 1 && vm.paginate.currentPage != 1) {
                        vm.paginate.currentPage--;
                        vm.paginate.numberOfItems--;
                        vm.paginate.getContent();
                    }
                    else {
                        vm.paginate.numberOfItems--;
                        vm.paginate.getContent();
                    }
                }, function ($itemScope) {
                    if (vm.space.rootFolderId == null) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
            }, function ($itemScope) {
                if (vm.space.rootFolderId == null) {
                    return true;
                }
                else {
                    return false;
                }
            }]
        ];

        vm.fileMenuOptions = [
            ['Edit', function ($itemScope) {
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
                            localStorageService.set('container', 'shared');
                            $location.url('/apps/events/' + vm.event.id + '/edit');
                        });
                    }
                    else {
                        vm.openFileWindow();
                    }
                }
            }, function ($itemScope) { return $itemScope.file.canModify; }
            ],
           ['Delete', function ($itemScope) {
               sharedSpaceService.deleteSharedContent($itemScope.file.id, function () {
                   if (vm.space.files.lenght = 1 && vm.paginate.currentPage != 1) {
                       vm.paginate.currentPage--;
                       vm.paginate.numberOfItems--;
                       vm.paginate.getContent();
                   }
                   else {
                       vm.paginate.numberOfItems--;
                       vm.paginate.getContent();
                   }
               });
           }, function ($itemScope) {
               if (vm.space.rootFolderId == null) {
                   return true;
               }
               else {
                   return false;
               }
           }]
        ];

        function getFolderContent(id) {
            if (vm.space.rootFolderId == null) {
                vm.space.rootFolderId = id;
            }
            vm.space.folderId = id;
            getSharedData();
            localStorageService.set('current', id);
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
            vm.searchText = '';
            vm.paginate.currentPage = 1;
            vm.paginate.getContent = getResultSearchFoldersAndFiles;
            getResultSearchFoldersAndFiles();
            getNumberOfResultSearch();
        }

        function getResultSearchFoldersAndFiles() {
            sharedSpaceService.search(vm.searchText, vm.paginate.currentPage, vm.paginate.pageSize, vm.sortByDate, vm.space.folderId, vm.space.rootFolderId, function (data) {
                vm.space.folders = data.folders;
                vm.space.files = data.files;
            });
        }

        function getNumberOfResultSearch(){
            sharedSpaceService.searchTotal(vm.searchText, vm.space.folderId, vm.space.rootFolderId, function (data) {
                vm.paginate.numberOfItems = data;
            });
        }

        function openDocument(file) {
            switch (file.fileType) {
                case 6: {
                    fileService.downloadFile(file.link);
                    break;
                }
                case 7: {
                    fileService.findCourse(file.id,
                        function(data) {
                            if (data !== undefined) {
                                $location.url('/apps/academy/' + data.id);
                            }
                        });
                    break;
                }
                case 9: {
                    fileService.findEvent(file.id,
                        function (data) {
                            if (data != undefined) {
                                $location.url('/apps/events/' + data.id);
                            }
                        });
                    break;
                }
                default: {
                    window.open(file.link, '_blank');
                    break;
                }
            }
        }

        function orderByColumn(column) {
            vm.columnForOrder = fileService.orderByColumn(column, vm.columnForOrder);
        }

        function chooseIcon(type) {
            vm.iconSrc = fileService.chooseIcon(type);
            return vm.iconSrc;
        }

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
                var index = findById(vm.space.files, response.fileUnit.id);
                if (index != -1) {
                    vm.space.files[index] = response.fileUnit;
                }
            },
                function () {
                    console.log('Modal dismissed');
                });
        };

        function findById(data, id) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].id === id) {
                    return i;
                }
            }
            return -1;
        };
    }
}());