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
        vm.submitTask = submitTask;
        vm.removeTask = removeTask;
        vm.editTask = editTask;
        vm.cancelUpdate = cancelUpdate;

        activate();

        function activate() {
            vm.isEditing = false;

            vm.currentTask = {};

            vm.calendar = {
                isOpen: false,
                openCalendar: openCalendar,
                timepickerOptions: {
                    showMeridian: false
                }
            }

            vm.calendarHomeTask = {
                isOpen: false,
                openCalendarHomeTask: openCalendarHomeTask,
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
            vm.isEditing = true;
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
                        return vm.getLecture();
                    });
                toastr.success(
                'New lecture was successfully updated!', 'Academy Pro',
                {
                    closeButton: true, timeOut: 6000
                });
            }
        }

        function cancelUpdate() {
            vm.isEditing = false;
            return vm.getLecture();
        }

        function openCalendar(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendar.isOpen = true;
        };

        function openCalendarHomeTask(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendarHomeTask.isOpen = true;
        };

        function submitTask() {
            if (vm.currentTask.description) {
                vm.lecture.homeTasks.push(vm.currentTask);
                vm.currentTask = {};
            }
        }

        function removeTask(index) {
            vm.lecture.homeTasks.splice(index, 1);
        };

        function editTask(index) {
            vm.currentTask = vm.lecture.homeTasks[index];
            vm.lecture.homeTasks.splice(index, 1);
        };
    }
}());