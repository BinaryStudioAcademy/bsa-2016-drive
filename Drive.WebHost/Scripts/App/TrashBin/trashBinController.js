(function () {
    "use strict";

    angular.module("driveApp")
        .controller("TrashBinController", TrashBinController);

    TrashBinController.$inject = ['TrashBinService', 'FileService', '$routeParams'];

    function TrashBinController(trashBinService, fileService, $routeParams) {
        var vm = this;

        vm.changeView = changeView;
        vm.chooseIcon = chooseIcon;
        vm.orderByColumn = orderByColumn;

        vm.search = search;
        //vm.cancelSearch = cancelSearch;
        vm.getTrashBinContent = getTrashBinContent;
        vm.deleteFilePermanently = deleteFilePermanently;
        vm.restoreFile = restoreFile;
        vm.deleteFileFromScope = deleteFileFromScope;

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
                vm.deleteFileFromScope(id, spaceId);
                console.log('==> file restored');
            });
        }

        function deleteFilePermanently(id, spaceId) {
            trashBinService.deleteFilePermanently(id, function (data) {
                deleteFileFromScope(id, spaceId);
                console.log('==> file DELETED');
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

        //function deleteFilePermanently(id) {
        //    fileService.deleteFilePermanently(id, function () {
        //        getFiles();
        //    });
        //}

        function search() {
            fileService.searchFiles(vm.filesType, vm.searchText, function (data) {
                vm.spaces = data;
            });
            vm.searchText = '';
        }

        /*
        function cancelSearch() {         
            getFiles();
            vm.searchText = '';
        }
        */

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
            vm.columnForOrder = fileService.orderByColumn(column, vm.columnForOrder);
        }

        function chooseIcon(type) {
            vm.iconSrc = fileService.chooseIcon(type);
            return vm.iconSrc;
        }

        function deleteFileFromScope(itemId, spaceId) {
            var s = vm.spaces;
            for (var i = 0; i < s.length; i++) {
                if (s[i].spaceId == spaceId) {
                    var f = s[i].files;
                    for (var j = 0; j < f.length; j++) {
                        if (f[j].id == itemId){
                            vm.spaces[i].files.splice(j, 1);
                        }
                    }
                }
            }
        }
    }
}());