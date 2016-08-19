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
        vm.addReadUser = addReadUser;
        vm.addWriteUser = addWriteUser;
        vm.removeSpaceUser = removeSpaceUser;
        vm.setChoice = setChoice;
            
        vm.space = {
            readPermittedUsers: []
        }
        vm.permittedUsers = [];
        activate();

        function activate() {
            settingsService.getSpace(1, function (data) {
                vm.space = data;
                console.log(vm.space);

                settingsService.getAllUsers(function (data) {
                    vm.users = data;

                    if (vm.space.readPermittedUsers != undefined)
                        for (var i = 0; i < vm.space.readPermittedUsers.length; i++) {
                            for (var j = 0; j < vm.users.length; j++) {
                                if (vm.space.readPermittedUsers[i].globalId == vm.users[j].id) {
                                    vm.permittedUsers.push({
                                        name: vm.users[j].name,
                                        globalId: vm.space.readPermittedUsers[i].globalId,
                                        confirmed: true
                                    });
                                }
                            }
                        }
                });
            });
            

            vm.userAddName = null;
            vm.userAddId = null;
        }

        function save() {
            settingsService.pushChanges(vm.space);
        };

        function cancel() {
            settingsService.getSpace(1, function (data) {
                vm.space = data;
            });
        };

        function addSpaceUser() {
            if (vm.userAddId != null) {
                if (vm.permittedUsers.find(x => x.globalId === vm.userAddId)) {
                    vm.userAddName = null;
                    vm.userAddId = null;
                    console.log('The user already exist in this space!');
                    return;
                };
                vm.permittedUsers.push({
                    name: vm.userAddName,
                    globalId: vm.userAddId
                });
                vm.userAddName = null;
                vm.userAddId = null;
            }
        };

        function addReadUser(bool, id) {
            if (bool === true) {
                for (var i = 0; i < vm.permittedUsers.length; i++) {
                    if (vm.permittedUsers[i].globalId === id) {
                        vm.space.readPermittedUsers.push({
                            name: vm.permittedUsers[i].name,
                            globalId: vm.permittedUsers[i].globalId
                        });
                        break;
                    }
                }
            } else {
                for (var i = 0; i < vm.space.readPermittedUsers.length; i++) {
                    if (vm.space.readPermittedUsers[i].globalId === id) {
                        vm.space.readPermittedUsers.splice(i, 1);
                        break;
                    }
                }
            }
        }

        function addWriteUser(bool, id) {
            
        }

        function removeSpaceUser(id) {
            for (var i = 0; i < vm.space.readPermittedUsers.length; i++) {
                if (vm.space.readPermittedUsers[i].id === id) { vm.space.readPermittedUsers.splice(i, 1); break; }
            }
        };

        function setChoice(name, id) {
            vm.userAddName = name;
            vm.userAddId = id;
        };
    }
}());