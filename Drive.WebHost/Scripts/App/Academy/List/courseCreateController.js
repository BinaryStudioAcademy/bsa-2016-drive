(function () {
    angular.module('driveApp.academyPro')
        .controller('CourseCreateController', CourseCreateController);

    CourseCreateController.$inject = [
        'AcademyListService',
        '$uibModalInstance',
        'toastr',
        '$timeout'
    ];

    function CourseCreateController(academyListService, $uibModalInstance, toastr, $timeout) {
        var vm = this;
        vm.addNewCourse = addNewCourse;
        vm.create = create;
        vm.cancel = cancel;

        activate();

        function activate() {
            vm.course = {
                fileUnit: {}
            };

            vm.calendar = {
                isOpen: false,
                openCalendar: openCalendar,
                timepickerOptions: {
                    showMeridian: false,
                }
            }

            vm.dpOptions = {
                minDate: new Date(),
                showWeeks: true
            };
        }

        function addNewCourse() {
            if (vm.course.name !== null) {
                academyListService.pushData(vm.course);
            }
        };

        function create() {
            vm.addNewCourse();

            $timeout(function () {
                $uibModalInstance.close();

                toastr.success(
               'New course was added successfully!', 'Create new Course',
               {
                   closeButton: true, timeOut: 6000
               });
            }, 700);
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }

        function openCalendar(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendar.isOpen = true;
        };
    }
})();