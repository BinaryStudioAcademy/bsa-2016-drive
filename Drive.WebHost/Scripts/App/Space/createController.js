(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("CreateController", CreateController);

    CreateController.$inject = ['CreateService', '$uibModalInstance'];

    function CreateController(createService, $uibModalInstance) {
        var vm = this;
        vm.save = save;
        vm.cancel = cancel;
        vm.addNewSpace = addNewSpace;
        vm.addSpaceUser = addSpaceUser;
        vm.removeSpaceUser = removeSpaceUser;
        vm.setChoice = setChoice;

        activate();

        function activate() {
            createService.getSpace(1, function (data) {
                vm.space = data;
            });
            createService.getAllUsers(function (data) {
                vm.users = data;
            });
            
            vm.Name = null;
            vm.Description = null;
            vm.MaxFilesQuantity = null;
            vm.MaxFileSize = null;
            vm.userAddName = null;
            vm.userAddId = null;
        }

        function addNewSpace() {
            if (vm.Name != null) {
                vm.space.push({
                    Name: vm.Name,
                    Description: vm.Description,
                    MaxFilesQuantity: vm.MaxFilesQuantity,
                    MaxFileSize: vm.MaxFileSize
                });
                vm.Name = null;
                vm.Description = null;
                vm.MaxFilesQuantity = null;
                vm.MaxFileSize = null;
            }
        };

        function addSpaceUser() {
            if (vm.userAddId != null) {
                if (vm.space.ReadPermittedUsers.find(x => x.Id === vm.userAddId)) {
                    vm.userAddId = null;
                    vm.userAddName = null;
                    console.log('User already exist in this space!');
                    return;
                };
                vm.space.ReadPermittedUsers.push({
                    Id: vm.userAddId,
                    Login: vm.userAddName
                });
                vm.userAddName = null;
                vm.userAddId = null;
            }
        };

        function removeSpaceUser(id) {
            for (var i = 0; i < vm.space.ReadPermittedUsers.length; i++) {
                if (vm.space.ReadPermittedUsers[i].Id === id)
                {
                    vm.space.ReadPermittedUsers.splice(i, 1);
                    break;
                }
            }
        };

        function setChoice(name, id) {
            vm.userAddName = name;
            vm.userAddId = id;
        };

        function save() {
            createService.pushData(vm.space);
            $uibModalInstance.close();
        };

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());