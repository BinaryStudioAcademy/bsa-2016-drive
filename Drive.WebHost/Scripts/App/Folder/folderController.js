(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("FolderController", FolderController);

    FolderController.$inject = ['FolderService'];

    function FolderController(folderService) {
        var vm = this;
        vm.folders = [];
        vm.updateFolder = updateFolder;
        vm.deleteFolder = deleteFolder;

        activate();

        function activate() {
            folderService.getFolders(function(folders) {
                vm.folders = folders;
            });
        }

        function updateFolder(id, folder) {
            folderService.updateFolder(id, folder, function() {});
        }

        function deleteFolder(id) {
            folderService.deleteFolder(id, function () { });

            folderService.getFolders(function(folders) {
                vm.folders = folders;
            });
        }
    }
}());