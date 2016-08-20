(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['MenuService', '$location', '$uibModal'];

    function MenuController(menuService, $location, $uibModal) {

        var vm = this;

        vm.redirectToBinarySpace = redirectToBinarySpace;
        vm.redirectToMySpace = redirectToMySpace;
        vm.redirectToSpace = redirectToSpace;
        vm.redirectToAddFile = redirectToAddFile;
        vm.redirectToAdminPanel = redirectToAdminPanel;
        vm.redirectToApps = redirectToApps;
        vm.getAllSpaces = getAllSpaces;
        vm.open = open;
        vm.spaces = {};

        activate();

        function getAllSpaces() {
            menuService.getAllSpaces(function (data) {
                vm.spaces = data;
            });
        }

        function redirectToBinarySpace() {
            $location.url("/binaryspace");
        };

        function redirectToMySpace() {
            $location.url("/myspace");
        };

        function redirectToSpace(id) {
            $location.url("/spaces/" + id);
        };

        function redirectToAddFile() {
            $location.url("api/files/");
        };

        function redirectToApps(app) {
            $location.url("/apps/" + app);
        };

        function redirectToAdminPanel() {
            $location.url("/AdminPanel");
        };

        //Open modal window for creating new space
        function open(size) {

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'Scripts/App/Space/Create.html',
                windowTemplateUrl: 'Scripts/App/Space/Modal.html',
                controller: 'CreateController',
                controllerAs: 'createCtrl',
                keyboard: true,
                size: size

            });

            modalInstance.result.then(function () {
            }, function () {

            });
        };

        function activate() {
            return getAllSpaces();
        }
    }
}());