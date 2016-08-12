(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['$location'];

    function MenuController($location) {

        var vm = this;

        vm.redirectToBinarySpace = redirectToBinarySpace;
        vm.redirectToMySpace = redirectToMySpace;
        vm.redirectToSpaceSettings = redirectToSpaceSettings;
        vm.redirectToAddNewSpace = redirectToAddNewSpace;
        vm.redirectToAddFile = redirectToAddFile;
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

        function redirectToAddNewSpace() {
            $location.url('/api/spaces/');
        };

        function redirectToAddFile() {
            $location.url('api/files/');
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
            $location.url('/settings');
        };

        function activate() {
        };
    }
}());