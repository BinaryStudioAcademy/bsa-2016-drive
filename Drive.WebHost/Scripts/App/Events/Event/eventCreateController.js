(function () {
    angular.module('driveApp.academyPro')
        .controller('EventCreateController', EventCreateController);

    EventCreateController.$inject = [
        'EventService',
        'toastr',
        '$location',
        'localStorageService'
    ];

    function EventCreateController(eventService, toastr, $location, localStorageService) {
        var vm = this;
        vm.addNewEvent = addNewEvent;
        vm.create = create;
        vm.cancel = cancel;
        vm.newVideo = newVideo;
        vm.newSimpleLink = newSimpleLink;
        vm.newPhoto = newPhoto;
        vm.newText = newText;
        vm.getEventList = getEventList;
        vm.contentSaved = contentSaved;
        vm.removeContent = removeContent;
        vm.editContent = editContent;
        vm.changeCollapseState = changeCollapseState;

        activate();

        function activate() {
            vm.showEditArea = false;
            vm.order = 0;
            vm.isCollapsed = false;
            vm.tempevent = localStorageService.get('event');

            vm.event = {
                fileUnit: vm.tempevent.fileUnit,
                contentList: []
            };

            vm.calendar = {
                isOpen: false,
                openCalendar: openCalendar,
                timepickerOptions: {
                    showMeridian: false
                }
            };

        }

        function addNewEvent() {
            if (vm.event.contentList.length > 0)
                for (var i = 0; i < vm.event.contentList.length; i++) {
                    vm.event.contentList[i].order = i;
                }
            return eventService.pushData(vm.event);
        };

        function create() {
            if (vm.event.fileUnit.name) {
                vm.addNewEvent()
                    .then(function () {
                        localStorageService.remove('event');
                        $location.url(localStorageService.get('location'));
                    });
                toastr.success(
                'New event was added successfully!', 'Events',
                {
                    closeButton: true, timeOut: 6000
                });
            }
        }

        function cancel() {
            localStorageService.remove('event');
            $location.url(localStorageService.get('location'));
        }

        function openCalendar(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendar.isOpen = true;
        };


        function newVideo() {
            vm.showEditArea = true;
            vm.currentContent = { contentType: 3 };
        }

        function newSimpleLink() {
            vm.showEditArea = true;
            vm.currentContent = { contentType: 4 };
        }

        function newPhoto() {
            vm.showEditArea = true;
            vm.currentContent = { contentType: 2 };
        }

        function newText() {
            vm.showEditArea = true;
            vm.currentContent = { contentType: 1 };
        }

        function contentSaved() {

            vm.showEditArea = false;
            vm.currentContent = {};
        }

        function getEventList() {
            $location.url('/apps/events');
        }

        function removeContent(index) {
            vm.event.contentList.splice(index, 1);
        };

        function editContent(index) {
            vm.event.contentList[index].order = index;
            vm.currentContent = vm.event.contentList[index];
            vm.currentContent.isEdit = true;
            vm.currentContent.isCollapsed = false;
        }

        function changeCollapseState() {
            vm.isCollapsed = !vm.isCollapsed;
            if (vm.event.contentList.length > 0)
                for (var i = 0; i < vm.event.contentList.length; i++) {
                    vm.event.contentList[i].isCollapsed = vm.isCollapsed;
                }
        }

    }
})();