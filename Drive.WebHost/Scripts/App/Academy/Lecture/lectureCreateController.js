(function () {
    angular.module('driveApp.academyPro')
        .controller('LectureCreateController', LectureCreateController);

    LectureCreateController.$inject = [
        'LectureService',
        'toastr'
    ];

    function LectureCreateController(lectureService, toastr) {
        var vm = this;
        vm.addNewLecture = addNewLecture;
        vm.create = create;
        vm.cancel = cancel;

        activate();

        function activate() {
            vm.lecture = {

            };

            vm.calendar = {
                isOpen: false,
                openCalendar: openCalendar,
                timepickerOptions: {
                    showMeridian: false
                }
            }
        }

        function addNewLecture() {
            if (vm.lecture.name !== null) {
                lectureService.pushData(vm.course);
            }
        };

        function create() {
            vm.addNewLecture();
        }

        function cancel() {
            
        }

        function openCalendar(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendar.isOpen = true;
        };
    }
})();