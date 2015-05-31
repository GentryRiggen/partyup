(function () {
    'use strict';
    angular
        .module('partyUp')
        .factory('authInterceptor', authInterceptor);
    authInterceptor.$inject = ['$location', '$q', 'authTokenService'];

    function authInterceptor($location, $q, authTokenService) {
        return {
            request: function (config) {
                var token = authTokenService.getToken();
                if (token)
                    config.headers.Authorization = 'Bearer ' + token;

                console.log("Sending request with this config", config);
                return config;
            },
            response: function (response) {
                return response;
            },
            responseError: function (rejection) {
                if (rejection.status === 401) {
                    $location.path('login');
                }
                return $q.reject(rejection);
            }
        }
    }
})();