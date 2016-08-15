(function() {
    "use strict";

    angular
        .module("driveApp")
        .controller("FileModalCtrl", FileModalCtrl);

    FileModalCtrl.$inject = ['FileService', '$uibModalInstance', 'items'];

    function FileModalCtrl(fileService, $uibModalInstance, items) {
        var vm = this;
        vm.save = save;
        vm.cancel = cancel;
        vm.submitted = false;
        vm.file = {};

        vm.title = 'Update File';

        activate();

        function activate() {
            vm.file = items;
            console.log(vm.file);
            if (vm.file.id == undefined) {
                vm.title = 'Create File';
            }
        }

        function save() {
            vm.submitted = true;
            if (vm.file.name !== undefined) {
                if (vm.file.id === undefined) {
                    fileService.createFile(vm.file,
                        function (response) {
                            if (response)
                                $uibModalInstance.close(response);
                        });
                } else {
                    fileService.updateFile(vm.file.id, vm.file,
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