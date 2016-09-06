(function () {
	"use strict";

	angular.module('driveApp.academyPro')
        .directive('lectureLinks', LectureLinks);

	LectureLinks.$inject = ['BaseUrl', 'LinkTypeService', '$sce'];

	function LectureLinks(baseUrl, linkTypeService, $sce) {
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

		function trustSrc(src) {
			return $sce.trustAsResourceUrl(src);
		}

		return directive;
	}
})();