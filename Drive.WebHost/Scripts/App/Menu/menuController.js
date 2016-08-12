(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['$location'];

    function MenuController() {

        var vm = this;

        vm.redirectToBinarySpace = redirectToBinarySpace;
        vm.redirectToMySpace = redirectToMySpace;
        vm.redirectToSpaceSettings = redirectToSpaceSettings;
        vm.redirectToAddBinarySpace = redirectToAddBinarySpace;
        vm.redirectToAddFile = redirectToAddFile;
        vm.redirectToAddMySpace = redirectToAddMySpace;
        vm.redirectToNetSpace = redirectToNetSpace;
        vm.redirectToDocs = redirectToDocs;
        vm.redirectToEvents = redirectToEvents;
        vm.redirectToAdminPanel = redirectToAdminPanel;
        vm.redirectToUsersSettings = redirectToUsersSettings;
        
        activate();

        function redirectToBinarySpace(id) {
            $location.url('/api/spaces/' + id);
        };

        function redirectToMySpace(id) {
            $location.url('/api/spaces/' + id);
        };

        function redirectToSpaceSettings(id) {
            $location.url('/api/spaces/settings/' + id);
        };

        function redirectToAddBinarySpace() {
            $location.url('/api/spaces/');
        };

        function redirectToAddFile() {
            $location.url('api/files/');
        };

        function redirectToAddMySpace() {
            $location.url('api/spaces/');
        };

        function redirectToNetSpace() {
            $location.url('api/netSpace/');
        };

        function redirectToDocs() {
            $location.url('api/docs/');
        };

        function redirectToEvents() {
            $location.url('api/events/');
        };

        function redirectToAdminPanel() {
            $location.url('api/adminPanel/');
        };

        function redirectToUsersSettings() {
            $location.url('api/usersSettings/');
        };

        function activate() {
        };
    }
}());