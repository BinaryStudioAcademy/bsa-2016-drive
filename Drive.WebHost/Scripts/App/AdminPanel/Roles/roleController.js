(function () {
    "use strict";

    angular.module("driveApp")
        .controller("RoleController", RoleController);

    RoleController.$inject = ['$uibModalInstance', 'RoleService', 'items', '$window', 'toastr'];

    function RoleController($uibModalInstance, RoleService, items, $window, toastr) {
        var vm = this;
        vm.createRole = createRole;
        vm.cancel = cancel;
        vm.addRoleUser = addRoleUser;
        vm.removeRoleUser = removeRoleUser;
        vm.save = save;
        vm.tab = 1;
        vm.setTab = setTab;
        vm.isSet = isSet;
        vm.permittedUsers = [];

        activate();

        function activate() {
            vm.title = "Admin Panel";
            vm.name = null;
            vm.description = null;
            if (items !== undefined) {
                RoleService.getById(items, function (data) {
                    vm.role = data;
                    RoleService.getAllUsers(function (data) {
                        vm.users = data;
                        for (var i = 0; i < vm.role.users.length; i++) {
                            for (var j = 0; j < vm.users.length; j++) {
                                if (vm.role.users[i].globalId === vm.users[j].id) {
                                    vm.permittedUsers.push({
                                        name: vm.users[j].name,
                                        globalId: vm.role.users[i].globalId,
                                        confirmedRead: true
                                    });
                                    break;
                                }
                            }
                        }
                    })
                })
            }
            else {       
                vm.role = {};
                vm.role.users = [];
            }
            RoleService.getAllUsers(function (data) {
                vm.users = data;
            });
        }

        function createRole() {
            console.log('Creating role');
            vm.role.users = vm.role.users || [];
            vm.role.users = vm.permittedUsers;
            RoleService.createRole(vm.role);
            $uibModalInstance.close();
        }

        function save() {
            console.log('Saving role...');
            vm.role.users = vm.role.users || [];
            vm.role.users = vm.permittedUsers;
            RoleService.saveRole(vm.role);
            $uibModalInstance.close();
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };

        function addRoleUser() {
            if (vm.selected.id != null) {
                if (vm.permittedUsers.find(x => x.globalId === vm.selected.id)) {
                    toastr.warning(
                   'User already exist in this role!', 'Admin panel',
                   {
                       closeButton: true, timeOut: 5000
                   });
                    return;
                };
                vm.permittedUsers.push({
                    name: vm.selected.name,
                    globalId: vm.selected.id
                });
            }
        };

        function removeRoleUser(id) {
            for (var i = 0; i < vm.permittedUsers.length; i++) {
                if (vm.permittedUsers[i].globalId === id) { vm.permittedUsers.splice(i, 1); break; }
            }
        };

        function setTab(newTab) {
            vm.tab = newTab;
        };

        function isSet(tabNum) {
            return vm.tab === tabNum;
        };

    }
}());