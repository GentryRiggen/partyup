(function () {
    'use strict';

    var app = angular.module('partyUp');

    app.constant('API_URL', '/api');

    // Handle Auth on every navigation
    app.run(function ($rootScope, $state, UserService, $window, $location) {
        $rootScope.$on('$viewContentLoaded', function (event) {
            // console.log("Updating google analytics with page view", $location.url());
            // $window.ga('send', 'pageview', { page: $location.url() });
        });

        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            if (angular.isDefined(toState.data) &&
                angular.isDefined(toState.data.requireLogin) &&
                toState.data.requireLogin === false) {
                return;
            }

            UserService.getCurrentUser().then(
                function (currentUser) {
                    var userIsAdmin = false;
                    angular.forEach(currentUser.roles, function (role) {
                        if (role == 'Admin') {
                            userIsAdmin = true;
                        }
                    });
                    // Check if state requires user to be in certain role (Admin trumps everything)
                    if (!userIsAdmin &&
                        angular.isDefined(toState.data) &&
                        angular.isDefined(toState.data.allowedRoles) &&
                        toState.data.allowedRoles.length > 0) {

                        var allowedThrough = false;
                        angular.forEach(toState.data.allowedRoles, function (role) {
                            angular.forEach(currentUser.roles, function (userRole) {
                                if (role == userRole) {
                                    allowedThrough = true;
                                }
                            });
                        });

                        if (!allowedThrough) {
                            // If we get this far, they don't have access
                            event.preventDefault();
                            $state.go('login');
                        }
                    }
                },
                function () {
                    event.preventDefault();
                    // Route to login
                    $state.go('login');
                }
                );
        });
    });

    app.config(['$urlRouterProvider', '$stateProvider', '$httpProvider', '$mdThemingProvider',
        function ($urlRouterProvider, $stateProvider, $httpProvider, $mdThemingProvider) {
            // Default route
            $urlRouterProvider.otherwise('/');

            // FRONT END
            $stateProvider
                .state('about', {
                url: '/about',
                templateUrl: '/client/app/about/about.tmpl.html',
                controller: 'AboutCtrl',
                controllerAs: 'AboutCtrl',
                data: { requireLogin: false }
            })
                .state('communities', {
                url: '/',
                templateUrl: '/client/app/community/communities.tmpl.html',
                controller: 'CommunitiesCtrl',
                controllerAs: 'CommunitiesCtrl',
                data: { requireLogin: false }
            })
                .state('community', {
                url: '/community/{communityId}',
                templateUrl: '/client/app/community/community.tmpl.html',
                controller: 'CommunityCtrl',
                controllerAs: 'CommunityCtrl',
                data: { requireLogin: false }
            })
                .state('mission', {
                url: '/community/{communityId}/missions/{missionId}',
                templateUrl: '/client/app/mission/mission.tmpl.html',
                controller: 'MissionCtrl',
                controllerAs: 'MissionCtrl',
                data: { requireLogin: false }
            })
                .state('event', {
                url: '/communities/{communityId}/missions/{missionId}/event/{eventId}',
                templateUrl: '/client/app/event/event.tmpl.html',
                controller: 'EventCtrl',
                controllerAs: 'EventCtrl',
                data: { requireLogin: true }
            })
            
            // LOGIN/LOGOUT
                .state('login', {
                url: '/login',
                templateUrl: '/client/app/login/login.tmpl.html',
                controller: 'LoginCtrl',
                controllerAs: 'LoginCtrl',
                data: { requireLogin: false }
            })
                .state('logout', {
                url: '/logout',
                controller: 'LogoutCtrl',
                data: { requireLogin: false }
            })
            // REGISTER
                .state('register', {
                url: '/register',
                templateUrl: '/client/app/register/register.tmpl.html',
                controller: 'RegisterCtrl',
                controllerAs: 'RegisterCtrl',
                data: { requireLogin: false }
            })
            // ACCOUNT
                .state('account', {
                url: '/account',
                templateUrl: '/client/app/account/account.tmpl.html',
                controller: 'AccountCtrl',
                controllerAs: 'AccountCtrl',
                data: { requireLogin: true }
            })
            // ADMIN
                .state('admin', {
                url: '/admin',
                templateUrl: '/client/app/admin/admin.tmpl.html',
                controller: 'AdminCtrl',
                controllerAs: 'AdminCtrl',
                data: { allowedRoles: ['Moderator'] }
            })
            // COMMUNITIES
                .state('admin.communities', {
                url: '/communities',
                templateUrl: '/client/app/community/communities.admin.tmpl.html',
                controller: 'CommunitiesAdminCtrl',
                controllerAs: 'CommunitiesAdminCtrl',
            })
                .state('admin.community', {
                url: '/communities/{communityId}',
                templateUrl: '/client/app/community/community.admin.tmpl.html',
                controller: 'CommunityAdminCtrl',
                controllerAs: 'CommunityAdminCtrl',
            })
            // MISSIONS
                .state('admin.mission', {
                url: '/communities/{communityId}/missions/{missionId}',
                templateUrl: '/client/app/mission/mission.admin.tmpl.html',
                controller: 'MissionAdminCtrl',
                controllerAs: 'MissionAdminCtrl',
            })
            ;
            $httpProvider.interceptors.push('AuthInterceptor');

            $mdThemingProvider.definePalette('primary', {
                "50": "#e6eaed",
                "100": "#b5c1c8",
                "200": "#8397a4",
                "300": "#5a7486",
                "400": "#305267",
                "500": "#072f49",
                "600": "#062940",
                "700": "#052337",
                "800": "#041d2e",
                "900": "#041825",
                "A100": "#b5c1c8",
                "A200": "#8397a4",
                "A400": "#305267",
                "A700": "#052337"
            });

            $mdThemingProvider.theme('default')
                .primaryPalette('primary');
        }]);
})();