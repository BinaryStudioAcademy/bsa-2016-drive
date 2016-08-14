(function () {
    "use strict";

    angular.module("driveApp")
        .controller("TrelloController", TrelloController);

    function TrelloController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Trello";
        }
    }
}());