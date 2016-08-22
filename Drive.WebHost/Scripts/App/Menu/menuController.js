(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['MenuService', '$location', '$uibModal', '$rootScope', '$timeout'];

    function MenuController(menuService, $location, $uibModal, $rootScope, $timeout) {

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
<<<<<<< HEAD
            $location.url('/AdminPanel');
        };

        function redirectToEmployees() {
            $location.url('/apps/employees');
        };

        function redirectToChecklist() {
            $location.url('/apps/checklist');
        };

        function redirectToTrello() {
            $location.url('/apps/trello');
        };

        function redirectToSheets() {
            $location.url('/apps/sheets');
        };

        function redirectToSlides() {
            $location.url('/apps/slides');
=======
            $location.url("/AdminPanel");
>>>>>>> refs/remotes/origin/develop
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
                getAllSpaces();
            },
            function () {
                getAllSpaces();
            });
        };

        function activate() {
            return getAllSpaces();
        }

        $rootScope.$on("getSpacesInMenu", function () {
            getAllSpaces();
        });
    }
}());