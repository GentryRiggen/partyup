(function () {
    'use strict';

    angular.module('partyUp').config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$mdThemingProvider',
        function ($urlRouterProvider, $stateProvider, $httpProvider, $mdThemingProvider) {
            $urlRouterProvider.otherwise('/');

            $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/client/app/home/home.tmpl.html',
                controller: 'homeCtrl',
                controllerAs: 'ctrl'
            });

            //$httpProvider.interceptors.push('authInterceptor');
        }])

    .constant('API_URL', '/api/');
})();