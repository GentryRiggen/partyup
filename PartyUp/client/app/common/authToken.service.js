(function () {
    'use strict';
    angular.module('partyUp').service('AuthTokenService', authTokenService);

    authTokenService.$inject = ['$window', '$rootScope', '$q'];
    function authTokenService($window, $rootScope, $q) {
        var tokenKeyName = 'partyUp-Token';
        var storage = $window.localStorage;
        var authTokenSvc = {};

        authTokenSvc.getTokenAsync = function () {
            var dfd = $q.defer();

            if (!$rootScope.userToken)
                $rootScope.userToken = storage.getItem(tokenKeyName);

            dfd.resolve($rootScope.userToken);

            return dfd.promise;
        };

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

        $rootScope.$on('partyUp.user.logout', function () {
            storage.removeItem(tokenKeyName);
        });

        $rootScope.$on('partyUp.user.login', function (event, user) {
            authTokenSvc.setToken(user.token);
        });

        return authTokenSvc;
    }
})();