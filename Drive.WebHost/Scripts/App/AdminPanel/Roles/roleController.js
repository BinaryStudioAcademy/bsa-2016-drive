(function () {
    "use strict";

    angular.module("driveApp")
        .controller("RoleController", RoleController);

    RoleController.$inject = ['$uibModalInstance', 'RoleService'];

    function RoleController($uibModalInstance, RoleService) {
        var vm = this;
        vm.createRole = createRole;
        vm.cancel = cancel;
        activate();

        function activate() {
            vm.title = "Admin Panel";
            vm.space = {};
            vm.space.Name = "RoleName";
            vm.space.Description = "RoleDescription";
            vm.name = null;
            vm.description = null;
        }

        function createRole() {
            console.log('creating role');
            //vm.space.push({
            //    Name: vm.name,
            //    Description: vm.description
            //});
            //vm.space.Name = vm.name;
            //vm.space.Description = vm.description;
            RoleService.createRole(vm.space);
            $uibModalInstance.close();
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());