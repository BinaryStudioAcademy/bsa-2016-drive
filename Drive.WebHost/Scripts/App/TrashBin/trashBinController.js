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
        //vm.cancelSearch = cancelSearch;
        vm.getTrashBinContent = getTrashBinContent;
        vm.deleteFilePermanently = deleteFilePermanently;
        vm.restoreFile = restoreFile;

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

        function restoreFile(id) {
            trashBinService.restoreFile(id, function (data) {
                toastr.success(
                      'File was successfully restored!', 'Trash bin',
                      {
                          closeButton: true, timeOut: 5000
                      });
                console.log('==> file restored');
            });
        }

        function deleteFilePermanently(id) {
            trashBinService.deleteFilePermanently(id, function (data) {
                toastr.success(
                      'File was deleted!', 'Trash bin',
                      {
                          closeButton: true, timeOut: 5000
                      });
                console.log('==> file DELETED');
            });
        }

        vm.fileMenuOptions = [
            ['Restore', function ($itemScope) {
                    vm.restoreFile($itemScope.file.id);
                }
            ],
            ['Delete permanently', function ($itemScope) {
                    vm.deleteFilePermanently($itemScope.file.id);
                }
            ]
        ];

        vm.folderMenuOptions = [
           ['Restore', function ($itemScope) {
               vm.restoreFile($itemScope.file.id);
           }
           ],
           ['Delete permanently', function ($itemScope) {
               vm.deleteFilePermanently($itemScope.file.id);
           }
           ]
        ];

        vm.spaceMenuOptions = [
           [
               'Restore all items', function ($itemScope) {
               }
           ],
           [
               'Empty Trash Bin', function ($itemScope) {
                   return deleteFile($itemScope.file.id);
               }
           ]
        ];

        //function deleteFilePermanently(id) {
        //    fileService.deleteFilePermanently(id, function () {
        //        getFiles();
        //    });
        //}

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
            vm.columnForOrder = trashBinService.orderByColumn(column, vm.columnForOrder);
        }

        function chooseIcon(type) {
            vm.iconSrc = fileService.chooseIcon(type);
            return vm.iconSrc;
        }
    }
}());