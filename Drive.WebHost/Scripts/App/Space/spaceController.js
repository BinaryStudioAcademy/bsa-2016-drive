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
            
            vm.listView = function () {
                angular.element(document.getElementsByClassName("item")).addClass("list-group-item");
                angular.element(document.getElementsByClassName("item")).removeClass("grid-group-item");
            };

            vm.gridView = function () {
                angular.element(document.getElementsByClassName("item")).addClass("grid-group-item");
                angular.element(document.getElementsByClassName("item")).removeClass("list-group-item");
            };
        }
    }
}());