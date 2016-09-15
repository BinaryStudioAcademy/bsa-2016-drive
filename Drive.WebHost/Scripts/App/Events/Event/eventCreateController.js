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
        vm.saveContent = saveContent;
        vm.isValidEventUrl = isValidEventUrl;

        activate();

        function activate() {
            vm.showEditArea = false;
            vm.order = 0;
            eventService.getEventTypes(function (data) {
                vm.eventTypes = data;
            });
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

 

        function isValidEventUrl() {
            if (vm.currentContent.contentType === 1) {
                vm.urlIsValid = true;
                return;
            }

            var expression = "^(?:(?:ht|f)tps?://)?(?:[\\-\\w]+@)?(?:[\\-0-9a-z]*[0-9a-z]\\.)+[a-z]{2,6}(?::\\d{1,5})?(?:[?/\\\\#][?!^$.(){}:|=[\\]+\\-/\\\\*;&~#@,%\\wА-Яа-я]*)?$";
            var reg = new RegExp(expression);
            vm.urlIsValid = reg.test(vm.currentContent.content);
        }

        function getEventList() {
            $location.url('/apps/events');
        }


    }
})();