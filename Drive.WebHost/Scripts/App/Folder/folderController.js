(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("FolderController", FolderController);

    FolderController.$inject = ['FolderService', '$uibModal'];

    function FolderController(folderService, $uibModal) {
        var vm = this;
        vm.folder = {
            id: 0,
            isDeleted: false,
            name: '',
            description: ''
        };

        

        vm.folders = [];
        vm.getAll = getAll;
        vm.get = get;
        vm.create = create;
        vm.updateFolder = updateFolder;
        vm.deleteFolder = deleteFolder;
        vm.open = open;


        vm.menuOptions = [
            [
                'Share', function ($itemScope) {
                    console.log($itemScope.folder.id);
                }
            ],
            [
                'Edit', function($itemScope) {
                    console.log($itemScope.folder.id);
                }
            ],
            [
                'Delete', function ($itemScope) {
                    return deleteFolder($itemScope.folder.id);
                }
            ]
        ];
        //vm.otherMenuOptions = [
        //    [
        //        'Create', function ($itemScope) {

        //        }
        //    ]
        //];

        function open(size) {

            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'Scripts/App/Folder/Form.html',
                windowTemplateUrl: 'Scripts/App/Folder/Modal.html',
                controller: 'ModalInstanceCtrl',
                size: size,
                resolve: {
                    items: function () {
                        //return $scope.items;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                //$scope.selected = selectedItem;
            }, function () {
                console.log('Modal dismissed');
            });
        };

        activate();

        function activate() {
            return getAll();
        }

        function get(id) {
            folderService.get(id, function (folder) {
                vm.folder = folder;
            });
        }

        function getAll() {
            folderService.getAll(function (folders) {
                vm.folders = folders;
            });
        }

        function create() {
            folderService.create(vm.folder, function (id) {
                vm.folder.id = id;
            });
        }

        function updateFolder(id, folder) {
            folderService.updateFolder(id, folder);
        }

        function deleteFolder(id) {
            folderService.deleteFolder(id, function () {
                folderService.getAll(function (folders) {
                    vm.folders = folders;
                });
            });
        }
    }
}());