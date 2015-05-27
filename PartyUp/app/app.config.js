(function () {
    'use strict';

    angular.module('partyUp').config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$mdThemingProvider',
        function ($urlRouterProvider, $stateProvider, $httpProvider, $mdThemingProvider) {
            $urlRouterProvider.otherwise('/');

            $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/app/home/home.tmpl.html',
                controller: 'homeCtrl',
                controllerAs: 'homeCtrl'
            })
            .state('login', {
                url: '/login',
                templateUrl: '/app/login/login.tmpl.html',
                controller: 'loginCtrl',
                controllerAs: 'loginCtrl'
            });

            //$httpProvider.interceptors.push('authInterceptor');
        }])

    .constant('API_URL', '/api');


    //.value('chat', $.connection.chat);

    //$(function () {
    //    $.connection.hub.logging = true;
    //    $.connection.hub.start();
    //});
})();