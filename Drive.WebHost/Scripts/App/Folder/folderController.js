(function() {
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
        vm.menuOptions = [
            [
                'Edit', function($itemScope) {

                }
            ],
            null,
            [
                'Delete', function($itemScope) {
                }
            ],
            null
        ];
        vm.otherMenuOptions = [
            [
                'Create', function($itemScope) {

                }
            ]
        ];

        activate();

        function activate() {
            return getAll();
        }

        function get(id) {
            folderService.get(id,
                function(folder) {
                    vm.folder = folder;

                });
        }

        function getAll() {
            folderService.getAll(function(folders) {
                vm.folders = folders;
            });
        }

        function create() {
            folderService.create(vm.folder,
                function(id) {
                    vm.folder.id = id;
                });
        }

        function updateFolder(id, folder) {
            folderService.updateFolder(id, folder);
        }

        function deleteFolder(id) {
            folderService.deleteFolder(id,
                function() {

                    folderService.getAll(function(folders) {
                        vm.folders = folders;
                    });
                });
        }
    }
}());