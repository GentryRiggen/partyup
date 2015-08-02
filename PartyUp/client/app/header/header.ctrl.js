(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('HeaderCtrl', HeaderCtrl);

    HeaderCtrl.$inject = ['$scope', 'UserService', '$state', '$mdSidenav', '$mdDialog'];
    function HeaderCtrl($scope, UserService, $state, $mdSidenav, $mdDialog) {
        var Header = this;

        Header.currentUser = false;
        Header.showModerator = false;
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
                Header.currentUser = data;
                angular.forEach(Header.currentUser.roles, function (role) {
                    if (role === 'Admin' || role === 'Moderator') {
                        Header.showModerator = true;
                    }
                });
            } else {
                Header.currentUser = false;
                Header.showModerator = false;
            }
        }

        Header.navigate = function (state) {
            $mdSidenav('sideNav').close();
            $state.go(state);
        };

        Header.toggleNav = function () {
            $mdSidenav('sideNav').toggle();
        };

        $scope.$on('partyUp.user.login', function (event, data) {
            updatePermissions(data);
        });

        $scope.$on('partyUp.user.logout', function () {
            updatePermissions();
        });

        Header.title = 'Party Up';
        $scope.$on('partyUp.header.updateTitle', function (event, title) {
            if (angular.isDefined(title) && title !== '') {
                Header.title = title;
            }
        });

        Header.goBack = false;
        $scope.$on('partyUp.header.updateBack', function (event, goBackFunction) {
            if (angular.isDefined(goBackFunction)) {
                Header.goBack = goBackFunction;
            } else {
                Header.goBack = false;
            }
        });

        $scope.$on('partyup.hostedEvent', function(event, newEvent) {
            if (Header.currentUser) {
                Header.currentUser.recentlyHostedEvents.unshift(newEvent);
            }
        });

        $scope.$on('partyup.joinedEvent', function (event, newEvent) {
            if (Header.currentUser) {
                Header.currentUser.recentlyJoinedEvents.unshift(newEvent);
            }
        });

        $scope.$on('$stateChangeStart', function () {
            Header.goBack = false;
        });

        Header.hostEvent = function (event) {
            $mdSidenav('sideNav').close();
            var confirm = $mdDialog.confirm()
                .parent(angular.element(document.body))
                .title('Host Again?')
                .content('Would you like to host this event again?')
                .ariaLabel('Host Event Again')
                .ok('OK')
                .cancel('Cancel');
            $mdDialog.show(confirm).then(function () {
                $state.go('mission', {
                    communityId: event.mission.communityId,
                    missionId: event.mission.id,
                    host: true
                });
            });
        };

        Header.findEvents = function (event) {
            $mdSidenav('sideNav').close();
            $state.go('mission', { communityId: event.mission.communityId, missionId: event.mission.id });
        };

        checkUserAuth();
    }
})();