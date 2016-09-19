(function () {
    "use strict";

    angular.module("driveApp")
        .controller("TrashBinController", TrashBinController);

    TrashBinController.$inject = ['TrashBinService', 'FileService', '$routeParams', 'toastr', 'localStorageService'];

    function TrashBinController(trashBinService, fileService, $routeParams, toastr, localStorageService) {
        var vm = this;

        vm.changeView = changeView;
        vm.chooseIcon = chooseIcon;
        vm.orderByColumn = orderByColumn;

        vm.getTrashBinContent = getTrashBinContent;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.deleteFilePermanently = deleteFilePermanently;
        vm.deleteFolderPermanently = deleteFolderPermanently;
        vm.restoreFile = restoreFile;
        vm.restoreFolder = restoreFolder;
        vm.deleteFileFromScope = deleteFileFromScope;
        vm.deleteFolderFromScope = deleteFolderFromScope;
        vm.restoreSpace = restoreSpace;
        vm.restoreTrashBin = restoreTrashBin;
        vm.clearSpace = clearSpace;
        vm.clearTrashBin = clearTrashBin;
        

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
            vm.columnForOrder = 'name';
            vm.searchText = '';
            vm.iconHeight = 30;

            vm.spaces = [];

            getTrashBinContent();
        }

        function getTrashBinContent() {
            trashBinService.getTrashBinContent(function (data) {
                vm.spaces = data;
            });
        }

        function search() {
            trashBinService.searchTrashBin(vm.searchText, function (data) {
                vm.spaces = data;
                console.log(data);
            });
        }

        function cancelSearch() {
            if (vm.searchText.length >= 1) {
                vm.searchText = '';
                getTrashBinContent();
            }
        }

        function restoreFile(id, spaceId) {
            trashBinService.restoreFile(id, function () {
                deleteFileFromScope(id, spaceId);
                toastr.success(
                      'File was successfully restored!', 'Trash bin',
                      {
                          closeButton: true, timeOut: 5000
                      });
            });
        }

        function restoreFolder(id, spaceId) {
            trashBinService.restoreFolder(id, function () {
                deleteFolderFromScope(id, spaceId);
                toastr.success(
                      'Folder was successfully restored!', 'Trash bin',
                      {
                          closeButton: true, timeOut: 5000
                      });
            });
        }

        function restoreSpace(space, index) {
            var spaces = [space.spaceId];
            trashBinService.restoreSpaces(spaces, function () {
                vm.spaces.splice(index, 1);
                toastr.success(
                      'All content of '+ space.name +' successfully restored!', 'Trash bin',
                      {
                          closeButton: true, timeOut: 5000
                      });
            });
        }

        function restoreTrashBin() {
            var spaces = [];
            for (var i = 0; i < vm.spaces.length; i++) {
                spaces.push(vm.spaces[i].spaceId);
            }
            trashBinService.restoreSpaces(spaces, function () {
                vm.spaces = [];
                toastr.success(
                      'All contents of the trash successfully restored', 'Trash bin',
                      {
                          closeButton: true, timeOut: 5000
                      });
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
                trashBinService.deleteFilePermanently(id, function () {
                    deleteFileFromScope(id, spaceId);
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
                trashBinService.deleteFolderPermanently(id, function () {
                    deleteFolderFromScope(id, spaceId);
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

        function clearSpace(space, index) {
            swal({
                title: "Deleting " + space.name + " content from the trash bin",
                text: "Are you sure you want to delete " + space.name + " content permanently?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            }, function () {
                trashBinService.clearSpace(space.spaceId, function () {
                    vm.spaces.splice(index, 1);
                });
                swal({
                    title: "Deleted!",
                    text: space.name + " content successfully deleted from the trash bin",
                    timer: 2000,
                    showConfirmButton: false,
                    type: "success"
                });
            });
        }

        function clearTrashBin() {
            swal({
                title: "Deleting all content from the trash bin",
                text: "Are you sure you want to delete ALL permanently?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            }, function () {
                trashBinService.clearTrashBin(function () {
                    vm.spaces = [];
                });
                swal({
                    title: "Deleted!",
                    text: "Trash bin successfully cleared",
                    timer: 2000,
                    showConfirmButton: false,
                    type: "success"
                });
            });
        }

        vm.fileMenuOptions = [
            ['Restore', function ($itemScope) {vm.restoreFile($itemScope.file.id, $itemScope.file.spaceId);}],
            ['Delete permanently', function ($itemScope) { vm.deleteFilePermanently($itemScope.file.id, $itemScope.file.spaceId);}]
        ];

        vm.folderMenuOptions = [
           ['Restore', function ($itemScope) {vm.restoreFolder($itemScope.folder.id, $itemScope.folder.spaceId);}],
           ['Delete permanently', function ($itemScope) {vm.deleteFolderPermanently($itemScope.folder.id, $itemScope.folder.spaceId);} ]
        ];

        vm.spaceMenuOptions = [
           ['Restore all for this space', function ($itemScope) {vm.restoreSpace($itemScope.space, $itemScope.$index);}],
           ['Clear all for this space', function ($itemScope) {vm.clearSpace($itemScope.space, $itemScope.$index);}]
        ];

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
                            break;
                        }
                    }
                    deleteSpaceFromScope(i);
                    break;
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
                            break;
                        }
                    }
                    deleteSpaceFromScope(i);
                    break;
                }
            }
        }

        function deleteSpaceFromScope(index)
        {
            var s = vm.spaces;
            if (!(s[index].folders.length) && !(s[index].files.length)) {
                vm.spaces.splice(index, 1);
            }
        }
    }
}());