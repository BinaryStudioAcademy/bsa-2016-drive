(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['$location'];

    function MenuController() {

        var vm = this;

        vm.redirectToBinarySpace = redirectToBinarySpace;
        vm.redirectToSpaceSettings = redirectToSpaceSettings;
        vm.redirectToAddBinarySpace = redirectToAddBinarySpace;
        vm.redirectToAddFile = redirectToAddFile;
        vm.redirectToAddMySpace = redirectToAddMySpace;
        
        activate();

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

        function activate() {
        };
    }
}());