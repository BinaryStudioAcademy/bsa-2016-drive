(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SharedContentModalCtrl", SharedContentModalCtrl);

    SharedContentModalCtrl.$inject = ['SharedSpaceService', '$uibModalInstance', 'items'];

    function SharedContentModalCtrl(sharedSpaceService, $uibModalInstance, items) {
        var vm = this;
        vm.title = null;

        vm.save = save;
        vm.cancel = cancel;
        vm.users = null;
        vm.usersWithPermissions = [];
        vm.addUserPermissons = addUserPermissons;
        vm.removeUsersWithPermissions = removeUsersWithPermissions;
        vm.permissionCanModifyChanged = permissionCanModifyChanged;
        vm.permissionCanReadChanged = permissionCanReadChanged;

        activate();

        function activate() {
            vm.fileId = items.contentId;
            vm.title = items.title;

            getUsersWithPermissions();
            addUsersName();
        }

        function save() {
            updateUsersPermissions();
            $uibModalInstance.close();
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };


        function getUsersWithPermissions() {
            sharedSpaceService.getPermissions(vm.fileId, function (data) {
                sharedSpaceService.getAllUsers(function (usersInfo) {
                    vm.users = usersInfo;
                    vm.usersWithPermissions = data;
                    addUsersName();
                    removeExistingUsers();
                });

            });
        }

        function removeExistingUsers() {
            for (var i = 0; i < vm.usersWithPermissions.length; i++)
                for (var j = 0; j < vm.users.length; j++) {
                    if (vm.usersWithPermissions[i].globalId == vm.users[j].id) {
                        vm.users.splice(j,1);
                        break;
                    }
                }
        }

        function removeUserFromListUsers(userId) {
            for (var j = 0; j < vm.users.length; j++) {
                if (vm.users[j].id == userId) {
                    vm.users.splice(j, 1);
                    break;
                }
            }
        }

        function removeUsersWithPermissions(user) {
            for (var i = 0; i < vm.usersWithPermissions.length; i++) {
                if (vm.usersWithPermissions[i].globalId == user.globalId) {
                    vm.usersWithPermissions[i].isDeleted = true;
                    vm.usersWithPermissions[i].canRead = false;
                    vm.usersWithPermissions[i].canModify = false;
                    vm.users.push({ name: user.name, id: user.globalId });
                    break;
                }
            }
        }

        function updateUsersPermissions() {
            sharedSpaceService.createOrUpdatePermission(vm.usersWithPermissions, vm.fileId, function (data) {
                console.log(data);
            });
        }

        function addUserPermissons() {
            if (vm.selected.id == undefined || vm.selected.id == null) {
                return;
            }
            var user = {
                globalId: vm.selected.id,
                name: vm.selected.name,
                isDeleted: false,
                canRead: false,
                canModify: false
            }
            var existing = -1;
            for (var i = 0; i < vm.usersWithPermissions.length; i++) {
                if (vm.usersWithPermissions[i].globalId == vm.selected.id) {
                    existing = i;
                    break;
                }
            }
            if (existing == -1) {
                if (Array.isArray(vm.usersWithPermissions)) {
                    vm.usersWithPermissions.push(user);
                    removeUserFromListUsers(user.globalId);
                }
                else {
                    vm.usersWithPermissions = [];
                    vm.usersWithPermissions.push(user);
                    removeUserFromListUsers(user.globalId);
                }
            }
            else {
                vm.usersWithPermissions[existing].isDeleted = false;
            }
        }

        function addUsersName() {
            for (var i = 0; i < vm.usersWithPermissions.length; i++)
                for (var j = 0; j < vm.users.length; j++) {
                    if (vm.usersWithPermissions[i].globalId == vm.users[j].id) {
                        vm.usersWithPermissions[i].name = vm.users[j].name;
                        break;
                    }
                }
            
        }

        function permissionCanModifyChanged(userId) {
            for (var i = 0; i < vm.usersWithPermissions.length; i++) {
                if (vm.usersWithPermissions[i].globalId == userId && vm.usersWithPermissions[i].canModify == true) {
                    vm.usersWithPermissions[i].canRead = true;
                    break;
                }
            }
        }

        function permissionCanReadChanged(userId) {
            for (var i = 0; i < vm.usersWithPermissions.length; i++) {
                if (vm.usersWithPermissions[i].globalId == userId && vm.usersWithPermissions[i].canRead == false) {
                    vm.usersWithPermissions[i].canModify = false;
                    break;
                }
            }
        }
}
}());