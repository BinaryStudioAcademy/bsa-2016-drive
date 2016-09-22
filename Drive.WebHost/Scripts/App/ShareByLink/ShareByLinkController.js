(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("ShareByLinkController", ShareByLinkController);

    ShareByLinkController.$inject = ['ShareByLinkService', 'FileService', '$uibModalInstance', 'items'];

    function ShareByLinkController(shareByLinkService, fileService, $uibModalInstance, items) {
        var vm = this;

        vm.cancel = cancel;
        vm.setShareLink = setShareLink;
        vm.delItemFromList = delItemFromList;
        vm.chooseIcon = chooseIcon;
        activate();

        function activate() {
            vm.content = items;
        }

        function setShareLink() {
            var contentsId = { filesId: [], foldersId: [] };
            vm.content.files.forEach(function (f) { contentsId.filesId.push(f.id); });
            vm.content.folders.forEach(function (f) { contentsId.foldersId.push(f.id);});
            shareByLinkService.setShareLink(contentsId, function (link) {
                vm.shareLink = link;
            });
        }

        function delItemFromList(index, isFile) {
            if (isFile) {
                vm.content.files.splice(index, 1);
            }
            else {
                vm.content.folders.splice(index, 1);
            }
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };

        function chooseIcon(type) {
            return fileService.chooseIcon(type);
        }








    }
}());