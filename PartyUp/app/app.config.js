(function () {
    'use strict';

    var app = angular.module('partyUp');

    app.constant('API_URL', '/api');

    // Handle Auth on every navigation
    app.run(function ($rootScope, userService, $state) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            var requireLogin = toState.data.requireLogin;
            if (requireLogin) {
                userService.getCurrentUser().then(
                    function () {
                        // Do nothing for now. Maybe some logging later
                    },
                    function () {
                        // Do not route
                        event.preventDefault();
                        // Route to login
                        $state.go('login');
                    }
                );
               
            }
        });
    });
    
    app.config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$mdThemingProvider',
        function ($urlRouterProvider, $stateProvider, $httpProvider, $mdThemingProvider) {
            // Default route
            $urlRouterProvider.otherwise('/');

            $stateProvider
            .state('welcome', {
                url: '/welcome',
                templateUrl: '/app/welcome/welocme.tmpl.html',
                controller: 'welcomeCtrl',
                controllerAs: 'welcomeCtrl',
                data: {
                    requireLogin: false
                }
            })
            .state('login', {
                url: '/login',
                templateUrl: '/app/login/login.tmpl.html',
                controller: 'loginCtrl',
                controllerAs: 'loginCtrl',
                data: {
                    requireLogin: false
                }
            })
            .state('home', {
                url: '/',
                templateUrl: '/app/home/home.tmpl.html',
                controller: 'homeCtrl',
                controllerAs: 'homeCtrl',
                data: {
                    requireLogin: true
                }
            });

            $httpProvider.interceptors.push('authInterceptor');
        }]);
})();