(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("FolderModalCtrl", FolderModalCtrl);

    FolderModalCtrl.$inject = ['FolderService', '$uibModalInstance', 'items'];

    function FolderModalCtrl(folderService, $uibModalInstance, items) {
        var vm = this;
        vm.save = save;
        vm.cancel = cancel;
        vm.submitted = false;
        vm.folder = {};

        vm.title = 'Edit';

        activate();

        function activate() {
            vm.folder = items;
            if (vm.folder.id == undefined) {
                vm.title = 'New folder';
            } else {
                vm.name = items.name;
                vm.description = items.description;
            }
            if (vm.folder.parentId === 0) vm.folder.parentId = null;
        }

        function save() {
            vm.submitted = true;
            if (vm.folder.name !== undefined) {
                if (vm.folder.id === undefined) {
                    folderService.create(vm.folder,
                        function (response) {
                            if (response) {
                                var data = {operation: 'create',
                                        item : response}
                                $uibModalInstance.close(data);
                            }
                        });

                } else {
                    folderService.updateFolder(vm.folder,
                        function (response) {
                            if (response) {
                                var data = {
                                    operation: 'update',
                                    item: response}
                                $uibModalInstance.close(data);
                            }
                        });
                }
            }
        }

        function cancel() {
            items.name = vm.name;
            items.description = vm.description;
            $uibModalInstance.dismiss('cancel');
        };
    }
}());