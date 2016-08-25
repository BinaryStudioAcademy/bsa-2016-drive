(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SharedFileModalCtrl", SharedFileModalCtrl);

    SharedFileModalCtrl.$inject = ['SharedSpaceService', '$uibModalInstance', 'items'];

    function SharedFileModalCtrl(sharedSpaceService, $uibModalInstance, items) {
        var vm = this;

        vm.save = save;
        vm.cancel = cancel;
        //vm.isValidUrl = isValidUrl;
        //vm.checkUrl = checkUrl;
        vm.user = null;

        activate();

        function activate() {
            vm.file = items;
            getAllUsers();
            //vm.title = 'New link';
            //vm.icon = "fa fa-file-o";
            //vm.urlIsValid = false;
            //vm.submitted = false;
            //vm.types = {
            //    1: ["docs.google.com/document/"],
            //    2: ["docs.google.com/spreadsheets/"],
            //    3: ["docs.google.com/presentation/"],
            //    4: ["trello.com"],
            //    5: ["."] // link
            //};

            //if (vm.file.parentId === 0) vm.file.parentId = null;

            //if (vm.file.name) {
            //    vm.title = 'Edit';
            //    vm.urlIsValid = true;
            //    vm.name = items.name;
            //    vm.description = items.description;
            //    vm.link = items.link;
            //}
        }

        function save() {
            vm.submitted = true;
            vm.checkUrl();
            if (vm.file.name !== undefined) {
                if (vm.file.id === undefined) {
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

        function getAllUsers() {
            sharedSpaceService.getAllUsers(function (data) {
                vm.user = data.files;
            });
        }


        //function checkUrl() {
        //    vm.file.fileType = null;
        //    var reg = new RegExp("^https?://");

        //    if (!reg.test(vm.file.link)) {
        //        vm.file.link = "http://" + vm.file.link;
        //    }

        //    for (var t in vm.types) {
        //        for (var i = 0; i < vm.types[t].length; i++) {
        //            if (vm.file.link.includes(vm.types[t][i])) {
        //                vm.file.fileType = Number(t);
        //                break;
        //            }
        //        }
        //        if (vm.file.fileType) {
        //            break;
        //        }
        //    }

        //}

        //function isValidUrl() {
        //    var expression = "^(?:(?:ht|f)tps?://)?(?:[\\-\\w]+@)?(?:[\\-0-9a-z]*[0-9a-z]\\.)+[a-z]{2,6}(?::\\d{1,5})?(?:[?/\\\\#][?!^$.(){}:|=[\\]+\\-/\\\\*;&~#@,%\\wР-пр-џ]*)?$";
        //    var reg = new RegExp(expression);
        //    vm.urlIsValid = reg.test(vm.file.link);
        //}
    }
}());