(function () {
    "use strict";

    angular.module("driveApp")
        .controller("TrashBinController", TrashBinController);

    TrashBinController.$inject = ['TrashBinService', 'FileService', '$routeParams', 'toastr'];

    function TrashBinController(trashBinService, fileService, $routeParams, toastr) {
        var vm = this;

        vm.changeView = changeView;
        vm.chooseIcon = chooseIcon;
        vm.orderByColumn = orderByColumn;

        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.getTrashBinContent = getTrashBinContent;
        vm.deleteFilePermanently = deleteFilePermanently;
        vm.deleteFolderPermanently = deleteFolderPermanently;
        vm.restoreFile = restoreFile;
        vm.restoreFolder = restoreFolder;
        vm.deleteFileFromScope = deleteFileFromScope;
        vm.deleteFolderFromScope = deleteFolderFromScope;

        activate();

        function activate() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.columnForOrder = 'name';
            vm.searchText = '';
            vm.iconHeight = 30;

            vm.spaces = [];

            getTrashBinContent();
        }

        function getTrashBinContent() {
            trashBinService.getTrashBinContent(function (data) {
                vm.spaces = data;
                console.log(data);
            });
        }

        function restoreFile(id, spaceId) {
            trashBinService.restoreFile(id, function (data) {
                deleteFileFromScope(id, spaceId);
                toastr.success(
                      'File was successfully restored!', 'Trash bin',
                      {
                          closeButton: true, timeOut: 5000
                      });
                console.log('==> file restored');
            });
        }

        function restoreFolder(id, spaceId) {
            trashBinService.restoreFolder(id, function (data) {
                deleteFolderFromScope(id, spaceId);
                toastr.success(
                      'Folder was successfully restored!', 'Trash bin',
                      {
                          closeButton: true, timeOut: 5000
                      });
                console.log('==> folder restored');
            });
        }

        function deleteFilePermanently(id, spaceId) {
            swal({
                title: "Deleting file!",
                text: "Are you sure that you want to delete file?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            }, function () {
                trashBinService.deleteFilePermanently(id, function (data) {
                    deleteFileFromScope(id, spaceId);
                    console.log('==> file DELETED');
                });
                swal({
                    title: "Deleted!",
                    text: "File deleted successfully",
                    timer: 2000,
                    showConfirmButton: false,
                    type: "success"
                });
            });
        }

        function deleteFolderPermanently(id, spaceId) {
            swal({
                title: "Deleting folder!",
                text: "Are you sure you want to delete the folder and its contents?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            }, function () {
                trashBinService.deleteFolderPermanently(id, function (data) {
                    deleteFolderFromScope(id, spaceId);
                    console.log('==> folder DELETED');
                });
                swal({
                    title: "Deleted!",
                    text: "Folder deleted successfully",
                    timer: 2000,
                    showConfirmButton: false,
                    type: "success"
                });
            });
        }

        vm.fileMenuOptions = [
            ['Restore', function ($itemScope) {
                    vm.restoreFile($itemScope.file.id, $itemScope.file.spaceId);
                }
            ],
            ['Delete permanently', function ($itemScope) {
                    vm.deleteFilePermanently($itemScope.file.id, $itemScope.file.spaceId);
                }
            ]
        ];

        vm.folderMenuOptions = [
           ['Restore', function ($itemScope) {
               vm.restoreFolder($itemScope.folder.id, $itemScope.folder.spaceId);
           }
           ],
           ['Delete permanently', function ($itemScope) {
               vm.deleteFolderPermanently($itemScope.folder.id, $itemScope.folder.spaceId);
           }
           ]
        ];

        vm.spaceMenuOptions = [
           [
               'Restore all items for this space', function ($itemScope) {
               }
           ],
           [
               'Clear all items for this space', function ($itemScope) {
                   return deleteFile($itemScope.file.id);
               }
           ]
        ];

        function search() {
            trashBinService.searchData(vm.searchText, function (data) {
                vm.spaces = data;
            });
            vm.searchText = '';
        }

        function cancelSearch() {
            if (vm.searchText.length >= 1) {
            vm.searchText = '';
                getTrashBinContent();
        }
        }

        function changeView(view) {
            if (view === "fa fa-th") {
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
            vm.columnForOrder = trashBinService.orderByColumn(column, vm.columnForOrder);
        }

        function chooseIcon(type) {
            vm.iconSrc = fileService.chooseIcon(type);
            return vm.iconSrc;
        }

        function deleteFileFromScope(itemId, spaceId) {
            var s = vm.spaces;
            for (var i = 0; i < s.length; i++) {
                if (s[i].spaceId === spaceId) {
                    var f = s[i].files;
                    for (var j = 0; j < f.length; j++) {
                        if (f[j].id === itemId){
                            vm.spaces[i].files.splice(j, 1);
                        }
                    }
                }
            }
        }

        function deleteFolderFromScope(itemId, spaceId) {
            var s = vm.spaces;
            for (var i = 0; i < s.length; i++) {
                if (s[i].spaceId === spaceId) {
                    var f = s[i].folders;
                    for (var j = 0; j < f.length; j++) {
                        if (f[j].id === itemId) {
                            vm.spaces[i].folders.splice(j, 1);
                        }
                    }
                }
            }
        }
    }
}());