(function () {
    "use strict";

    angular
        .module("driveApp")
        .directive('fileName', ['FileService', '$q', function (fileService, $q) {
            return {
                require: 'ngModel',
                link: function (scope, elm, attrs, ctrl) {

                    var files = [];

                    fileService.getAllByParentId(attrs.spaceId, attrs.parentId, function (data) {
                        for (var i = 0; i < data.length; i++) {
                            files.push(data[i].name);
                        }              
                    });

                    ctrl.$asyncValidators.fileName = function (modelValue) {

                        if (ctrl.$isEmpty(modelValue)) {
                            return $q.when();
                        }

                        var def = $q.defer();

                        if (files.indexOf(modelValue) === -1) {
                            def.resolve();
                        } else {
                            if (attrs.fileName === modelValue)
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