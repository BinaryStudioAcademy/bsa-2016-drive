(function () {
    "use strict";

    angular.module("driveApp")
        .controller("SettingsController", SettingsController);

    SettingsController.$inject = ['SettingsService'];

    function SettingsController(settingsService) {
        var vm = this;

        activate();

        function activate() {
            vm.title = settingsService.getTitle();
            vm.spaceName = settingsService.getSpaceName();
            vm.description = settingsService.getDescription();
            vm.maxFilesQuantity = settingsService.getMaxFilesQuantity();
            vm.maxFileSize = settingsService.getMaxFileSize();
            vm.users = settingsService.getUsers();
            vm.spaceUsers = settingsService.getSpaceUsers();

            vm.userAddName = null;
            vm.userAddId = null;

            vm.save = function () {

            };

            vm.cancel = function () {

            };

            vm.setChoice = function (name, id) {
                vm.userAddName = name;
                vm.userAddId = id;
            };

            vm.addSpaceUser = function () {
                if (vm.userAddId != null) {
                    vm.spaceUsers.push({
                        name: vm.userAddName,
                        id: vm.userAddId
                    });
                    vm.userAddName = null;
                    vm.userAddId = null;
                }
            };

            vm.removeUser = function (id) {
                var i;
                for (i in vm.spaceUsers) {
                    if (vm.spaceUsers.hasOwnProperty(i)) {
                        if (vm.spaceUsers[i].id === id) {
                            vm.spaceUsers.splice(i, 1);
                        }
                    }
                }
            };
        }
    }
}());