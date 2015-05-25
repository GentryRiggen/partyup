(function () {
    'use strict';
    angular
        .module('partyUp')
        .factory('authInterceptor', authInterceptor);
    authInterceptor.$inject = ['$location', '$q', 'authToken'];

    function authInterceptor($location, $q, authToken) {
        return {
            request: function (config) {
                var token = authToken.getToken();
                if (token)
                    config.headers.Authorization = 'Bearer ' + token;

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