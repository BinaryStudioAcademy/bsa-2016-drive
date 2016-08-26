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
        vm.addSpaceRole = addSpaceRole;
        vm.addReadUser = addReadUser;
        vm.addWriteUser = addWriteUser;
        vm.addReadRole = addReadRole;
        vm.addWriteRole = addWriteRole;
        vm.removeSpaceUser = removeSpaceUser;
        vm.removeSpaceRole = removeSpaceRole;
        vm.create = create;
        vm.cancel = cancel;

        vm.space = {
            readPermittedUsers: [],
            modifyPermittedUsers: [],
            readPermittedRoles: [],
            modifyPermittedRoles: []
        }
        vm.permittedUsers = [];
        vm.permittedRoles = [];
        vm.tab = 1;
        vm.setTab = setTab;
        vm.isSet = isSet;
        activate();

        function activate() {
            spaceService.getAllUsers(function (data) {
                vm.users = data;
                if (vm.space.readPermittedUsers != undefined) {
                    for (var i = 0; i < vm.space.readPermittedUsers.length; i++) {
                        for (var j = 0; j < vm.users.length; j++) {
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
                if (vm.space.modifyPermittedUsers != null) {
                    for (var i = 0; i < vm.space.modifyPermittedUsers.length; i++) {
                        vm.bool = true;
                        for (var j = 0; j < vm.permittedUsers.length; j++) {
                            if (vm.space.modifyPermittedUsers[i].globalId === vm.permittedUsers[j].globalId) {
                                vm.permittedUsers[j].confirmedWrite = true;
                                vm.bool = false;
                            }
                        }
                        if (vm.bool) {
                            for (var j = 0; j < vm.users.length; j++) {
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
            spaceService.getAllRoles(function (data) {
                vm.roles = data;
                if (vm.space.readPermittedRoles != undefined) {
                    for (var i = 0; i < vm.space.readPermittedRoles.length; i++) {
                        for (var j = 0; j < vm.roles.length; j++) {
                            if (vm.space.readPermittedRoles[i].id === vm.roles[j].id) {
                                vm.permittedRoles.push({
                                    name: vm.roles[j].name,
                                    id: vm.space.readPermittedRoles[i].id,
                                    confirmedRead: true
                                });
                                break;
                            }
                        }
                    }
                }
                vm.bool = true;
                if (vm.space.modifyPermittedRoles != null) {
                    for (var i = 0; i < vm.space.modifyPermittedRoles.length; i++) {
                        vm.bool = true;
                        for (var j = 0; j < vm.permittedRoles.length; j++) {
                            if (vm.space.modifyPermittedRoles[i].id === vm.permittedRoles[j].id) {
                                vm.permittedRoles[j].confirmedWrite = true;
                                vm.bool = false;
                            }
                        }
                        if (vm.bool) {
                            for (var j = 0; j < vm.roles.length; j++) {
                                if (vm.space.modifyPermittedRoles[i].id === vm.roles[j].id) {
                                    vm.permittedRoles.push({
                                        name: vm.roles[j].name,
                                        id: vm.space.modifyPermittedRoles[i].id,
                                        confirmedWrite: true
                                    });
                                    break;
                                }
                            }
                        }
                    }
                }
            });
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
            if (vm.selected.id != null) {
                if (vm.permittedUsers.find(x => x.globalId === vm.selected.id)) {
                    console.log('The user already exist in this space!');
                    return;
                };
                vm.permittedUsers.push({
                    name: vm.selected.name,
                    globalId: vm.selected.id
                });
            }
        };

        function addSpaceRole() {
            if (vm.selectedRole.id != null) {
                if (vm.permittedRoles.find(x => x.id == vm.selectedRole.id)) {
                    console.log('The role already exist in this space!');
                    return;
                };
                vm.permittedRoles.push({
                    name: vm.selectedRole.name,
                    id: vm.selectedRole.id
                });
            }
        }

        function addReadUser(bool, id) {
            vm.space.readPermittedUsers = vm.space.readPermittedUsers || [];
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

        function addReadRole(bool, id) {
            vm.space.readPermittedRoles = vm.space.readPermittedRoles || [];
            if (bool === true) {
                for (var i = 0; i < vm.permittedRoles.length; i++) {
                    if (vm.permittedRoles[i].id === id) {
                        vm.space.readPermittedRoles.push({
                            name: vm.permittedRoles[i].name,
                            id: vm.permittedRoles[i].id
                        });
                        break;
                    }
                }
            } else {
                for (var i = 0; i < vm.space.readPermittedRoles.length; i++) {
                    if (vm.space.readPermittedRoles[i].id === id) {
                        vm.space.readPermittedRoles.splice(i, 1);
                        break;
                    }
                }
            }
        }

        function addWriteUser(bool, id) {
            vm.space.modifyPermittedUsers = vm.space.modifyPermittedUsers || [];
            if (bool === true) {
                for (var i = 0; i < vm.permittedUsers.length; i++) {
                    if (vm.permittedUsers[i].globalId === id) {
                        vm.space.modifyPermittedUsers.push({
                            name: vm.permittedUsers[i].name,
                            globalId: vm.permittedUsers[i].globalId
                        });
                        break;
                    }
                }
            } else {
                for (var i = 0; i < vm.space.modifyPermittedUsers.length; i++) {
                    if (vm.space.modifyPermittedUsers[i].globalId === id) {
                        vm.space.modifyPermittedUsers.splice(i, 1);
                        break;
                    }
                }
            }
        }

        function addWriteRole(bool, id) {
            vm.space.modifyPermittedRoles = vm.space.modifyPermittedRoles || [];
            if (bool === true) {
                for (var i = 0; i < vm.permittedRoles.length; i++) {
                    if (vm.permittedRoles[i].id === id) {
                        vm.space.modifyPermittedRoles.push({
                            name: vm.permittedRoles[i].name,
                            id: vm.permittedRoles[i].id
                        });
                        break;
                    }
                }
            } else {
                for (var i = 0; i < vm.space.modifyPermittedRoles.length; i++) {
                    if (vm.space.modifyPermittedRoles[i].id === id) {
                        vm.space.modifyPermittedRoles.splice(i, 1);
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

        function removeSpaceRole(id) {
            for (var i = 0; i < vm.permittedRoles.length; i++) {
                if (vm.permittedRoles[i].id === id) { vm.permittedRoles.splice(i, 1); break; }
            }
        };

        function create() {
            vm.addNewSpace();

            $timeout(function () {
                $uibModalInstance.close();
            }, 2500);
        };

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };

        function setTab(newTab) {
            vm.tab = newTab;
        };

        function isSet(tabNum) {
            return vm.tab === tabNum;
        };
    }
}());