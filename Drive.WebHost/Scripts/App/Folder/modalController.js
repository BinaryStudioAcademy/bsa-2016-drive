angular.module('driveApp').controller('ModalInstanceCtrl', function ($scope, $uibModalInstance, items) {

    //$scope.items = items;
    //$scope.selected = {
    //    item: $scope.items[0]
    //};

    //$scope.ok = function () {
    //    $uibModalInstance.close($scope.selected.item);
    //};

    //$scope.cancel = function () {
    //    $uibModalInstance.dismiss('cancel');
    //};
});

(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("ModalInstanceCtrl", ModalInstanceCtrl);

    ModalInstanceCtrl.$inject = ['FolderService', '$uibModalInstance'];

    function ModalInstanceCtrl(folderService, $uibModalInstance) {
        var vm = this;
        vm.folder = {
            id: 0,
            isDeleted: false,
            name: '',
            description: ''
        };

        vm.create = create;
        vm.cancel = cancel;

        function create() {
            folderService.create(vm.folder, function (id) {
                vm.folder.id = id;
                $uibModalInstance.close(vm.folder.id);
            });
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());