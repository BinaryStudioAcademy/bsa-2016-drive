(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("FolderController", FolderController);

    FolderController.$inject = ['FolderService'];

    function FolderController(folderService) {
        var vm = this;
        vm.folder = {
            id: 0,
            isDeleted: false,
            name: '',
            description: ''
        };

        vm.folders = [];
        vm.getAll = getAll;
        vm.get = get;
        vm.create = create;
        vm.updateFolder = updateFolder;
        vm.deleteFolder = deleteFolder;

        activate();

        function activate() {
            return getAll();
        }

        function get(id) {
            return folderService.get(id, function (folder) {
                vm.folder = folder;
                return vm.folder;
            });
        }

        function getAll() {
            return folderService.getAll(function (folders) {
                vm.folders = folders;
                return vm.folders;
            });
        }

        function create() {
            return folderService.create(vm.folder, function (id) {
                vm.folder.id = id;
                return vm.folder.id;
            });
        }

        function updateFolder(id, folder) {
            folderService.updateFolder(id, folder, function() {});
        }

        function deleteFolder(id) {
            folderService.deleteFolder(id, function () { });

            folderService.getAll(function(folders) {
                vm.folders = folders;
            });
        }
    }
}());