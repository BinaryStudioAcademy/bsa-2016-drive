(function () {
    "use strict";

    angular.module('driveApp.events')
        .controller('EventsListController', EventsListController);


    EventsListController.$inject = [
        "EventsListService"
    ];

    function EventsListController(EventsListService) {
        var vm = this;
        vm.columnForOrder = 'name';
        vm.openEvent = openEvent;
        //vm.changeView = changeView;
        //vm.openNewEventWindow = openNewEventWindow;
        //vm.deleteEvent = deleteEvent;
        //vm.createNewEvent = createNewEvent;
        //vm.orderEventByColumn = orderEventByColumn;
        vm.eventMenuOptions = [
            [
                'Edit', function ($itemScope) {
                    vm.events = $itemScope.events;
                    vm.openNewEventWindow();
                }
            ],
            null,
            [
                'Delete', function ($itemScope) {
                    deleteEvent($itemScope.events.id);
                }
            ]
        ];

        activate();

        function activate() {
            vm.eventsList = [];
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
            vm.eventColumnForOrder = 'name';
            vm.iconHeight = 30;
            vm.icon = "./Content/Icons/academyPro.svg";
            getEvents();
        }

        function getEvents() {
            return EventsListService.getEvents()
            .then(function (data) {
                vm.eventsList = data;
                return vm.eventsList;
            });
        }

        function openEvent(id) {
            $location.url('/apps/events/' + id);
        }
    }
}());