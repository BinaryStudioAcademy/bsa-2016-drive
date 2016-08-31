(function () {
    "use strict";

    angular.module("driveApp")
        .controller("TrashBinController", TrashBinController);

    TrashBinController.$inject = ['SpaceService', 'FileService', '$routeParams'];

    function TrashBinController(spaceService, fileService, $routeParams) {
        var vm = this;

        vm.changeView = changeView;
        vm.chooseIcon = chooseIcon;
        vm.orderByColumn = orderByColumn;

        vm.search = search;
        //vm.cancelSearch = cancelSearch;
        vm.getTrashBinContent = getTrashBinContent;
        //vm.deleteFilePermanently = deleteFilePermanently;

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
            spaceService.getTrashBinContent(function (data) {
                vm.spaces = data;
                console.log(data);
            });
 
        }

        vm.fileMenuOptions = [
            [
                'Restore', function ($itemScope) {

                }
            ],
            [
                'Delete permanently', function ($itemScope) {
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
    }
}());