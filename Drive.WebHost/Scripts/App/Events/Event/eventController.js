(function() {
    'use strict';

    angular
        .module('driveApp.events')
        .controller('EventController', EventController);

    EventController.$inject = [
        'EventService',
        '$routeParams',
        '$sce',
        'ContentTypeService',
        '$location',
        'toastr',
        'localStorageService',
    ];

    function EventController(eventService, $routeParams, $sce, contentTypeService, $location, toastr, localStorageService) {
        var vm = this;

        vm.currentEventId = $routeParams.id;
        vm.event = {};
        vm.getEvent = getEvent;
        vm.trustSrc = trustSrc;
        vm.contentTypes = contentTypeService;
        vm.edit = edit;
        vm.newVideo = newVideo;
        vm.newSimpleLink = newSimpleLink;
        vm.newPhoto = newPhoto;
        vm.newText = newText;
        vm.contentSaved = contentSaved;
        vm.removeContent = removeContent;
        vm.editContent = editContent;
        vm.updateEvent = updateEvent;
        vm.cancelEvent = cancelEvent;
        vm.update = update;
        vm.getEventsList = getEventsList;
        vm.changeCollapseState = changeCollapseState;
        vm.redirectTo = redirectTo;

        activate();

        function activate() {
            vm.isCollapsed = false;
            vm.isEditing = false;
            vm.isLinkExists = false;
            if ($location.path().indexOf('edit') + 1) {
                edit();
            }
            vm.showEditArea = false;
            vm.sortedContentList = [];
            vm.returnPath = '/apps/events/';

            vm.calendar = {
                isOpen: false,
                openCalendar: openCalendar,
                timepickerOptions: {
                    showMeridian: false
                }
            };
            return getEvent();
        }

        function edit() {
            vm.isEditing = true;
        }

        function getEvent() {
            return eventService.getEvent(vm.currentEventId)
                .then(function(data) {
                    vm.event = data;
                    vm.sortedContentList = vm.event.contentList;
                    if (vm.sortedContentList.length) {
                        vm.sortedContentList.sort(function (a, b) {
                            return a.order - b.order;
                        });
                        for (var i = 0; i < vm.sortedContentList.length; i++) {
                            if (vm.sortedContentList[i].contentType == 4)
                            {
                                vm.isLinkExists = true;
                                break;
                            }
                        }
                    }
                    return vm.event;
                });
        }

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

        function updateEvent() {
            if (vm.event.fileUnit.name) {
                vm.update()
                    .then(function() {
                        return vm.getEvent();
                    });
                toastr.success(
                    'Event was successfully updated!',
                    'Events',
                    {
                        closeButton: true,
                        timeOut: 6000
                    });
                redirectTo();
            }
        }

        function update() {
            if (vm.sortedContentList.length > 0)
                for (var i = 0; i < vm.sortedContentList.length; i++) {
                    vm.sortedContentList[i].order = i;
                }
            vm.event.contentList = vm.sortedContentList;
            return eventService.putData(vm.currentEventId, vm.event)
                .then(function() {
                    vm.isEditing = !vm.isEditing;
                });
        }

        function trustSrc(src) {
            return $sce.trustAsResourceUrl(src);
        }

        function cancelEvent() {
            vm.isEditing = false;
            redirectTo();
        }

        function getEventsList() {
            $location.url('/apps/events/');
        }

        function openCalendar(e) {
            e.preventDefault();
            e.stopPropagation();

            vm.calendar.isOpen = true;
        };

        function redirectTo() {
            var container = localStorageService.get('container');
            switch (container) {
                case 'space':
                    vm.returnPath = localStorageService.get('location');
                    break;
                case 'shared':
                    vm.returnPath = '/sharedspace';
                    break;
                default:
                    vm.returnPath = '/apps/events/';
                    break;
            }
            localStorageService.remove('container')
            $location.url(vm.returnPath);
            
        }
    }
})();