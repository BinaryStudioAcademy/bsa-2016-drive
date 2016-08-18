(function () {
    "use strict";

    angular.module("driveApp")
        .controller("RoleController", RoleController);

    RoleController.$inject = ['$uibModalInstance', 'RoleService', 'items'];

    function RoleController($uibModalInstance, RoleService, items) {
        var vm = this;
        vm.createRole = createRole;
        vm.cancel = cancel;
        vm.addSpaceUser = addSpaceUser;
        vm.removeSpaceUser = removeSpaceUser;
        vm.setChoice = setChoice;
        //vm.users = [{ id: 2, name: "Nikita Krasnov" }];
        //vm.users = [];
        activate();

        function activate() {
            vm.title = "Admin Panel";
            vm.space = {};
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
            console.log('creating role');
            vm.space.Name = vm.name;
            vm.space.Description = vm.description;
            vm.space.Users = vm.users;
            RoleService.createRole(vm.space);
            $uibModalInstance.close();
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };

        function addSpaceUser() {
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

        function removeSpaceUser(id) {
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