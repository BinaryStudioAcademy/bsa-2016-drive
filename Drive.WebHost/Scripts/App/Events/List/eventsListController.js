(function () {
    "use strict";

    angular.module('driveApp.events')
        .controller('EventsListController', EventsListController);


    EventsListController.$inject = [
        "EventsListService",
        '$location'
    ];

    function EventsListController(EventsListService, $location) {
        var vm = this;
        vm.columnForOrder = 'name';
        vm.openEvent = openEvent;
        vm.changeView = changeView;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
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
            vm.icon = "./Content/Icons/event.svg";
            search();
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

        function changeView(view) {
            if (view === "fa fa-th") {
                activateGridView();
            } else {
                activateTableView();
            }
        }

        function activateTableView() {
            vm.view = "fa fa-th";
            vm.showTable = true;
        }

        function activateGridView() {
            vm.view = "fa fa-list";
            vm.showTable = false;
        }

        function search() {
            EventsListService.searchEvents(vm.searchText, function (data) {
                vm.eventsList = data;
                getBinarySpaceIdent(vm.eventsList);
            });
        }

        function cancelSearch() {
            vm.searchText = "";
            search();
        }

        function getBinarySpaceIdent(list) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].spaceType === 0) {
                    vm.binarySpaceId = list[i].spaceId;
                    return vm.binarySpaceId;
                }
            }
            return 0;
        }
    }
}());