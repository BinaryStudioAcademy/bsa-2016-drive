(function () {
    "use strict";

    angular.module("driveApp")
        .controller("MenuController", MenuController);

    MenuController.$inject = ['MenuService', '$location', '$uibModal', '$rootScope'];

    function MenuController(menuService, $location, $uibModal, $rootScope) {

        var vm = this;

        vm.redirectToBinarySpace = redirectToBinarySpace;
        vm.redirectToMySpace = redirectToMySpace;
        vm.redirectToSpace = redirectToSpace;
        vm.redirectToSharedSpace = redirectToSharedSpace;
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
                for (var i = 0; i < vm.spaces.length; i++) {
                    if (vm.spaces[i].type === 0) { vm.binarySpace = vm.spaces[i] };
                    if (vm.spaces[i].type === 1) { vm.mySpace = vm.spaces[i] };
                }
            });

        }

        function redirectToBinarySpace(id) {
            $location.url("/binaryspace/" + id);
        };

        function redirectToMySpace(id) {
            $location.url("/myspace/" + id);
        };

        function redirectToSharedSpace() {
            $location.url("/sharedspace");
        };

        function redirectToSpace(id) {
            $location.url("/others/" + id);
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
                getAllSpaces();
            },
            function () {
                getAllSpaces();
            });
        };

        function activate() {
            getAllSpaces();
        }

        $rootScope.$on("getSpacesInMenu", function () {
            getAllSpaces();
        });
    }
}());