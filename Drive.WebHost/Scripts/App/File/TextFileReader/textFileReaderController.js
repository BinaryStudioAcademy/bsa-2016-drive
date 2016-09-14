(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("TextFileReaderCtrl", TextFileReaderCtrl);

    TextFileReaderCtrl.$inject = ['FileService', '$uibModalInstance', 'items', 'pdfDelegate'];

    function TextFileReaderCtrl(fileService, $uibModalInstance, items, pdfDelegate) {
        var vm = this;

        vm.save = save;
        vm.cancel = cancel;
        vm.downloadFile = downloadFile;

        activate();

        function activate() {
            vm.file = items;
            vm.pdfDelegate = pdfDelegate.$getByHandle('my-pdf-container');
            fileService.fileTextReader(vm.file.link,
                    function (response) {
                        var fileData = response.data;
                        var fileHeader = response.headers();
                        var contentType = fileHeader['content-type'];
                        var blob = new Blob([fileData], { type: contentType });
                        var url = URL.createObjectURL(blob);
                        vm.pdfUrl = url;
                        pdfDelegate.$getByHandle('my-pdf-container').load(url);
                    });
        }

        function save() {

        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        };

        function downloadFile(link) {
            fileService.downloadFile(link);
        }

    }
}());