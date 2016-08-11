(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['MenuService', '$location'];

    function MenuController(menuService) {
        var vm = this;
        vm.redirectToBinarySpace = redirectToBinarySpace;
        vm.redirectToMySpace = redirectToMySpace;
        vm.redirectToSpaceSettings = redirectToSpaceSettings;
        vm.redirectToAddBinarySpace = redirectToAddBinarySpace;
        vm.redirectToAddFile = redirectToAddFile;
        vm.redirectToAddMySpace = redirectToAddMySpace;

        activate();

        function activate() {
            vm.message = menuService.getMessage();
        }

        function redirectToBinarySpace(id) {
            $location.url('/api/spaces/' + id);
        };

        function redirectToSpaceSettings(id) {
            $location.url('/api/spaces/settings/' + id);
        };

        function redirectToAddBinarySpace() {
            $location.url('/api/spaces/');
        };

        function redirectToAddFile() {
            $location.url('api/files');
        };

        function redirectToAddMySpace() {
            $location.url('api/Space/spaces');
        };
    }
}());