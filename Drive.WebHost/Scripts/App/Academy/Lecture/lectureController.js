(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('LectureController', LectureController);

    LectureController.$inject = ['LectureService', '$routeParams', '$sce', 'LinkTypeService'];

    function LectureController(lectureService, $routeParams, $sce, linkTypeService) {
        var vm = this;
        vm.currentLectureId = $routeParams.lectureId;
        vm.lecture = null;
        vm.trustSrc = trustSrc;
        vm.edit = edit;
        vm.update = update;
        vm.updateLecture = updateLecture;
        vm.linkTypes = linkTypeService;

        activate();

        function activate() {
            vm.isEditing = false;
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

        function edit() {
            vm.isEditing = true;
        }

        function update() {
            return lectureService.pushData(vm.lecture);
        }

        function updateLecture() {
            if (vm.lecture.name) {
                vm.update()
                    .then(function () {
                        vm.getLecture();
                    });
                toastr.success(
                'New lecture was successfully updated!', 'Academy Pro',
                {
                    closeButton: true, timeOut: 6000
                });
            }
        }
    }
}());