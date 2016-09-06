(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('LectureController', LectureController);

    LectureController.$inject = [
        'LectureService',
        '$routeParams',
        '$sce',
        'LinkTypeService',
        'toastr'
    ];

    function LectureController(lectureService, $routeParams, $sce, linkTypeService, toastr) {
        var vm = this;
        vm.currentLectureId = $routeParams.lectureId;
        vm.lecture = null;
        vm.trustSrc = trustSrc;
        vm.edit = edit;
        vm.update = update;
        vm.updateLecture = updateLecture;
        vm.linkTypes = linkTypeService;
        vm.getLecture = getLecture;
        vm.submitVideo = submitVideo;
        vm.submitSlide = submitSlide;
        vm.submitRepository = submitRepository;
        vm.submitSample = submitSample;
        vm.submitUseful = submitUseful;

        activate();

        function activate() {
            vm.isEditing = false;

            vm.calendar = {
                isOpen: false,
                openCalendar: openCalendar,
                timepickerOptions: {
                    showMeridian: false
                }
            }

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
            vm.isEditing = !vm.isEditing;
        }

        function update() {
            return lectureService.putData(vm.currentLectureId, vm.lecture)
            .then(function() {
                vm.isEditing = !vm.isEditing;
            });
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

        function openCalendar(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendar.isOpen = true;
        };

        function submitVideo() {
            if (vm.currentVideo.name && vm.currentVideo.link) {
                vm.lecture.videoLinks.push(vm.currentVideo);
                vm.currentVideo = {};
            }
        }

        function submitSlide() {
            if (vm.currentSlide.name && vm.currentSlide.link) {
                vm.lecture.slidesLinks.push(vm.currentSlide);
                vm.currentSlide = {};
            }
        }

        function submitRepository() {
            if (vm.currentRepository.name && vm.currentRepository.link) {
                vm.lecture.repositoryLinks.push(vm.currentRepository);
                vm.currentRepository = {};
            }
        }

        function submitSample() {
            if (vm.currentSample.name && vm.currentSample.link) {
                vm.lecture.sampleLinks.push(vm.currentSample);
                vm.currentSample = {};
            }
        }

        function submitUseful() {
            if (vm.currentuseful.name && vm.currentuseful.link) {
                vm.lecture.usefulLinks.push(vm.currentuseful);
                vm.currentuseful = {};
            }
        }
    }
}());