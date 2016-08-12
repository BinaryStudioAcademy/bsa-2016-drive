(function () {
    "use strict";

    angular
        .module("driveApp")
        .controller("SpaceController", SpaceController);

    SpaceController.$inject = ['SpaceService'];

    function SpaceController(spaceService) {
        var vm = this;

        activate();

        function activate() {
            spaceService.getSpace(1, function (data) {
                vm.space = data;
            });
        }
    }
}());