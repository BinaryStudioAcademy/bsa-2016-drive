(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SpaceController", SpaceController);

    SpaceController.$inject = ['SpaceService', 'FolderService', 'FileService', '$uibModal', 'localStorageService', '$routeParams', '$location'];

    function SpaceController(spaceService, folderService, fileService, $uibModal, localStorageService, $routeParams, $location) {
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

        vm.findById = findById;
        vm.getSpace = getSpace;
        vm.getSpaceByButton = getSpaceByButton;
        
        vm.createNewFolder = createNewFolder;
        vm.createNewFile = createNewFile;

        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.searchText = '';

        vm.redirectToSpaceSettings = redirectToSpaceSettings;

        vm.paginate = {
            currentPage: 1,
            pageSize: 2,
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

            spaceService.getSpace(vm.selectedSpace, vm.paginate.currentPage, vm.paginate.pageSize, function (data) {
                vm.space = data;
                vm.spaceId = data.id;
                console.log(localStorageService.get('current'));
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
            if ($routeParams.type) {
                if ($routeParams.type === "binaryspace") {
                    return 1;
                }
                if ($routeParams.type === "myspace") {
                    return 2;
                }
            }
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
            spaceService.getSpace(vm.selectedSpace, vm.paginate.currentPage, vm.paginate.pageSize, function (data) {
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
            spaceService.getSpaceTotal(vm.selectedSpace, function (data) {
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
            ['Share', function ($itemScope) {
                    console.log($itemScope.folder.id);
                }],
            ['Edit', function ($itemScope) {
                    vm.folder = $itemScope.folder;
                    vm.folder.parentId = vm.parentId;
                    vm.folder.spaceId = vm.spaceId;
                    vm.openFolderWindow();
                }],
            ['Delete', function ($itemScope) {
                    return deleteFolder($itemScope.folder.id);
                }]
        ];

        vm.fileMenuOptions = [
            ['Share', function ($itemScope) {
                    console.log($itemScope.file.id);
                }],
            ['Edit', function ($itemScope) {
                    vm.file = $itemScope.file;
                    vm.file.parentId = vm.parentId;
                    vm.file.spaceId = vm.spaceId;
                    vm.openFileWindow();
                }],
            ['Delete', function ($itemScope) {
                    return deleteFile($itemScope.file.id);
                }]
        ];

        vm.createOption = [
            ['Create folder', function () { vm.createNewFolder(); }],
            ['Create file', function () { vm.createNewFile(); }],
            null,
            ['Upload file', function(){}]
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
                        vm.getFolderContent(vm.parentId)
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
                        vm.getFolderContent(vm.parentId)
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

        function createNewFolder() {
            vm.folder = { parentId: vm.parentId, spaceId: vm.spaceId };
            vm.openFolderWindow();
        }

        function createNewFile() {
            vm.file = {parentId: vm.parentId, spaceId: vm.spaceId };
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
             folderService.getContent(vm.parentId, vm.paginate.currentPage, vm.paginate.pageSize, function (data) {
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
            vm.searchText = '';
            vm.paginate.currentPage = 1;
            vm.paginate.getContent = getResultSearchFoldersAndFiles;
            getResultSearchFoldersAndFiles();
            getNumberOfResultSearch();
        }

        function getResultSearchFoldersAndFiles() {
            spaceService.searchFoldersAndFiles(vm.spaceId, vm.parentId, vm.searchText, vm.paginate.currentPage,vm.paginate.pageSize, function (data) {
                vm.space.folders = data.folders;
                vm.space.files = data.files;
            });
        }

        function getNumberOfResultSearch(){
            spaceService.getNumberOfResultSearchFoldersAndFiles(vm.spaceId, vm.parentId, vm.searchText, function (data) {
                vm.paginate.numberOfItems = data;
            });
        }

        function openDocument(url) {
            window.open(url, '_blank');
        }
    }
}());