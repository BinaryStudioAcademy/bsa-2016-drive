(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("TextFileReaderCtrl", TextFileReaderCtrl);

    TextFileReaderCtrl.$inject = ['FileService', '$uibModalInstance', 'items', 'pdfDelegate', '$scope'];

    function TextFileReaderCtrl(fileService, $uibModalInstance, items, pdfDelegate, $scope) {
        var vm = this;

        vm.save = save;
        vm.cancel = cancel;
        vm.downloadFile = downloadFile;

        activate();

        function activate() {
            vm.file = items;
            var fileExtantion = vm.file.name.slice(vm.file.name.lastIndexOf("."));

            fileService.fileTextReader(vm.file.link,
                    function (response) {
                        var fileData = response.data;
                        var fileHeader = response.headers();
                        var contentType = fileHeader['content-type'];
                        var blob = new Blob([fileData], { type: contentType });
                        var url = URL.createObjectURL(blob);
                        vm.fileUrl = url;
                        if (fileExtantion == '.pdf') {
                            pdfDelegate.$getByHandle('my-pdf-container').load(url);
                            vm.pdfReaderShow = true;
                            vm.elseReaderShow = false;
                        }
                        else{
                            vm.elseReaderShow = true;
                            vm.pdfReaderShow = false;
                            var reader = new FileReader();
                            reader.onload = function (e) {
                                $scope.$apply(function () {
                                    var lines = null;
                                    lines = e.target.result.split("\n")
                                    vm.textFileContent = lines;
                                });
                            };
                            reader.readAsText(blob);
                        }
                        
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