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
            $uibModalInstance.close(vm.folder);
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());