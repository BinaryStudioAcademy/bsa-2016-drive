(function() {
    'use strict';

    angular
        .module('driveApp.events')
        .controller('EventController', EventController);

    EventController.$inject = [
        'EventService',
        '$routeParams',
        '$sce',
        'ContentTypeService'
    ];

    function EventController(eventService, $routeParams, $sce, contentTypeService) {
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
        vm.update = update;
        
        activate();

        function activate() {
            vm.isEditing = false;
            vm.showEditArea = false;
            vm.sortedContentList = [];
            eventService.getEventTypes(function (data) {
                vm.eventTypes = data;
            });
            return getEvent();
        }

        function edit() {
            vm.isEditing = true;
        }

        function getEvent() {
            return eventService.getEvent(vm.currentEventId)
                .then(function (data) {
                    vm.event = data;
                    vm.sortedContentList = vm.event.contentList;
                    if (vm.sortedContentList.length) vm.sortedContentList.sort(function (a, b) {
                        return a.order - b.order;
                    });
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
            vm.showEditArea = true;
            //vm.event.contentList.splice(index, 1);
        }

        function updateEvent() {
            if (vm.event.fileUnit.name) {
                vm.update()
                    .then(function () {
                        return vm.getevent();
                    });
                toastr.success(
                'Event was successfully updated!', 'Events',
                {
                    closeButton: true, timeOut: 6000
                });
            }
        }

        function update() {
            if (vm.sortedContentList.length > 0)
                for (var i = 0; i < vm.sortedContentList.length; i++) {
                    vm.sortedContentList[i].order = i;
                }
            vm.event.contentList = vm.sortedContentList;
            return eventService.putData(vm.currentEventId, vm.event)
            .then(function () {
                vm.isEditing = !vm.isEditing;
            });
        }

        function trustSrc(src) {
            return $sce.trustAsResourceUrl(src);
        }
    }
})();