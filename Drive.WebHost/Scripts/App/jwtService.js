(function() {
    'use strict';

    angular
        .module('driveApp')
        .factory('JwtService', JwtService);

    JwtService.$inject = [];

    function JwtService() {
        var service = {
            isAdmin: isAdmin,
            isInRole: isInRole,
            decodeToken: decodeToken
        };

        return service;

        function isAdmin() {
            return isInRole("admin");
        }

        function isInRole(role) {
            var cookieRole = getRole();
            if (cookieRole) return cookieRole === role.toLowerCase() ? true : false;
            return false;
        }

        function getRole() {
            var cookieToken = getCookie('x-access-token');
            var token;
            if (cookieToken) token = decodeToken(cookieToken);
            if (token) {
                return token.role.toLowerCase();
            }
            return null;
        }

        function getCookie(name) {
            var value = "; " + document.cookie;
            var parts = value.split("; " + name + "=");
            if (parts.length == 2) return parts.pop().split(";").shift();
        }

        function decodeToken(token) {
            var parts = token.split('.');

            if (parts.length !== 3) {
                throw new Error('JWT must have 3 parts');
            }

            var decoded = this.urlBase64Decode(parts[1]);
            if (!decoded) {
                throw new Error('Cannot decode the token');
            }

            return angular.fromJson(decoded);
        }
    }
})();