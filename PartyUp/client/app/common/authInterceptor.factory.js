﻿(function () {
    'use strict';
    angular
        .module('partyUp')
        .factory('AuthInterceptor', authInterceptor);
    authInterceptor.$inject = ['$location', '$q', 'AuthTokenService'];

    function authInterceptor($location, $q, AuthTokenService) {
        return {
            request: function (config) {
                var token = AuthTokenService.getToken();
                if (token) {
                    config.headers.Authorization = 'Bearer ' + token;
                }

                return config;
            },
            response: function (response) {
                return response;
            },
            responseError: function (rejection) {
                return $q.reject(rejection);
            }
        };
    }
})();