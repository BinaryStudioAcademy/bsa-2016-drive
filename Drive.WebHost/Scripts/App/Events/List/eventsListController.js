(function () {
    "use strict";

    angular.module('driveApp.events')
        .controller('EventsListController', EventsListController);


    EventsListController.$inject = [
        "EventsListService",
        '$location',
        'localStorageService',
        'EventService',
        '$uibModal'
    ];

    function EventsListController(EventsListService, $location, localStorageService, eventService, $uibModal) {
        var vm = this;
        vm.columnForOrder = 'name';
        vm.orderEventByColumn = orderEventByColumn;
        vm.openEvent = openEvent;
        vm.changeView = changeView;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.openShareEventContentWindow = openShareEventContentWindow;
        vm.sharedEventContent = sharedEventContent;
        vm.deleteEvent = deleteEvent;
        vm.eventMenuOptions = [
                    [
            'Share', function ($itemScope) {
                vm.contentSharedId = $itemScope.event.fileUnit.id;
                sharedEventContent();
            },
                function ($itemScope) {
                    if ($itemScope.event.fileUnit.spaceId == vm.binarySpaceId) {
                        return false;
                    }
                    return true;
                }
                    ],
                    null,
            [
                'Edit', function ($itemScope) {
                    $location.url('/apps/events/' + $itemScope.event.id + '/edit');
                }
            ],
            null,
            [
                'Delete', function ($itemScope) {
                    deleteEvent($itemScope.event.id);
                }
            ]
        ];

        activate();

        function activate() {
            vm.eventsList = [];
            var view = localStorageService.get('view')
            if (view == undefined) {
                vm.showTable = true;
                vm.showGrid = false;
                vm.view = "fa fa-th";
            }
            else if (view.showTable) {
                vm.showTable = true;
                vm.showGrid = false;
                vm.view = "fa fa-th";
            }
            else {
                vm.showGrid = true;
                vm.showTable = false;
                vm.view = "fa fa-list";
            }
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
                localStorageService.set('view', { showTable: false });
            } else {
                activateTableView();
                localStorageService.set('view', { showTable: true });
            }
        }

        function activateTableView() {
            vm.view = "fa fa-th";
            vm.showTable = true;
            vm.showGrid = false;
        }

        function activateGridView() {
            vm.view = "fa fa-list";
            vm.showTable = false;
            vm.showGrid = true;
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

        function deleteEvent(id) {
            EventsListService.deleteEvent(id, function () {
                return search();
            })
        }

        function sharedEventContent() {
            openShareEventContentWindow();
        }

        function openShareEventContentWindow(size) {

            var shareModalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'Scripts/App/SharedContent/SharedContentForm.html',
                windowTemplateUrl: 'Scripts/App/SharedContent/Modal.html',
                controller: 'SharedContentModalCtrl',
                controllerAs: 'sharedContentModalCtrl',
                size: size,
                resolve: {
                    items: function () {
                        var sharedContInfo = {
                            contentId: vm.contentSharedId,
                            title: 'Shared file'
                        }
                        return sharedContInfo;
                    }
                }
            });

            shareModalInstance.result.then(function (response) {
                console.log(response);
            }, function () {
                console.log('Modal dismissed');
            });
        }

        function orderEventByColumn(column) {
            vm.eventColumnForOrder = EventsListService.orderEventsByColumn(column, vm.eventColumnForOrder);
        }
    }
}());