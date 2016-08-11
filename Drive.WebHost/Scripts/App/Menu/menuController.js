(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['MenuService'];

    function MenuController(menuService) {
        var vm = this;

        activate();

        function activate() {
            vm.message = menuService.getMessage();
            vm.redirectToBinarySpace = menuService.redirectToBinarySpace();
            vm.redirectToMySpace = menuService.redirectToMySpace();
            vm.redirectToSpaceSettings = menuService.redirectToSpaceSettings();
            vm.redirectToAddBinarySpace = menuService.redirectToAddBinarySpace();
            vm.redirectToAddFile = menuService.redirectToAddFile();
            vm.redirectToAddMySpace = menuService.redirectToAddMySpace();
        }
    }
}());