(function () {
    "use strict";

    angular.module("driveApp")
        .controller("EventsController", EventsController);

    function EventsController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Events";
        }
    }
}());