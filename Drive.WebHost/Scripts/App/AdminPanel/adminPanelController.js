(function () {
    "use strict";

    angular.module("driveApp")
        .controller("adminPanelController", AdminPanelController);

    AdminPanelController.$inject = ['AdminPanelService', '$uibModal', '$window'];

    function AdminPanelController(adminPanelService, $uibModal, $window) {
        var vm = this;
        vm.dirty = {};
        vm.createRole = createRole;
        vm.editRole = editRole;
        vm.removeRole = removeRole;
        vm.tab = 1;
        vm.setTab = setTab;
        vm.isSet = isSet;
        activate();

        function activate() {
            vm.title = "Admin Panel";
            adminPanelService.getAllRoles(function(data) {
                vm.roles = data;
            });
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
            modalInstance.result.then(function (data) {
                adminPanelService.getAllRoles(function (data) {
                    vm.roles = data;
                });
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
                adminPanelService.getAllRoles(function (data) {
                    vm.roles = data;
                });
            }, function () {
            });
        }

        function setTab(newTab) {
            vm.tab = newTab;
        };

        function isSet(tabNum) {
            return vm.tab === tabNum;
        };

        function removeRole(id) {
            if (confirm('Are you really want to delete the role?') == true) {
                adminPanelService.deleteRole(id, function (response) {
                    if (response) {
                        var data = {
                            operation: 'delete',
                            item: response
                        }
                    }
                });
                for (var i = 0; i < vm.roles.length; i++) {
                    if (vm.roles[i].id === id) {
                        vm.roles.splice(i, 1); break;
                    }
                }
            } else {

            }
        }
    }
}());