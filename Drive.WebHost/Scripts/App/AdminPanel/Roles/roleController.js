(function () {
    "use strict";

    angular.module("driveApp")
        .controller("RoleController", RoleController);

    RoleController.$inject = ['$uibModalInstance', 'RoleService', 'items'];

    function RoleController($uibModalInstance, RoleService, items) {
        var vm = this;
        vm.createRole = createRole;
        vm.cancel = cancel;
        vm.addUser = addUser;
        //vm.users = [{ id: 2, name: "Nikita Krasnov" }];
        vm.users = [];
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
        }

        function addUser() {
            //vm.users = [{ id: 1, name: "Nikita Krasnov" }];
            vm.users.push({id: 1, name: "Nikita Krasnov" });
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
    }
}());