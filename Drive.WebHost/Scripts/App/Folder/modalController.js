(function() {
    "use strict";

    angular
        .module("driveApp")
        .controller("ModalInstanceCtrl", ModalInstanceCtrl);

    ModalInstanceCtrl.$inject = ['FolderService', '$uibModalInstance', '$rootScope', 'items'];

    function ModalInstanceCtrl(folderService, $uibModalInstance, $rootScope, items) {
        var vm = this;
        vm.save = save;
        vm.cancel = cancel;
        vm.submitted = false;
        vm.folder = {};

        //vm.title = 'Update Folder';

        activate();

        function activate() {
            vm.folder = items;
            //if (vm.folder == undefined) {
            //    vm.title = 'Create Folder';
            //}
        }

        function save() {
            if (vm.folder.id == undefined) {
                $rootScope.$emit("Create", { folder: vm.folder });
            }
            else {
                $rootScope.$emit("Update", { folder: vm.folder });
            }
            $uibModalInstance.close();
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());