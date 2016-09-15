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
        
        activate();

        function activate() {
            vm.sortedContentList = [];
            return getEvent();
        }

        function getEvent() {
            return eventService.getEvent(vm.currentEventId)
                .then(function (data) {
                    vm.event = data;
                    if (vm.event.contentVideoLinks) vm.sortedContentList = vm.sortedContentList.concat(vm.event.contentVideoLinks);
                    if (vm.event.contentSimpleLinks) vm.sortedContentList = vm.sortedContentList.concat(vm.event.contentSimpleLinks);
                    if (vm.event.contentPhotos) vm.sortedContentList = vm.sortedContentList.concat(vm.event.contentPhotos);
                    if (vm.event.contentTexts) vm.sortedContentList = vm.sortedContentList.concat(vm.event.contentTexts);
                    if (vm.sortedContentList.length) vm.sortedContentList.sort(function (a, b) {
                        return a.order - b.order;
                    });
                    return vm.event;
                });
        }

        function trustSrc(src) {
            return $sce.trustAsResourceUrl(src);
        }
    }
})();