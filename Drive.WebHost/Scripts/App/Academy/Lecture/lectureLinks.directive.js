(function () {
	"use strict";

	angular.module('driveApp.academyPro')
        .directive('lectureLinks', LectureLinks);

	LectureLinks.$inject = ['BaseUrl'];

	function LectureLinks(baseUrl) {
		var directive = {
			restrict: 'A',
			scope: {
				iconClass: '@iconClass',
				blockTitle: '@blockTitle',
				links: '=linksModel',
				linkType: "=linkType",
				linkTypes: '=linkTypes',
				linkTrust: '&linkTrust'
			},
			templateUrl: baseUrl + '/Scripts/App/Academy/Lecture/lectureLinks.directive.html'
		}

		return directive;
	}
})();