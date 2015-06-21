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
        
        function updatePermissions(data) {
            if (angular.isDefined(data)) {
                HeaderCtrl.currentUser = angular.isDefined(data.user) ? data.user : data;
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

        $scope.$on('partyUp.user.login', function (event, data) {
            updatePermissions(data.user);
        });
        
        $scope.$on('partyUp.user.logout', function () {
            updatePermissions();
        });
        
        HeaderCtrl.title = "Party Up";
        $scope.$on('partyUp.header.updateTitle', function(event, title) {
           if (angular.isDefined(title) && title != "") {
               HeaderCtrl.title = title;
           } 
        });
        
        HeaderCtrl.goBack = false;
        $scope.$on('partyUp.header.updateBack', function(event, goBackFunction) {
           if (angular.isDefined(goBackFunction)) {
               HeaderCtrl.goBack = goBackFunction;
           } else {
               HeaderCtrl.goBack = false;
           }
        });
        
        $scope.$on('$stateChangeStart', function() {
            HeaderCtrl.goBack = false;
        });
        
        checkUserAuth();
    }
})();