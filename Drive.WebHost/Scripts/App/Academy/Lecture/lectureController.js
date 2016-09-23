(function () {
    "use strict";

    angular.module('driveApp.academyPro')
        .controller('LectureController', LectureController);

    LectureController.$inject = [
        'LectureService',
        '$routeParams',
        '$sce',
        'LinkTypeService',
        'toastr',
        '$location',
        'AcademyService',
        '$cookies'
    ];

    function LectureController(lectureService, $routeParams, $sce, linkTypeService, toastr, $location, academyService, $cookies) {
        var vm = this;
        vm.currentLectureId = $routeParams.lectureId;
        vm.currentAcademyId = $routeParams.id;
        vm.lecture = null;
        vm.trustSrc = trustSrc;
        vm.edit = edit;
        vm.update = update;
        vm.updateLecture = updateLecture;
        vm.linkTypes = linkTypeService;
        vm.getLecture = getLecture;
        vm.getAcademy = getAcademy;
        vm.submitTask = submitTask;
        vm.removeTask = removeTask;
        vm.editTask = editTask;
        vm.cancelUpdate = cancelUpdate;
        vm.getCourseList = getCourseList;
        vm.getCourse = getCourse;

        activate();

        function activate() {
            vm.isEditing = false;
            if ($location.path().indexOf('edit') + 1) {
                edit();
            }
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

            getAcademy();

            return getLecture();
        }

        function getLecture() {
            return lectureService.getLecture(vm.currentLectureId)
                .then(function (data) {
                    vm.lecture = data;
                    if ($cookies.get('serverUID') == data.author.globalId) {
                        vm.canEdit = true;
                    }
                    else {
                        vm.canEdit = false;
                    }
                    return vm.lecture;
                });
        }

        function getAcademy() {
            return academyService.getAcademy(vm.currentAcademyId)
                .then(function (data) {
                    vm.academy = data;
                    return vm.academy;
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
            .then(function () {
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
                'Lecture was successfully updated!', 'Academy Pro',
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

        function getCourseList() {
            $location.url('/apps/academy/');
        }

        function getCourse(id) {
            $location.url('/apps/academy/' + id);
        }
    }
}());