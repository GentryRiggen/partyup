(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('HeaderCtrl', HeaderCtrl);

    HeaderCtrl.$inject = ['$scope', 'UserService', '$state', '$mdSidenav'];
    function HeaderCtrl($scope, UserService, $state, $mdSidenav) {
        var HeaderCtrl = this;
        
        HeaderCtrl.currentUser = false;
        HeaderCtrl.showModerator = false;
        function checkUserAuth() {
            UserService.getCurrentUser().then(
                function (user) {
                    updatePermissions(user);
                }, function () {
                    updatePermissions();
                }
            );
        }
        
        function updatePermissions(user) {
            if (user) {
                HeaderCtrl.currentUser = user;
                angular.forEach(HeaderCtrl.currentUser.roles, function(role) {
                    if (role == "Admin" || role == "Moderator") HeaderCtrl.showModerator = true;
                });
            } else {
                HeaderCtrl.currentUser = false;
                HeaderCtrl.showModerator = false;
            }
        }

        HeaderCtrl.navigate = function (state) {
            $mdSidenav("sideNav").close();
            $state.go(state);
        };

        HeaderCtrl.toggleNav = function () {
            $mdSidenav("sideNav").toggle();
        };

        $scope.$on('partyUp.user.login', function (event, authResponse) {
            updatePermissions(authResponse.user);
        });
        
        $scope.$on('partyUp.user.logout', function () {
            updatePermissions();
        });
        
        checkUserAuth();
    }
})();