(function () {
    "use strict";

    angular.module("driveApp")
        .controller("FileController", FileController);

    FileController.$inject = ["$routeParams", "$location" , "FileService" ];

    function FileController($routeParams, $location, fileService) {
        var vm = this;

        vm.file;
        vm.files;

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
                console.log(vm.file);
                return vm.formTitle = 'Edit file';
                // init model
            }
            else {
                vm.formTitle = 'Create new file';
            }
            vm.getAllFiles();
        }

        function saveFile(file) {
            var id = $routeParams["id"];
            if (id !== undefined) {
                vm.updateFile(vm.file.Id, file);
            }
            else {
                vm.createFile(file);
            }
        }

        function createFile(file) {
            fileService.createFile(file, function (id) {
                $location.path('/file_list');
            });
        }

        function getAllFiles() {
            fileService.getAllFiles(function (response) {
                vm.files = response.data;
            });
        }

        function getFile(id) {
            fileService.getFile(id, function (file) {
                vm.file = file;
            });
        }

        function updateFile(id, file) {
            fileService.updateFile(id, file, function (id) {
                $location.path('/file_list');
            });
        }

        function deleteFile(id) {
            fileService.deleteFile(id);
        }
          
    }
}());