(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("ShareByLinkController", ShareByLinkController);

    ShareByLinkController.$inject = ['ShareByLinkService', '$uibModalInstance', 'items'];

    function ShareByLinkController(shareByLinkService, $uibModalInstance, items) {
        var vm = this;

        vm.cancel = cancel;
        vm.setShareLink = setShareLink;

        activate();

        function activate() {
            vm.content = items;

            //var shareLinksId = [];
            //vm.content.files.forEach(function (f) { shareLinksId.push(f.shareLinks); });
            //vm.content.folders.forEach(function (f) { shareLinksId.push(f.id); });
            //shareByLinkService.getLink(1, function (data) {
            //    console.log(data);
            //});
        }

        function setShareLink() {
            var contentsId = { filesId: [], foldersId: [] };
            vm.content.files.forEach(function (f) { contentsId.filesId.push(f.id); });
            vm.content.folders.forEach(function (f) { contentsId.foldersId.push(f.id);});
            shareByLinkService.setShareLink(contentsId, function (link) {
                vm.shareLink = link;
            });
            //$uibModalInstance.close();
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };










    }
}());