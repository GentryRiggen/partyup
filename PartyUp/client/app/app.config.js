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
                    var userIsAdmin = _.contains(currentUser.roles, "Admin");
                    // Check if state requires user to be in certain role (Admin trumps everything)
                    if (!userIsAdmin &&
                        angular.isDefined(toState.data) &&
                        angular.isDefined(toState.data.allowedRoles) &&
                        toState.data.allowedRoles.length > 0) {

                        var allowedThrough = false;
                        angular.forEach(toState.data.allowedRoles, function (role) {
                            if (_.contains(currentUser.roles, role)) {
                                allowedThrough = true;
                            }
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

            $stateProvider
            .state('communities', {
                url: '/',
                templateUrl: '/client/app/community/communities.tmpl.html',
                controller: 'CommunitiesCtrl',
                controllerAs: 'CommunitiesCtrl'
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
            // ADMIN
            .state('admin', {
                url: '/admin',
                templateUrl: '/client/app/admin/admin.tmpl.html',
                controller: 'AdminCtrl',
                controllerAs: 'AdminCtrl',
                data: { allowedRoles: ['Admin', 'Moderator'] }
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
        }]);
})();