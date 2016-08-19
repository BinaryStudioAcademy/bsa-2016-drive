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
        vm.linkTemplate = "";
        vm.isValidUrl = isValidUrl;
        vm.checkUrl = checkUrl;

        activate();

        function activate() {
            vm.file = items;
            vm.urlIsValid = false;
            console.log(vm.file);

            switch(vm.file.fileType) {
                case 0:
                    vm.icon = "fa fa-file-o"; // Simple file
                    vm.title = 'Create file';
                    break;
                case 1:
                    vm.icon = "fa fa-file-word-o"; // Docs
                    vm.title = 'Add document';
                    vm.linkTemplate = 'docs.google.com/document/';
                    break;
                case 2:
                    vm.icon = "fa fa-file-excel-o"; // Sheets
                    vm.title = 'Add sheets';
                    vm.linkTemplate = 'docs.google.com/spreadsheets/';
                    break;
                case 3:
                    vm.icon = "fa fa-file-powerpoint-o"; // Slides
                    vm.title = 'Add slides';
                    vm.linkTemplate = 'docs.google.com/presentation/';
                    break;
                case 4:
                    vm.icon = "fa fa-trello"; // Trello
                    vm.title = 'Add Trello';
                    vm.linkTemplate = 'trello.com';
                    break;
                case 5:
                    vm.icon = "fa fa-link"; // Link
                    vm.title = 'Add link';
                    vm.linkTemplate = '.';
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
                vm.urlIsValid = true;
            }

        }

        function save() {
            vm.submitted = true;
            vm.checkUrl();
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

        function checkUrl() {
            var reg = new RegExp("^https?://");
            if (!reg.test(vm.file.link)) {
                vm.file.link = "http://" + vm.file.link;
            }
        }

        function isValidUrl() {
            //var expression = "^(?:(?:ht|f)tps?://)?(?:[\\-\\w]+:[\\-\\w]+@)?(?:[0-9a-z][\\-0-9a-z]*[0-9a-z]\\.)+[a-z]{2,6}(?::\\d{1,5})?(?:[?/\\\\#][?!^$.(){}:|=[\\]+\\-/\\\\*;&~#@,%\\wА-Яа-я]*)?$";
            var expression = /[-a-zA-Z0-9@:%_\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?/gi;
            var reg = new RegExp(expression);
            vm.urlIsValid = reg.test(vm.file.link) && vm.file.link.includes(vm.linkTemplate);
        }
    }
}());