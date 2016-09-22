(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("ShareByLinkController", ShareByLinkController);

    ShareByLinkController.$inject = ['ShareByLinkService', 'FileService', '$uibModalInstance', 'items', 'toastr'];

    function ShareByLinkController(shareByLinkService, fileService, $uibModalInstance, items, toastr) {
        var vm = this;

        vm.cancel = cancel;
        vm.setShareLink = setShareLink;
        vm.delItemFromList = delItemFromList;
        vm.chooseIcon = chooseIcon;
        vm.onSuccess = onSuccess;

        activate();

        function activate() {
            vm.content = items;
        }

        function setShareLink() {
            var contentsId = { filesId: [], foldersId: [] };
            vm.content.files.forEach(function(f) { contentsId.filesId.push(f.id); });
            vm.content.folders.forEach(function(f) { contentsId.foldersId.push(f.id); });
            shareByLinkService.setShareLink(contentsId,
                function(link) {
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

        function onSuccess() {
            toastr.success(
                'Shared link copied to clipboard!',
                'Share by link',
                {
                    closeButton: true,
                    timeOut: 5000
                });
        };

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };

        function chooseIcon(type) {
            return fileService.chooseIcon(type);
        }
    }
}());