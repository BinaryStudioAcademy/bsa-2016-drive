(function () {
    "use strict";

    angular.module("driveApp")
        .controller("SettingsController", SettingsController);

    SettingsController.$inject = ['SettingsService'];

    function SettingsController(settingsService) {
        var vm = this;

        activate();

        function activate() {
            vm.title = settingsService.getTitle();
            vm.spaceName = settingsService.getSpaceName();
            vm.description = settingsService.getDescription();
            vm.maxFilesQuantity = settingsService.getMaxFilesQuantity();
            vm.maxFileSize = settingsService.getMaxFileSize();
            vm.users = settingsService.getUsers();
            vm.spaceUsers = settingsService.getSpaceUsers();
        }
    }
}());