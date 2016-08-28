(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('LectureController', LectureController);

    LectureController.$inject = ['LectureService', '$routeParams', '$sce'];

    function LectureController(lectureService, $routeParams, $sce) {
        var vm = this;
        vm.currentLectureId = $routeParams.lectureId;
        vm.lecture = null;
        vm.trustSrc = trustSrc;

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

        function trustSrc(src) {
            return $sce.trustAsResourceUrl(src);
        }
    }
}());