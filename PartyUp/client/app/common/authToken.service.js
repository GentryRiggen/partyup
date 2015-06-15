(function () {
    'use strict';
    angular.module('partyUp').service('AuthTokenService', authTokenService);

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

        authTokenSvc.setToken = function (token) {
            storage.setItem(tokenKeyName, token);
        };

        authTokenSvc.hasToken = function () {
            return !!authTokenSvc.getToken();
        };

        $rootScope.$on('partyup.user.logout', function () {
            storage.removeItem(tokenKeyName);
        });

        $rootScope.$on('partyup.user.login', function (event, user) {
            authTokenSvc.setToken(user.token);
        });

        return authTokenSvc;
    }
})();