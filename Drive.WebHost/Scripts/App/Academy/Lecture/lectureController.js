(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('LectureController', LectureController);

    LectureController.$inject = ['LectureService', '$routeParams'];

    function LectureController(lectureService, $routeParams) {
        var vm = this;
        vm.currentLectureId = $routeParams.lectureId;
        vm.lecture = null;

        activate();

        function activate() {
            return getLecture();
        }

        function getLecture() {
            return lectureService.getLecture(vm.currentLectureId)
                .then(function (data) {
                    vm.lecture = data;
                    return vm.lecture;
                });
        }
    }
}());