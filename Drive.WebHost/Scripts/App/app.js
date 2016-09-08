angular.module("driveApp",
    [
        "ngRoute",
        "ui.bootstrap.contextMenu",
        "ui.bootstrap",
        "angularUtils.directives.dirPagination",
        "LocalStorageModule", "ngLoadingSpinner",
        'ngAnimate',
        "toastr",
        "driveApp.academyPro",
        "cfp.hotkeys",
        "ang-drag-drop"
    ])
    .factory('BaseUrl', function () {
        return window.globalVars.baseUrl;
    })
    .config([
        "$routeProvider",
        function ($routeProvider) {
            var baseUrl = window.globalVars.baseUrl;
            $routeProvider
                .when("/", // Space page
                {
                    templateUrl: baseUrl + "/Scripts/App/Space/Space.html",
                    controller: "SpaceController",
                    controllerAs: "spaceCtrl",
                    redirectTo: "/binaryspace"
                })
                .when("/AdminPanel", // Admin Panel
                {
                    templateUrl: baseUrl + "/Scripts/App/AdminPanel/AdminPanel.html",
                    controller: "adminPanelController",
                    controllerAs: "adminCtrl"
                })
                .when("/sharedspace", // Shared Space page
                {
                    templateUrl: baseUrl + "/Scripts/App/SharedSpace/SharedSpace.html",
                    controller: "SharedSpaceController",
                    controllerAs: "sharedSpaceCtrl"
                })
                .when("/spaces/:id", // Space page
                {
                    templateUrl: baseUrl + "/Scripts/App/Space/Space.html",
                    controller: "SpaceController",
                    controllerAs: "spaceCtrl"
                })
                 .when("/spaces/:id/settings/", // Space settings Page
                {
                    templateUrl: baseUrl + "/Scripts/App/Space/Settings.html",
                    controller: "SettingsController",
                    controllerAs: "settingsCtrl"
                })
                .when("/AdminPanel/Logs", // Logs page
                {
                    templateUrl: baseUrl + "/Scripts/App/Logs/Logs.html",
                    controller: "LogsController",
                    controllerAs: "logsCtrl"
                })
                .when("/folders", // Folders page
                {
                    templateUrl: baseUrl + "/Scripts/App/Folder/Folder.html",
                    controller: "FolderController",
                    controllerAs: "folderCtrl"
                })
                .when("/apps/:appName", // Apps
                {
                    templateUrl: baseUrl + "/Scripts/App/FileFilter/FileFilter.html",
                    controller: "FileFilterController",
                    controllerAs: "fileFilterCtrl"
                })
                .when("/Error",
                {
                    templateUrl: baseUrl + "/Scripts/App/Error404/404.html",
                    controller: "ErrorController",
                    controllerAs: "errorCtrl"
                })
                .when("/trashbin",
                {
                    templateUrl: baseUrl + "/Scripts/App/TrashBin/TrashBin.html",
                    controller: "TrashBinController",
                    controllerAs: "trashBinCtrl"
                })
                .when("/:spaceType",
                {
                    templateUrl: baseUrl + "/Scripts/App/Space/Space.html",
                    controller: "SpaceController",
                    controllerAs: "spaceCtrl",
                    hotkeys: [
                        ['ctrl+x', 'Cut selected file', 'spaceCtrl.cutByHotkeys()'],
      ['ctrl+c', 'Copy selected file', 'spaceCtrl.copyByHotkeys()'],
      ['ctrl+v', 'Paste selected file', 'spaceCtrl.pasteByHotkeys()'],
      ['del', 'Delete selected file', 'spaceCtrl.deleteByHotkeys()']
                    ]
                })
                .otherwise({
                    // This is when any route not matched - error
                    redirectTo: "/Error"
                });
        }
    ]);
