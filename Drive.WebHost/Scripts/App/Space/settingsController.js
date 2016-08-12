(function () {
    "use strict";

    angular.module("driveApp")
        .controller("SettingsController", SettingsController);

    SettingsController.$inject = ['SettingsService'];

    function SettingsController(settingsService) {
        var vm = this;
        vm.save = save;
        vm.cancel = cancel;
        vm.addSpaceUser = addSpaceUser;
        vm.removeSpaceUser = removeSpaceUser;
        vm.setChoice = setChoice;

        activate();

        function activate() {
            settingsService.getSpace(1, function (data) {
                vm.space = data;
            });
            settingsService.getAllUsers(function (data) {
                vm.users = data;
            });
            vm.userAddName = null;
            vm.userAddId = null;
        }

        function save() {
            settingsService.pushChanges(vm.space);
        };

        function cancel() {
            location.reload();
        };

        function addSpaceUser() {
            if (vm.userAddId != null) {
                if (vm.space.ReadPermittedUsers.find(x => x.Id === vm.userAddId)) {
                    vm.userAddName = null;
                    vm.userAddId = null;
                    console.log('The user already exist in this space!');
                    return;
                };
                vm.space.ReadPermittedUsers.push({
                    Login: vm.userAddName,
                    Id: vm.userAddId
                });
                vm.userAddName = null;
                vm.userAddId = null;
            }
        };

        function removeSpaceUser(id) {
            for (var i = 0; i < vm.space.ReadPermittedUsers.length; i++) {
                if (vm.space.ReadPermittedUsers[i].Id === id) { vm.space.ReadPermittedUsers.splice(i, 1); break; }
            }
        };

        function setChoice(name, id) {
            vm.userAddName = name;
            vm.userAddId = id;
        };
    }
}());