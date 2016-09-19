(function () {
	"use strict";

    angular.module('driveApp.events')
        .directive('eventContent', EventContent);

	EventContent.$inject = [
        'BaseUrl'
	];

	function EventContent(baseUrl) {
		var directive = {
			restrict: 'A',
			scope: {
			    content: '=content',
				contentTypes: '=contentTypes',
				linkTrust: '&linkTrust'
			},
			templateUrl: baseUrl + '/Scripts/App/Events/Event/eventContent.directive.html'
		}

		return directive;
	}
})();