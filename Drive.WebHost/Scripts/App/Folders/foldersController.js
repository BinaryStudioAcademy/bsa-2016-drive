(function () {
    "use strict";

    angular.module("driveApp")
        .controller("FoldersController", FoldersController);

    FoldersController.$inject = ['$http', 'FoldersService'];

    function FoldersController($http, foldersService) {
        var vm = this;

        vm.title = "Folders";

        vm.folders = [];

        activate();

        function activate() {
            foldersService.getFolders(function (folders) {
                vm.folders = folders;
            });
        }
    }
}());