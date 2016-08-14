(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['$location',  '$uibModal'];

    function MenuController($location,  $uibModal) {

        var vm = this;

        vm.redirectToBinarySpace = redirectToBinarySpace;
        vm.redirectToMySpace = redirectToMySpace;
        vm.redirectToSpaceSettings = redirectToSpaceSettings;
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
        vm.open = open;
        
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
            
        };
    }
}());