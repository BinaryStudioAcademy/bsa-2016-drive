(function () {
    "use strict";

    angular.module("driveApp")
        .controller("adminPanelController", AdminPanelController);

    AdminPanelController.$inject = ['AdminPanelService', '$uibModal'];

    function AdminPanelController(adminPanelService, $uibModal) {
        var vm = this;
        vm.dirty = {};
        vm.suggest_role = suggest_role;
        vm.createRole = createRole;
        vm.editRole = editRole;
        vm.autocomplete_options = {
            suggest: vm.suggest_role,
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
            adminPanelService.getAllRoles(function(data) {
                vm.roles = data;
            });
            vm.states = ["asd", "qwe", "zxc", "zwdasda", "zxcxcvxvxcv"];
        }

        function suggest_role(term) {
            var q = term.toLowerCase().trim();
            var results = [];
            for (var i = 0; i < vm.roles.length && results.length < 10; i++) {
                var role = vm.roles[i].name;
                if (role.toLowerCase().indexOf(q) === 0) {
                    results.push({  value: role, label: role });

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
                size: size,
                resolve: {
                    items: function () {
                    }
            }
            });
            modalInstance.result.then(function () {
            }, function () {
            });
        };

        function editRole(size, id) {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'Scripts/App/AdminPanel/Roles/EditRole.html',
                windowTemplateUrl: 'Scripts/App/Space/Modal.html',
                controller: 'RoleController',
                controllerAs: 'roleCtrl',
                keyboard: true,
                size: size,
                resolve: {
                items: function () {
                    return id;
                }
        }
            });
            modalInstance.result.then(function () {
            }, function () {
            });
        }
    }
}());