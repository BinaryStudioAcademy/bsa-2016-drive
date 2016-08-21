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
        vm.folders = [];

        vm.title = 'Edit';

        activate();

        function activate() {
            vm.folder = items;
            if (vm.folder.id == undefined) {
                vm.title = 'Create Folder';
            } else {
                vm.name = items.name;
            }
            if (vm.folder.parentId === 0) vm.folder.parentId = null;

            folderService.getAllByParentId(vm.folder.spaceId, vm.folder.parentId, function (data) {
                vm.folders = data;
                console.log(data);
            });

            vm.bool = false;
        }

        function save() {
            vm.bool = false;
            vm.submitted = true;
            if (vm.folder.name !== undefined) {
                for (var i = 0; i < vm.folders.length; i++) {
                    if (vm.folders[i].name === vm.folder.name) {
                        vm.bool = true;                        
                    }
                }
                if (!vm.bool) {
                    if (vm.folder.id === undefined) {
                        folderService.create(vm.folder,
                            function (response) {
                                if (response)
                                    $uibModalInstance.close(response);
                            });

                    } else {
                        folderService.updateFolder(vm.folder,
                            function (response) {
                                if (response)
                                    $uibModalInstance.close(response);
                            });
                    }
                }
                if (vm.name === vm.folder.name) {
                    folderService.updateFolder(vm.folder,
                            function (response) {
                                if (response)
                                    $uibModalInstance.close(response);
                            });
                }
            }
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());