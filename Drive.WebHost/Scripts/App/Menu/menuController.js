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

        function redirectToBinarySpace(id) {
           $location.url('App/Space/Space' + id);
        };

        function redirectToSpaceSettings(id) {
            $location.url('App/Space/Settings/' + id);
        };

        function redirectToAddBinarySpace() {
            $location.url('App/Space/AddSpace');
        };

        function redirectToAddFile() {
            $location.url('App/Space/AddFile');
        };

        function redirectToAddMySpace() {
            $location.url('App/Space/AddSpace');
        };

        
    }
}());