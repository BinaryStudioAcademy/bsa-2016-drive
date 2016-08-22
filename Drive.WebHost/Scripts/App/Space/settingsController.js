(function () {
    "use strict";

    angular.module("driveApp")
        .controller("SettingsController", SettingsController);

    SettingsController.$inject = ['SettingsService', '$routeParams', '$location', '$rootScope'];

    function SettingsController(settingsService, $routeParams, $location, $rootScope) {
        var vm = this;
        vm.save = save;
        vm.cancel = cancel;     
        vm.addSpaceUser = addSpaceUser;
        vm.addReadUser = addReadUser;
        vm.addWriteUser = addWriteUser;
        vm.removeSpaceUser = removeSpaceUser;
        vm.setChoice = setChoice;
        vm.selectedSpace = $routeParams.id ? $routeParams.id : 1;

        vm.redirectToSpace = redirectToSpace;
            
        vm.space = {
            readPermittedUsers: [],
            modifyPermittedUsers: []
        }
        vm.permittedUsers = [];

        vm.deleteSpace = deleteSpace;
        vm.showDeleteBtn = showDeleteBtn;

        activate();

        function activate() {
            settingsService.getSpace(vm.selectedSpace, function (data) {
                vm.space = data;
                console.log(vm.space);

                settingsService.getAllUsers(function (data) {
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
            });
            

            vm.userAddName = null;
            vm.userAddId = null;
        }

        function save() {
            settingsService.pushChanges(vm.space, function() {
                $rootScope.$emit("getSpacesInMenu", {});
            });
            
            vm.redirectToSpace();
        };

        function cancel() {
            settingsService.getSpace(vm.selectedSpace, function (data) {
                vm.space = data;
            });
            vm.redirectToSpace();
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

        function removeSpaceUser(id) {
            for (var i = 0; i < vm.permittedUsers.length; i++) {
                if (vm.permittedUsers[i].globalId === id) { vm.permittedUsers.splice(i, 1); break; }
            }
        };

        function setChoice(name, id) {
            vm.userAddName = name;
            vm.userAddId = id;
        };

        function redirectToSpace() {
            $location.url("/spaces/" + vm.space.id);
        };

        function showDeleteBtn()
        {
            if (vm.space.name == 'Binary Space' || vm.space.name == 'My Space') {
                return false;
            }
            else {
                return true;
            }
        }

        function deleteSpace()
        {
            if (confirm('Are you really want to delete space and all inside folders and files?') == true)
            {
                settingsService.deleteSpace(vm.selectedSpace, function (response) {
                    if (response) {
                        var data = {
                            operation: 'delete',
                            item: response
                        }
                    }
                });
                $location.url("/");
            } else {

            }
        }
    }
}());