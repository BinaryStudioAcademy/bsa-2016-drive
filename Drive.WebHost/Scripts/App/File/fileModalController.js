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

        vm.title = 'Edit';
        vm.icon = "";

        activate();

        function activate() {
            vm.file = items;
            console.log(vm.file);

            switch(vm.file.fileType) {
                case 0:
                    vm.icon = "fa fa-file-o"; // Simple file
                    vm.title = 'Create file';
                    break;
                case 1:
                    vm.icon = "fa fa-file-word-o"; // Docs
                    vm.title = 'Add document';
                    break;
                case 2:
                    vm.icon = "fa fa-file-excel-o"; // Sheets
                    vm.title = 'Add sheets';
                    break;
                case 3:
                    vm.icon = "fa fa-file-powerpoint-o"; // Slides
                    vm.title = 'Add slides';
                    break;
                case 4:
                    vm.icon = "fa fa-trello"; // Trello
                    vm.title = 'Add Trello';
                    break;
                case 5:
                    vm.icon = "fa fa-link"; // Link
                    vm.title = 'Add link';
                    break;
                case 6:
                    vm.icon = "fa fa-upload"; // Upload file
                    vm.title = 'Upload file';
                    break;
                default:
                    vm.icon = "fa fa-file"; // Simple file
            }

            if (typeof vm.file.id == "number") {
                vm.title = 'Edit';
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