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

        vm.inputFile = [];
        vm.upload = upload;
        vm.removeAll = removeAll;
        vm.removeItem = removeItem;
        vm.disableElement = disableElement;
        vm.getFileName = getFileName;
        vm.getFileExtension = getFileExtension;
        vm.checkModel = checkModel;
        vm.editMode = editMode;

        activate();

        function activate() {
            vm.modelIsValid = false;
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
                //5: ["."], // link
                8: [".jpg", ".jpeg", ".png", ".bmp"],
                10: ["youtube.com", "vimeo.com", "dailymotion.com"]
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

            if (!vm.file.fileType) {
                vm.file.fileType = 5; // link
            }
        }

        function isValidUrl() {
            var expression = "^(?:(?:ht|f)tps?://)?(?:[\\-\\w]+@)?(?:[\\-0-9a-z]*[0-9a-z]\\.)+[a-z]{2,6}(?::\\d{1,5})?(?:[?/\\\\#][?!^$.(){}:|=[\\]+\\-/\\\\*;&~#@,%\\wР-пр-џ]*)?$";
            var reg = new RegExp(expression);
            vm.urlIsValid = reg.test(vm.file.link);

            if (vm.urlIsValid) {
                vm.checkUrl();
                vm.icon = fileService.chooseIcon(vm.file.fileType);
            }
            else {
                vm.icon = "./Content/Icons/add-file_bw.svg";
            }
        }

        function upload() {
            var valid = fileService.checkFilesValidationProperty(vm.inputFile);
            if (valid) {
                vm.modelIsValid = true;
                var data = [];
                for (var i = 0; i < vm.inputFile.length; i++) {
                    var temp = {};
                    temp.name = vm.inputFile[i].filename;
                    temp.extension = vm.inputFile[i].extension;
                    temp.description = vm.inputFile[i].description;
                    data.push(temp);
                }
                fileService.uploadFile(vm.file.spaceId, vm.file.parentId, vm.inputFile, data, function (response) {
                    if (response)
                        $uibModalInstance.close(response);
                });
            }
            else {
                vm.modelIsValid = false;
            }
        }
        function removeAll() {
            vm.inputFile.length = 0;
        }
        function removeItem(index) {
            vm.inputFile.splice(index, 1);
            vm.checkModel();
        }

        function disableElement() {
            if (vm.inputFile.length == 0)
                return true;
        }
        function getFileName(fileName) {
            var name = fileName.substr(0, fileName.lastIndexOf('.'));
            return name;
        }
        function getFileExtension(fileName) {
            var extension = fileName.substr(fileName.lastIndexOf('.'));
            return extension;
        }
        function checkModel() {
            var valid = fileService.checkFilesValidationProperty(vm.inputFile);
            if (valid) {
                vm.modelIsValid = true;
            }
            else vm.modelIsValid = false;
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
        function editMode(mode, index, name, ext) {
            if (mode) {
                vm.inputFile[index].mode = false;
                vm.inputFile[index].fname = name + ext;
            }
            else {
                vm.inputFile[index].mode = true;
            }
        }
    }
}());