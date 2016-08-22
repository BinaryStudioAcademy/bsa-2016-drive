(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('LectureController', LectureController);

    function LectureController() {
        var vm = this;

        activate();

        function activate() {
            vm.title = "Academy Pro";
        }
    }
}());