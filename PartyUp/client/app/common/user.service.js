(function () {
    'use strict';
    angular.module('partyUp').service('UserService', userService);

    userService.$inject = ['API_URL', '$window', '$http', '$rootScope', '$q', '$mdDialog'];
    function userService(API_URL, $window, $http, $rootScope, $q, $mdDialog) {
        var userSvc = {};

        userSvc.getCurrentUser = function () {
            var deferred = $q.defer();
            
            if (angular.isUndefined($rootScope.currentUser)) {
                $http.get(API_URL + "/auth/user").then(
                    function (userResponse) {
                        $rootScope.currentUser = userResponse.data;
                        deferred.resolve($rootScope.currentUser);
                    },
                    function (resp) {
                        deferred.reject();
                    });
            } else {
                deferred.resolve($rootScope.currentUser);
            }

            return deferred.promise;
        };

        userSvc.logout = function () {
            // Clear out in mem items
            $rootScope.$broadcast('partyUp.user.logout');
            $rootScope.currentUser = undefined;
        };

        userSvc.login = function (username, password) {
            var deferred = $q.defer();
            $http.post(API_URL + "/auth", { username: username, password: password }).then(
                function (authResponse) {
                    console.log(authResponse.data);
                    $rootScope.currentUser = authResponse.data;
                    $rootScope.$broadcast('partyUp.user.login', authResponse.data);
                    deferred.resolve(authResponse.data);
                },
                function (resp) {
                    deferred.reject("Invalid Login Attempt");
                });

            return deferred.promise;
        };

        userSvc.isAuthenticated = function () {
            return angular.isDefined($rootScope.currentUser);
        },

        userSvc.userHasAccess = function (requestedRole) {
            // Search current claims
            angular.forEach($rootScope.currentUser.roles, function (role) {
                if (role === requestedRole)
                    return true;
            });

            // If we get this far, their claim is invalid
            return false;
        };

        userSvc.register = function (user) {
            return $http.post(API_URL + '/auth/register', user).then(
                function (authResponse) {
                    $rootScope.currentUser = authResponse.data;
                    $rootScope.$broadcast('partyUp.user.login', authResponse.data);
                });
        };

        userSvc.updateUser = function (user) {
            return $http.put(API_URL + '/auth/update', user);
        };

        userSvc.checkUsername = function (username, loggedIn) {
            if (angular.isDefined(loggedIn) && loggedIn == true)
                return $http.get(API_URL + '/auth/checkusername/loggedin?username=' + username);
            else
                return $http.get(API_URL + '/auth/checkusername?username=' + username);
        };

        userSvc.checkEmail = function (email, loggedIn) {
            if (angular.isDefined(loggedIn) && loggedIn == true)
                return $http.get(API_URL + '/auth/checkemail/loggedin?email=' + email);
            else
                return $http.get(API_URL + '/auth/checkemail?email=' + email);
        };

        userSvc.showLoginModal = function (event) {
            return $mdDialog.show({
                controller: function($scope, $mdDialog, $state) {
                    $scope.user = {
                        username: '',
                        password: ''
                    };
                    
                    $scope.cancel = function() {
                        $mdDialog.cancel();
                    };
                    
                    $scope.submit = function() {
                        userSvc.login($scope.user.username, $scope.user.password).then(
                            function(resp) {
                                $mdDialog.hide(resp);
                            }, function() {
                                $scope.cancel();
                            });
                    };
                    
                    $scope.createAccount = function() {
                        $state.go('register');
                        $scope.cancel();
                    };
                },
                templateUrl: '/client/app/common/loginModal.tmpl.html',
                parent: angular.element(document.body),
                targetEvent: event,
                clickOutsideToClose: true
            });
        };

        return userSvc;
    }
})();