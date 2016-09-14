(function() {
    'use strict';

    angular
        .module('driveApp.academyPro')
        .controller('EventController', EventController);

    EventController.$inject = [
        'eventService'
    ];

    function EventController(eventService) {
        var vm = this;

        activate();

        function activate() {
            
        }
    }
})();