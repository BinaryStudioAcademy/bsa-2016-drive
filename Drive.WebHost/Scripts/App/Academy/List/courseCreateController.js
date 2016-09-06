(function () {
    angular.module('driveApp.academyPro')
        .controller('CourseCreateController', CourseCreateController);

    CourseCreateController.$inject = [
        "AcademyListService",
        "$uibModalInstance",
        "toastr",
        "items",
        "$timeout"
    ];

    function CourseCreateController(academyListService, $uibModalInstance, toastr, items, $timeout) {
        var vm = this;

        vm.addNewCourse = addNewCourse;
        vm.save = save;
        vm.cancel = cancel;
        vm.updateCourse = updateCourse;
        vm.title = "Add new course";

        activate();

        function activate() {
            vm.course = items;
            if (vm.course.id > 0)
                vm.title = "Edit course";

            vm.calendar = {
                isOpen: false,
                openCalendar: openCalendar,
                timepickerOptions: {
                    showMeridian: false
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

        function save() {
            if (vm.course.id !== null)
                vm.updateCourse();
            else
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

        function updateCourse()
        {
            academyListService.putData(vm.course.id, vm.course);
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