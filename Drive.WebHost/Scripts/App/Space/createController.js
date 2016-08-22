(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("CreateController", CreateController);

    CreateController.$inject = ['SpaceService', '$uibModalInstance', '$timeout'];

    function CreateController(spaceService, $uibModalInstance, $timeout) {
        var vm = this;
        
        vm.addNewSpace = addNewSpace;
        vm.addSpaceUser = addSpaceUser;
        vm.addReadUser = addReadUser;
        vm.addWriteUser = addWriteUser;
        vm.removeSpaceUser = removeSpaceUser;
        vm.setChoice = setChoice;
        vm.save = save;
        vm.cancel = cancel;

        vm.space = {
            readPermittedUsers: [],
            modifyPermittedUsers: []
        }
        vm.permittedUsers = [];

        activate();

        function activate() {
                spaceService.getAllUsers(function (data) {
                    vm.users = data;
                    var i;
                    var j;
                    if (vm.space.readPermittedUsers !== undefined) {
                        for (i = 0; i < vm.space.readPermittedUsers.length; i++) {
                            for (j = 0; j < vm.users.length; j++) {
                                if (vm.space.readPermittedUsers[i].globalId === vm.users[j].id) {
                                    vm.permittedUsers.push({
                                        name: vm.users[j].name,
                                        globalId: vm.space.readPermittedUsers[i].globalId,
                                        confirmedRead: true
                                    });
                                    break;
                                }
                            }
                        }
                    }

                    vm.bool = true;
                    if (vm.space.modifyPermittedUsers !== null) {
                        for (i = 0; i < vm.space.modifyPermittedUsers.length; i++) {
                            vm.bool = true;
                            for (j = 0; j < vm.permittedUsers.length; j++) {
                                if (vm.space.modifyPermittedUsers[i].globalId === vm.permittedUsers[j].globalId) {
                                    vm.permittedUsers[j].confirmedWrite = true;
                                    vm.bool = false;
                                }
                            }
                            if (vm.bool) {
                                for (j = 0; j < vm.users.length; j++) {
                                    if (vm.space.modifyPermittedUsers[i].globalId === vm.users[j].id) {
                                        vm.permittedUsers.push({
                                            name: vm.users[j].name,
                                            globalId: vm.space.modifyPermittedUsers[i].globalId,
                                            confirmedWrite: true
                                        });
                                        break;
                                    }
                                }
                            }
                        }
                    }
                });

            vm.userAddName = null;
            vm.userAddId = null;
        }

        function addNewSpace() {
            if (vm.space.Name !== null) {
                spaceService.pushData(vm.space);
                vm.spaceName = null;
                vm.Description = null;
                vm.MaxFilesQuantity = null;
                vm.MaxFileSize = null;
            }
        };

        function addSpaceUser() {
            if (vm.userAddId !== null) {
                if (vm.permittedUsers.find(x => x.globalId === vm.userAddId)) {
                    vm.userAddName = null;
                    vm.userAddId = null;
                    console.log('User already exist in this space!');
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
            var i;
            if (bool === true) {
                for (i = 0; i < vm.permittedUsers.length; i++) {
                    if (vm.permittedUsers[i].globalId === id) {
                        vm.space.readPermittedUsers.push({
                            name: vm.permittedUsers[i].name,
                            globalId: vm.permittedUsers[i].globalId
                        });
                        break;
                    }
                }
            } else {
                for (i = 0; i < vm.space.readPermittedUsers.length; i++) {
                    if (vm.space.readPermittedUsers[i].globalId === id) {
                        vm.space.readPermittedUsers.splice(i, 1);
                        break;
                    }
                }
            }
        }

        function addWriteUser(bool, id) {
            var i;
            if (bool === true) {
                for (i = 0; i < vm.permittedUsers.length; i++) {
                    if (vm.permittedUsers[i].globalId === id) {
                        vm.space.modifyPermittedUsers.push({
                            name: vm.permittedUsers[i].name,
                            globalId: vm.permittedUsers[i].globalId
                        });
                        break;
                    }
                }
            } else {
                for (i = 0; i < vm.space.modifyPermittedUsers.length; i++) {
                    if (vm.space.modifyPermittedUsers[i].globalId === id) {
                        vm.space.modifyPermittedUsers.splice(i, 1);
                        break;
                    }
                }
            }
        }

        function removeSpaceUser(id) {
            for (var i = 0; i < vm.permittedUsers.length; i++) {
                if (vm.permittedUsers[i].globalId === id) { vm.permittedUsers.splice(i, 1); break; }
            }
        };

        function setChoice(name, id) {
            vm.userAddName = name;
            vm.userAddId = id;
        };

        function save() {
            vm.addNewSpace();

            $timeout(function () {
                $uibModalInstance.close();
            }, 3000);
        };

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());