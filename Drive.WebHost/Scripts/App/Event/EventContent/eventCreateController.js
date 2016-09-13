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
        vm.submitVideo = submitVideo;
        vm.submitSimpleLink = submitSimpleLink;
        vm.submitPhoto = submitPhoto;
        vm.submitText = submitText;
        vm.getEventList = getEventList;

        activate();

        function activate() {
            vm.tempevent = localStorageService.get('event');
            localStorageService.remove('event');

            vm.event = {
                fileUnit: tempevent.fileUnit,
                videoLinks: [],
                simpleLinks: [],
                photoLinks: [],
                textLinks: []
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


        function submitVideo() {
            if (vm.currentVideo.name && vm.currentVideo.link) {
                vm.event.videoLinks.push(vm.currentVideo);
                vm.currentVideo = {};
            }
        }

        function submitSimpleLink() {
            if (vm.currentSimpleLink.name && vm.currentSimpleLink.link) {
                vm.event.simpleLinks.push(vm.currentSimpleLink);
                vm.currentSimpleLink = {};
            }
        }

        function submitPhoto() {
            if (vm.currentPhoto.name && vm.currentPhoto.link) {
                vm.event.photoLinks.push(vm.currentPhoto);
                vm.currentPhoto = {};
            }
        }

        function submitText() {
            if (vm.currentText.name && vm.currentText.description) {
                vm.event.textLinks.push(vm.currentText);
                vm.currentText = {};
            }
        }

        function getEventList() {
            $location.url('/apps/events');
        }


    }
})();