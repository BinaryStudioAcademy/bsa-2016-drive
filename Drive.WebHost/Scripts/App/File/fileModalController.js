(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("FileModalCtrl", FileModalCtrl);

    FileModalCtrl.$inject = ['FileService', '$uibModalInstance', 'items'];

    function FileModalCtrl(fileService, $uibModalInstance, items) {
        var vm = this;

        vm.save = save;
        vm.cancel = cancel;
        vm.isValidUrl = isValidUrl;
        vm.checkUrl = checkUrl;

        vm.inputFile = null;
        vm.upload = upload;
        vm.remove = remove;

        activate();

        function activate() {
            vm.file = items;
            vm.title = 'New link';
            vm.icon = "./Content/Icons/add-file_bw.svg";
            vm.urlIsValid = false;
            vm.submitted = false;
            vm.types = {
                1: ["docs.google.com/document/"],
                2: ["docs.google.com/spreadsheets/"],
                3: ["docs.google.com/presentation/"],
                4: ["trello.com"],
                5: ["."] // link
            };
            vm.buffer = {
                link : null,
                fileExtantion: null
            }
            vm.linkDisabled = false;

            if (vm.file.parentId === 0) vm.file.parentId = null;

            if (vm.file.name) {
                vm.title = 'Edit';
                vm.urlIsValid = true;
                vm.name = items.name;
                vm.description = items.description;
                vm.link = items.link;
                if (vm.file.fileType == 6) {
                    vm.buffer.link = vm.file.link;
                    //vm.link = '';
                    vm.file.link = '';
                    vm.linkDisabled = true;
                    vm.buffer.fileExtantion = getFileExtantion();
                    vm.file.name = getFileName();
                    vm.linkDisabled = true;
                }
            }

        }

        function save() {
            vm.submitted = true;
            if (vm.file.name !== undefined) {
                if (vm.file.id === undefined) {
                    vm.checkUrl();
                    fileService.createFile(vm.file,
                        function (response) {
                            if (response) {
                                var data = {
                                    operation: 'create',
                                    item: response
                                }
                                $uibModalInstance.close(data);
                            }
                        });
                } else {
                    if (vm.file.fileType == 6) {
                        var fullname = vm.file.name + vm.buffer.fileExtantion
                        vm.file.name = fullname;
                        vm.file.link = vm.buffer.link;
                    }
                    fileService.updateFile(vm.file.id, vm.file,
                        function (response) {
                            if (response) {
                                var data = {
                                    operation: 'update',
                                    item: response
                                }
                                $uibModalInstance.close(data);
                            }
                        });
                }
            }
        }

        function cancel() {
            items.name = vm.name;
            items.description = vm.description;
            items.link = vm.link;

            $uibModalInstance.dismiss('cancel');
        };

        function checkUrl() {
            vm.file.fileType = null;
            var reg = new RegExp("^https?://");

            if (!reg.test(vm.file.link)) {
                vm.file.link = "http://" + vm.file.link;
            }

            for (var t in vm.types) {
                for (var i = 0; i < vm.types[t].length; i++) {
                    if (vm.file.link.includes(vm.types[t][i])) {
                        vm.file.fileType = Number(t);
                        break;
                    }
                }
                if (vm.file.fileType) {
                    break;
                }
            }

        }

        function isValidUrl() {
            var expression = "^(?:(?:ht|f)tps?://)?(?:[\\-\\w]+@)?(?:[\\-0-9a-z]*[0-9a-z]\\.)+[a-z]{2,6}(?::\\d{1,5})?(?:[?/\\\\#][?!^$.(){}:|=[\\]+\\-/\\\\*;&~#@,%\\wР-пр-џ]*)?$";
            var reg = new RegExp(expression);
            vm.urlIsValid = reg.test(vm.file.link);

            if (vm.urlIsValid) {
                vm.checkUrl();
                vm.icon = fileService.chooseIcon(vm.file.fileType)
            }
            else {
                vm.icon = "./Content/Icons/add-file_bw.svg";
            }
        }

        function upload() {
            fileService.uploadFile(vm.file.spaceId,
                    vm.file.parentId,
                    vm.inputFile.file,
                    function (response) {
                        if (response)
                            $uibModalInstance.close(response);
                    });
        }
        function remove() {
            vm.inputFile = null;
        }

        function getFileExtantion() {
            var fullFileName = vm.file.name;
            var pointIndex = fullFileName.lastIndexOf(".");
            var fileExtantion = fullFileName.slice(pointIndex);
            return fileExtantion;
        }
        function getFileName() {
            var fullFileName = vm.file.name;
            var pointIndex = fullFileName.lastIndexOf(".");
            var fileName = fullFileName.slice(0, pointIndex);
            return fileName;
        }
    }
}());