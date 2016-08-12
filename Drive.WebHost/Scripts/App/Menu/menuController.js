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
        vm.redirectToAdminPanel = redirectToAdminPanel;
        vm.redirectToAcademyPro = redirectToAcademyPro;
        vm.redirectToDocs = redirectToDocs;
        vm.redirectToEvents = redirectToEvents;
        vm.redirectToEmployees = redirectToEmployees;
        vm.redirectToChecklist = redirectToChecklist;
        vm.redirectToTrello = redirectToTrello;
        vm.redirectToSheets = redirectToSheets;
        vm.redirectToSlides = redirectToSlides;
        
        activate();

        function redirectToBinarySpace(id) {
            $location.url('/api/spaces/' + id);
        };

        function redirectToMySpace(id) {
            $location.url('/api/spaces/' + id);
        };

        function redirectToSpaceSettings() {
            $location.url('/settings/');
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

        function redirectToEmployees() {
            $location.url('/employees');
        };

        function redirectToChecklist() {
            $location.url('/checklist');
        };

        function redirectToTrello() {
            $location.url('/trello');
        };

        function redirectToSheets() {
            $location.url('/sheets');
        };

        function redirectToSlides() {
            $location.url('/slides');
        };

        function redirectToAcademyPro() {
            $location.url('/academy');
        };

        function activate() {
        };
    }
}());