(function() {
    "use strict";

    angular
        .module("driveApp")
        .controller("ModalInstanceCtrl", ModalInstanceCtrl);

    ModalInstanceCtrl.$inject = ['FolderService', '$uibModalInstance', 'items'];

    function ModalInstanceCtrl(folderService, $uibModalInstance, items) {
        var vm = this;
        vm.save = save;
        vm.cancel = cancel;
        vm.submitted = false;
        vm.folder = {};

        activate();

        function activate() {
            vm.folder = items;
        }

        function save() {
            vm.submitted = true;
            if (vm.folder.name !== undefined) {
                if (vm.folder.id === undefined) {
                    folderService.create(vm.folder,
                        function (id) {
                            if (id > 0)
                                $uibModalInstance.close(id);
                        });
                } else {
                    folderService.updateFolder(vm.folder.id,
                        vm.folder,
                        function (callback) {
                            if (callback)
                                $uibModalInstance.close(vm.folder.id);
                        });
                }
            }
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());