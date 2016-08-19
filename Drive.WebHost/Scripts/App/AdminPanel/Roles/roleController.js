(function () {
    "use strict";

    angular.module("driveApp")
        .controller("RoleController", RoleController);

    RoleController.$inject = ['$uibModalInstance', 'RoleService', 'items'];

    function RoleController($uibModalInstance, RoleService, items) {
        var vm = this;
        vm.createRole = createRole;
        vm.cancel = cancel;
        vm.addRoleUser = addRoleUser;
        vm.removeRoleUser = removeRoleUser;
        vm.setChoice = setChoice;
        vm.save = save;
        activate();

        function activate() {
            vm.title = "Admin Panel";
            vm.name = null;
            vm.description = null;
            if (items !== undefined) {
                RoleService.getById(items, function (data) {
                    vm.role = data;
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
            vm.role.name = vm.name;
            vm.role.description = vm.description;
            vm.role.users = vm.role.users;
            RoleService.createRole(vm.role);
            $uibModalInstance.close();
        }

        function save() {
            console.log('Saving role...');
            RoleService.saveRole(vm.role);
            $uibModalInstance.close();
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };

        function addRoleUser() {
            if (vm.userAddId != null) {
                if (vm.role !== undefined) {
                if (vm.role.users.find(x => x.id === vm.userAddId)) {
                        vm.userAddName = null;
                        vm.userAddId = null;
                        console.log('The user already exist in this space!');
                        return;
                    }
                };

                vm.role.users.push({
                    name: vm.userAddName,
                    id: vm.userAddId
                });
                vm.userAddName = null;
                vm.userAddId = null;
            }
        };

        function removeRoleUser(id) {
            for (var i = 0; i < vm.role.users.length; i++) {
                if (vm.role.users[i].id === id) { vm.role.users.splice(i, 1); break; }
            }
        };

        function setChoice(name, id) {
            vm.userAddName = name;
            vm.userAddId = id;
        };
    }
}());