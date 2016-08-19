(function () {
    "use strict";

    angular.module("driveApp")
        .controller("SettingsController", SettingsController);

    SettingsController.$inject = ['SettingsService', '$routeParams'];

    function SettingsController(settingsService, $routeParams) {
        var vm = this;
        vm.save = save;
        vm.cancel = cancel;
        vm.addSpaceUser = addSpaceUser;
        vm.removeSpaceUser = removeSpaceUser;
        vm.setChoice = setChoice;
        vm.selectedSpace = $routeParams.id ? $routeParams.id : 1;
            
        vm.space = {
            readPermittedUsers: []
        }

        activate();

        function activate() {
            settingsService.getSpace(vm.selectedSpace, function (data) {
                vm.space = data;

                settingsService.getAllUsers(function (data) {
                    vm.users = data;

                    if (vm.space.readPermittedUsers != undefined)
                        for (var i = 0; i < vm.space.readPermittedUsers.length; i++) {
                            for (var j = 0; j < vm.users.length; j++) {
                                if (vm.space.readPermittedUsers[i].globalId == vm.users[j].id) {
                                    vm.space.readPermittedUsers[i].name = vm.users[j].name;
                                }
                            }
                        }
                });
                console.log(vm.space.readPermittedUsers);
            });
            

            vm.userAddName = null;
            vm.userAddId = null;
        }

        function save() {
            console.log(vm.space.readPermittedUsers);
            settingsService.pushChanges(vm.space);
        };

        function cancel() {
            settingsService.getSpace(vm.selectedSpace, function (data) {
                vm.space = data;
            });
        };

        function addSpaceUser() {
            if (vm.userAddId != null) {
                if (vm.space.readPermittedUsers.find(x => x.globalId === vm.userAddId)) {
                    vm.userAddName = null;
                    vm.userAddId = null;
                    console.log('The user already exist in this space!');
                    console.log(vm.space.readPermittedUsers);
                    return;
                };
                vm.space.readPermittedUsers.push({
                    name: vm.userAddName,
                    globalId: vm.userAddId
                });
                vm.userAddName = null;
                vm.userAddId = null;
            }
        };

        function removeSpaceUser(id) {
            for (var i = 0; i < vm.space.readPermittedUsers.length; i++) {
                if (vm.space.readPermittedUsers[i].id === id) { vm.space.readPermittedUsers.splice(i, 1); break; }
            }
            console.log(vm.space.readPermittedUsers);
        };

        function setChoice(name, id) {
            vm.userAddName = name;
            vm.userAddId = id;
        };
    }
}());