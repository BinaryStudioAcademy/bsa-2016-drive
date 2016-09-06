(function () {
    angular.module('driveApp.academyPro')
        .controller('CourseCreateController', CourseCreateController);

    CourseCreateController.$inject = [
        'AcademyListService',
        '$uibModalInstance',
        'toastr',
        'items'
    ];

    function CourseCreateController(academyListService, $uibModalInstance, toastr, items) {
        var vm = this;
        vm.addNewCourse = addNewCourse;
        vm.create = create;
        vm.cancel = cancel;
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

            $uibModalInstance.close();
            toastr.success(
                'New course was added successfully!', 'Academy Pro',
                {
                    closeButton: true, timeOut: 6000
                });
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