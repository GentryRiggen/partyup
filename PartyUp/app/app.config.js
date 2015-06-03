(function () {
    'use strict';

    var app = angular.module('partyUp');

    app.constant('API_URL', '/api');

    // Handle Auth on every navigation
    app.run(function ($rootScope, $state, userService) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            userService.getCurrentUser().then(
                    function (currentUser) {
                        var userIsAdmin = _.contains(currentUser.roles, "Admin");
                        // Check if state requires user to be in certain role (Admin trumps everything)
                        if (!userIsAdmin &&
                            angular.isDefined(toState.data) &&
                            angular.isDefined(toState.data.allowedRoles) &&
                            toState.data.allowedRoles.length > 0) {

                            var allowedThrough = false;
                            angular.forEach(toState.data.allowedRoles, function (role) {
                                if (_.contains(currentUser.roles, role)) {
                                    console.log("User has required role", toState.data.allowedRoles);
                                    allowedThrough = true;
                                }
                            });
                            
                            if (!allowedThrough) {
                                console.log("User not in role", currentUser.roles, toState.data.allowedRoles);
                                // If we get this far, they don't have access
                                $state.go('login');
                            }
                        }
                    },
                    function () {
                        // This may be a public route
                        if (angular.isDefined(toState.data) &&
                            angular.isDefined(toState.data.requireLogin) &&
                            toState.data.requireLogin === false) {
                            console.log("Public route, it's cool");
                            return;
                        } else {
                            console.log("User not logged in");
                            // Do not route
                            event.preventDefault();
                            // Route to login
                            $state.go('login');
                        }
                    }
                );
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
                controllerAs: 'welcomeCtrl'
            })
            .state('login', {
                url: '/login',
                templateUrl: '/app/login/login.tmpl.html',
                controller: 'loginCtrl',
                controllerAs: 'loginCtrl'
            })
            .state('home', {
                url: '/',
                templateUrl: '/app/home/home.tmpl.html',
                controller: 'homeCtrl',
                controllerAs: 'homeCtrl',
                data: {
                    allowedRoles: ["Pro"]
                }
            });

            $httpProvider.interceptors.push('authInterceptor');
        }]);
})();