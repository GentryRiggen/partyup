(function () {
    'use strict';
    angular.module('partyUp').service('authTokenService', authTokenService);

    authTokenService.$inject = ['$window', '$rootScope'];
    function authTokenService($window, $rootScope) {
        var tokenKeyName = 'partyUp-Token';
        var storage = $window.localStorage;
        var authTokenSvc = {};

        authTokenSvc.getToken = function () {
            if (!$rootScope.userToken)
                $rootScope.userToken = storage.getItem(tokenKeyName);

            return $rootScope.userToken;
        };

        authTokenSvc.hasToken = function () {
            return !!authTokenSvc.getToken();
        };

        return authTokenSvc;
    }
})();