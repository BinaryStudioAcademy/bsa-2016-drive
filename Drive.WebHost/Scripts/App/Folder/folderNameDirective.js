(function () {
    "use strict";

    angular
        .module("driveApp")
        .directive('folderName', ['FolderService', '$q', function (folderService, $q) {
            return {
                require: 'ngModel',
                link: function (scope, elm, attrs, ctrl) {

                    var folders = [];

                    folderService.getAllByParentId(attrs.spaceId, attrs.parentId, function (data) {
                        for (var i = 0; i < data.length; i++) {
                            folders.push(data[i].name);
                        }
                    });


                    ctrl.$asyncValidators.folderName = function (modelValue) {

                        if (ctrl.$isEmpty(modelValue)) {
                            return $q.when();
                        }

                        var def = $q.defer();

                        if (folders.indexOf(modelValue) === -1) {
                            def.resolve();
                        } else {
                            if (attrs.folderName === modelValue)
                                def.resolve();
                            else
                                def.reject();
                        }

                        return def.promise;
                    };
                }
            };
        }]);
}());