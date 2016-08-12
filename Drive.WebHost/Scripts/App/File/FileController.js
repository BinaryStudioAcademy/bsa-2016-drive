(function () {
    "use strict";

    angular.module("driveApp")
        .controller("FileController", FileController);

    FileController.$inject = ["FileService", "$routeParams"];

    function FileController(fileService, $routeParams) {
        var vm = this;

        vm.file = {
            id: 0,
            idDeleted: false,
            name: "",
            description: ""
        }
        vm.files = [];

        vm.saveFile = saveFile;
        vm.updateFile = updateFile;
        vm.createFile = createFile;
        vm.deleteFile = deleteFile;
        vm.getFile = getFile;
        vm.getAllFiles = getAllFiles;

        activate();
        function activate() {
            var id = $routeParams["id"];

            if (id !== undefined) {
                vm.formTitle = 'Edit file';
                // init model
            }
            else {
                vm.formTitle = 'Create new file';
            }
           
        }

        function saveFile() {
            if (id !== undefined) {
                vm.update();
            }
            else {
                vm.create();
            }
        }

        function createFile(file) {
            fileService.createFile(file, function (id) {
                vm.file.id = id;
            });
        }

        function getAllFiles() {
            fileService.getAllFiles(function (files) {
                vm.files = files;
            });
        }

        function getFile(id) {
            fileService.getFile(id, function (file) {
                vm.file = file;
            });
        }

        function updateFile(id, file) {
            fileService.updateFile(id, file);
        }

        function deleteFile(id) {
            fileService.deleteFile(id);
        }
          
    }
}());