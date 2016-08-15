(function () {
    "use strict";

    angular.module("driveApp")
        .controller("DocsController", DocsController);

    function DocsController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Docs";
        }
    }
}());