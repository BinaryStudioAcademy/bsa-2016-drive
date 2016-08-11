(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("ModalInstanceCtrl", ModalInstanceCtrl);

    ModalInstanceCtrl.$inject = ['FolderService', '$uibModalInstance'];

    function ModalInstanceCtrl(folderService, $uibModalInstance) {
        var vm = this;
        vm.folder = {
            id: 0,
            isDeleted: false,
            name: '',
            description: ''
        };

        activate();

        function activate() {
            vm.folder = folderService.getfolder();
        }

        vm.save = save;
        vm.cancel = cancel;

        function save() {
            if (vm.folder.id == 0) {
                folderService.create(vm.folder, function (id) {
                    vm.folder.id = id;
                });
            }
            else {
                folderService.updateFolder(vm.folder.id, vm.folder);
            }
            $uibModalInstance.close(vm.folder.id);
            vm.folder = {};
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());