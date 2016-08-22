(function () {
    "use strict";

    angular.module("driveApp")
        .controller("FileFilterController", FileFilterController);

    FileFilterController.$inject = ['FileService', '$routeParams'];

    function FileFilterController(fileService, $routeParams) {
        var vm = this;

        activate();

        function activate() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.changeView = changeView;

            vm.spaces = [];

            setFileData();
            
            fileService.getFilesApp(vm.filesType, function (data) {
                vm.spaces = data;
            });
        }

        function setFileData() {
            switch ($routeParams.appName) {
                // not defined types => update Enum
                case 'academy':
                    vm.filesType = 'Academy Pro';
                    vm.icon = 'fa fa-graduation-cap fa-lg';
                    break;
                case 'events':
                    vm.filesType = 'Events';
                    vm.icon = 'fa fa-calendar fa-lg';
                    break;
                case 'employees':
                    vm.filesType = 'Employees';
                    vm.icon = 'fa fa-users fa-lg';
                    break;
                case 'checklist':
                    vm.filesType = 'Checklist';
                    vm.icon = 'fa fa-check-square-o fa-lg';
                    break;
                //
                case 'trello':
                    vm.filesType = 'Trello';
                    vm.icon = 'fa fa-trello fa-lg';
                    break;
                case 'docs':
                    vm.filesType = 'Document';
                    vm.icon = 'fa fa-file-text fa-lg';
                    break;
                case 'sheets':
                    vm.filesType = 'Sheets';
                    vm.icon = 'fa fa-table fa-lg';
                    break;
                case 'slides':
                    vm.filesType = 'Slides';
                    vm.icon = 'fa fa-file-powerpoint-o fa-lg';
                    break;
            }
        }

        function changeView(view) {
            if (view == "fa fa-th") {
                activateGridView();
            } else {
                activateTableView();
            }
        }
        function activateTableView() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
        }
        function activateGridView() {
            vm.view = "fa fa-list";
            vm.showTable = false;
            vm.showGrid = true;
        }
    }
}());