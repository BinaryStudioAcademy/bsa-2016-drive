(function () {
    "use strict";

    angular.module("driveApp")
        .controller("FoldersController", FoldersController);

    FoldersController.$inject = ['$http', 'FoldersService'];

    function FoldersController($http, foldersService) {
        var vm = this;

        vm.title = "Folders";

        vm.folders = [];
        vm.folder = {};

        vm.getAll = getAll;
        vm.get = get;

        activate();

        function activate() {
            return get();
        }

        function get(id) {
            return foldersService.get(id, function (folder) {
                vm.folder = folder;
                return vm.folder;
            });
        }

        function getAll() {
            return foldersService.getAll(function (folders) {
                vm.folders = folders;
                return vm.folders;

            });
        }
    }
}());