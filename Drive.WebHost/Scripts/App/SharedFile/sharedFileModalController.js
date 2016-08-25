(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SharedFileModalCtrl", SharedFileModalCtrl);

    SharedFileModalCtrl.$inject = ['SharedSpaceService', '$uibModalInstance', 'items'];

    function SharedFileModalCtrl(sharedSpaceService, $uibModalInstance, items) {
        var vm = this;
        vm.title = 'Shared File';

        vm.save = save;
        vm.cancel = cancel;
        //vm.isValidUrl = isValidUrl;
        //vm.checkUrl = checkUrl;
        vm.user = null;
        vm.usersWithPermissions = null;
        vm.addUserPermissons = addUserPermissons;

        activate();

        function activate() {
            vm.fileId = items;

            getAllUsers();
            getUsersWithPermissions();
            //updateUsersPermissions();
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
            $uibModalInstance.close();
            updateUsersPermissions();
        }

        function cancel() {

            updateUsersPermissions();
            $uibModalInstance.dismiss('cancel');
        };

        function getAllUsers() {
            sharedSpaceService.getAllUsers(function (data) {
                vm.users = data;
            });
        }

        function getUsersWithPermissions() {
            sharedSpaceService.getPermissions(vm.fileId, function (data) {
                vm.usersWithPermissions = data;
            });
        }

        function updateUsersPermissions() {
            sharedSpaceService.createOrUpdatePermission(vm.usersWithPermissions, vm.fileId, function (data) {
                console.log(data);
            });
        }

        function addUserPermissons() {

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