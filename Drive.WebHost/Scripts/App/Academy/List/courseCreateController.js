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
        vm.loadTags = loadTags;


        activate();

        function activate() {
            vm.title = "Add new course";
            vm.course = items;
            vm.editAuthor = false;
            vm.allTags = [];
            vm.isMatched = true;

            academyListService.getAllUsers(function(data) {
                vm.users = data.map(function(user) {
                    return { globalid: user.id, name: user.name };
                });
            });

            if (vm.course.id !== undefined)
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

        function addNewCourse(callback) {
            if (vm.course.name !== null) {
                academyListService.pushData(vm.course, callback);
            }
        };

        function save() {
            vm.check = false;
            if (vm.course.author.name === undefined) {
                vm.course.author = {name: vm.course.author};
                }
            for (var i = 0; i < vm.users.length; i++) {
                if (vm.users[i].name.toLowerCase() === vm.course.author.name.toLowerCase()) {
                    vm.check = true;
                    vm.course.author.globalid = vm.users[i].globalid;
                    break;
                }
            }
            if (vm.check != true) {
                vm.isMatched = false;
                return;
            }
            vm.message = 'New course was added successfully!';
            vm.operation = 'Create new Course';
            if (vm.course.id !== undefined) {
                vm.updateCourse(notifyAboutSuccess);
                vm.message = 'Course was updated successfully!';
                vm.operation = 'Update Course';
            } else
                vm.addNewCourse(notifyAboutSuccess);


        }

        function updateCourse(callback) {

            academyListService.putData(vm.course.id, vm.course, callback);
        }

        function notifyAboutSuccess(response) {

            if (response)
            {
                $timeout(function () {
                    $uibModalInstance.close(response);

                    toastr.success(
                   vm.message, vm.operation,
                   {
                       closeButton: true, timeOut: 6000
                   });
                }, 700);
            }
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }

        function openCalendar(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendar.isOpen = true;
        };

        function loadTags($query) {
            if (vm.allTags.length == 0) {
                academyListService.getAllTags(function (data) {
                    vm.allTags = data;
                    return vm.allTags.filter(function (tag) {
                        return tag.name.toLowerCase().indexOf($query.toLowerCase()) != -1;
                    })
                });
            }
            else {
                return vm.allTags.filter(function (tag) {
                    return tag.name.toLowerCase().indexOf($query.toLowerCase()) != -1;
                });
            }
        }
    }
})();