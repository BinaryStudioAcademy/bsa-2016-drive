(function () {
    "use strict";

    angular.module("driveApp")
        .controller("FileController", FileController);

    FileController.$inject = ["FileService", "$routeParams"];

    function FileController(fileService, $routeParams) {
        var vm = this;

        var file;
        activate();

        vm.saveFile = saveFile;
        vm.update = update;
        vm.create = create;


        function saveFile() {
            if (id !== undefined) {
                function update() {
                    fileService.updateFile(id, file, function () { });
                }
            }
            else {
                function create() {
                    fileService.createFile(file, function() { });
                }
            }
        }

        //function create() {
        //    fileService.createFile(file, function() { });
        //}
        //function update() {
        //    fileService.updateFile(id, function () { });
        //}



        function activate() {
            var id = $routeParams["id"];

            if (id !== undefined) {
                vm.formTitle = 'Edit file';
                // init model
            }
            else {
                vm.formTitle = 'Create new file';
            }
           //file = { id: vm.id, name: vm.name, link: vm.link, description: vm.description };
        }
    }
}());