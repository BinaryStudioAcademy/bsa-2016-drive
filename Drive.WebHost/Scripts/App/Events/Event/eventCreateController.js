(function () {
    angular.module('driveApp.academyPro')
        .controller('EventCreateController', EventCreateController);

    EventCreateController.$inject = [
        'toastr',
        '$location',
        'localStorageService'
    ];

    function EventCreateController(toastr, $location, localStorageService) {
        var vm = this;
        vm.addNewEvent = addNewEvent;
        vm.create = create;
        vm.cancel = cancel;
        vm.newVideo = newVideo;
        vm.newSimpleLink = newSimpleLink;
        vm.newPhoto = newPhoto;
        vm.newText = newText;
        vm.getEventList = getEventList;
        vm.saveContent = saveContent;

        activate();

        function activate() {
            vm.showEditArea = false;
            vm.order = 0;
            vm.tempevent = localStorageService.get('event');
            localStorageService.remove('event');

            vm.event = {
                fileUnit: vm.tempevent.fileUnit,
                contentList:[]
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
            //return eventService.pushData(vm.event);
        };

        function create() {
            if (vm.event.fileUnit.name) {
                vm.addNewEvent()
                    .then(function () {
                        $location.url('/apps/events');
                    });
                toastr.success(
                'New event was added successfully!', 'Events',
                {
                    closeButton: true, timeOut: 6000
                });
            }
        }

        function cancel() {

        }

        function openCalendar(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendar.isOpen = true;
        };


        function newVideo() {
            vm.showEditArea = true;
            vm.currentContent = {contentType: 3};            
        }

        function newSimpleLink() {
            vm.showEditArea = true;
            vm.currentContent = {contentType: 4};            
        }

        function newPhoto() {
            vm.showEditArea = true;
            vm.currentContent = { contentType: 2};
        }

        function newText() {
            vm.showEditArea = true;
            vm.currentContent = { contentType: 1};
        }

        function saveContent() {
            vm.currentContent.order = ++vm.order;
            vm.event.contentList.push(vm.currentContent);
            vm.showEditArea = false;
        }

        function getEventList() {
            $location.url('/apps/events');
        }


    }
})();