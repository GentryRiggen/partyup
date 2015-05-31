(function () {
    'use strict';
    angular.module('partyUp').service('userService', userService);

    userService.$inject = ['API_URL', '$window', '$http', '$rootScope', '$q'];
    function userService(API_URL, $window, $http, $rootScope, $q) {
        var tokenKeyName = 'partyUp-Token';
        var storage = $window.localStorage;

        var userSvc = {};

        userSvc.getCurrentUser = function () {
            var deferred = $q.defer();
            if (angular.isUndefined($rootScope.currentUser)) {
                console.debug("User info not in memory. Making server request for user info.");
                $http.get(API_URL + "/auth/user").then(
                    function (userResponse) {
                        //console.debug("Successfully recieved user info", userResponse);
                        $rootScope.currentUser = userResponse.data.user;
                        $rootScope.currentUser.roles = userResponse.data.roles;
                        deferred.resolve($rootScope.currentUser);
                    },
                    function (resp) {
                        console.debug("Unsuccessfully recieved user info", resp);
                        deferred.reject();
                    }
                );
            } else {
                console.debug("User info in memory", $rootScope.currentUser);
                deferred.resolve($rootScope.currentUser);
            }

            return deferred.promise;
        };

        userSvc.getToken = function () {
            if (!$rootScope.userToken)
                $rootScope.userToken = storage.getItem(tokenKeyName);

            return $rootScope.userToken;
        };

        userSvc.logout = function () {
            // Clear out in mem items
            $rootScope.userToken = undefined;
            $rootScope.currentUser = undefined;
            $rootScope.currentUser.claims = undefined;

            // Clear out local storage
            storage.removeItem(tokenKeyName);
        };

        userSvc.login = function (username, password) {
            var deferred = $q.defer();
            $http.post(API_URL + "/auth", { username: username, password: password }).then(
                function (authResponse) {
                    console.debug("Successful Login Attempt", authResponse.data);
                    // Set token on scope and store it
                    $rootScope.userToken = authResponse.data.token;
                    storage.setItem(tokenKeyName, $rootScope.userToken);

                    // Set user info on scope
                    $rootScope.currentUser = authResponse.data.user;
                    $rootScope.currentUser.roles = authResponse.data.roles;

                    deferred.resolve(authResponse.data);
                },
                function (resp) {
                    console.debug("Unsuccessful Login Attempt");
                    deferred.reject("Invalid Login Attempt");
                });

            return deferred.promise;
        };

        userSvc.isAuthenticated = function () {
            return angular.isDefined($rootScope.currentUser);
        },

        userSvc.userHasAccess = function (requestedRole) {
            if (!userSvc.isAuthenticated) return false;

            // Search current claims
            angular.forEach($rootScope.currentUser.roles, function (role) {
                if (role === requestedRole)
                    return true;
            });

            // If we get this far, their claim is invalid
            return false;
        };

        return userSvc;
    }
})();