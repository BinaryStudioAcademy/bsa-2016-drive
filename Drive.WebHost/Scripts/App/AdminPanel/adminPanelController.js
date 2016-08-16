(function () {
    "use strict";

    angular.module("driveApp")
        .controller("adminPanelController", AdminPanelController);

    AdminPanelController.$inject = ['AdminPanelService', '$uibModal'];

    function AdminPanelController(adminPanelService, $uibModal) {
        var vm = this;
        vm.roles = ['HR', 'Tech Lead', 'Team Lead', 'Backend Developer'];
        vm.dirty = {};
        vm.suggest_role = suggest_role;
        vm.createRole = createRole;
        vm.autocomplete_options = {
            suggest: suggest_role
        };
        activate();

        function activate() {
            vm.title = "Admin Panel";
            //adminPanelService.getAllSpaces(function (data) {
            //    vm.spaces = data;
            //});
            //adminPanelService.getAllUsers(function (data) {
            //    vm.users = data;
            //});
            //adminPanelService.getAllRoles(function (data) {
            //    vm.roles = data;
            //})
        }

        function suggest_role(term) {
            var q = term.toLowerCase().trim();
            var results = [];
            for (var i = 0; i < vm.roles.length && results.length < 10; i++) {
                var role = vm.roles[i];
                if (role.toLowerCase().indexOf(q) === 0) {
                    results.push({ label: role, value: role });
                }
            }
            return results;
        }

        function createRole(size) {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'Scripts/App/AdminPanel/Roles/CreateRole.html',
                windowTemplateUrl: 'Scripts/App/Space/Modal.html',
                controller: 'RoleController',
                controllerAs: 'roleCtrl',
                keyboard: true,
                size: size
            });
            modalInstance.result.then(function () {
            }, function () {
            });
        };

    }
}());