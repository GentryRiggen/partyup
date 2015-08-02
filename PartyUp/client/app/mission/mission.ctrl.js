(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('MissionCtrl', MissionCtrl);

    MissionCtrl.$inject = ['$timeout', 'UserService', '$scope', 'MissionsService',
        'EventsService', 'AlertService', 'SignalRService', '$stateParams', '$state',
        '$mdDialog', 'moment'];
    /* jshint -W072 */
    function MissionCtrl($timeout, UserService, $scope, MissionsService,
        EventsService, AlertService, SignalRService, $stateParams, $state, $mdDialog, moment) {
        var Mission = this;
        Mission.events = false;
        Mission.allowHosting = false;
        Mission.play = true;
        var eventsHub = SignalRService.getHub('events');

        function init() {
            AlertService.updateTitle('Mission...');
            AlertService.updateGoBack(goBackToCommunity);
            AlertService.showLoading('Fetching mission...');
            MissionsService.getById($stateParams.missionId).then(gotMissionFromServer, function () {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                });
        }

        function gotMissionFromServer(resp) {
            Mission.mission = resp.data;
            AlertService.hideLoading();
            AlertService.updateTitle(Mission.mission.name);
            EventsService.getAllByMission($stateParams.missionId).then(gotMissionEventsFromServer, function () {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                });
        }

        function gotMissionEventsFromServer(resp) {
            Mission.events = resp.data;
            updateEventTimes();
            SignalRService.startConnection().then(function () {
                Mission.allowHosting = true;
                eventsHub.server.joinMissionGroup($stateParams.missionId);
                if (angular.isDefined($stateParams.host) && $stateParams.host === 'true') {
                    Mission.hostEvent();
                }
            });
        }
        
        // Define client functions first
        var eventsWhilePaused = [];
        eventsHub.client.newHostedEvent = function (event) {
            // Only add new ones if paused
            if (!Mission.play) {
                eventsWhilePaused.push(event);
                return;
            }

            var found = false;
            for (var i = 0; i < Mission.events.length; i++) {
                if (Mission.events[i].id === event.id) {
                    found = true;
                    break;
                }
            }
            if (!found && Mission.events !== false) {
                Mission.events.unshift(event);
                updateEventTimes(true);
            }
        };

        function updateEventTimes(runApply) {
            angular.forEach(Mission.events, function (event) {
                event.timeAgo = moment.utc(event.createdOn).fromNow();
            });
            if (runApply) {
                $scope.$apply();
            }
        }

        Mission.togglePlay = function () {
            Mission.play = !Mission.play;
            if (Mission.play) {
                // Add any while paused
                for (var i = eventsWhilePaused.length - 1; i >= 0; i--) {
                    Mission.events.push(eventsWhilePaused[i]);
                    if (i === 0) {
                        eventsWhilePaused = [];
                    }
                }
                updateEventTimes(false);
            }
        };

        eventsHub.client.removeEvent = function (event) {
            // Remove from current Events
            var currentMissoinsIndex = -1;
            for (var eventIndex = 0; eventIndex < Mission.events.length; eventIndex++) {
                if (Mission.events[eventIndex].id === event.id) {
                    currentMissoinsIndex = eventIndex;
                    break;
                }
            }
            if (currentMissoinsIndex !== -1) {
                Mission.events[currentMissoinsIndex].closed = true;
                $scope.$apply();
                $timeout(function () {
                    Mission.events.splice(currentMissoinsIndex, 1);
                    $scope.$apply();

                }, 2000);
            }
            
            // Remove From Paused Events
            var pausedMissionsIndex = -1;
            for (var i = 0; i < eventsWhilePaused.length; i++) {
                if (eventsWhilePaused[i].id === event.id) {
                    pausedMissionsIndex = i;
                    break;
                }
            }
            if (pausedMissionsIndex !== -1) {
                eventsWhilePaused.splice(currentMissoinsIndex, 1);
            }
        };

        eventsHub.client.updateLookingForCount = function (event, newCount) {
            for (var eventIndex = 0; eventIndex < Mission.events.length; eventIndex++) {
                if (Mission.events[eventIndex].id === event.id) {
                    Mission.events[eventIndex].DesiredAmount = newCount;
                    $scope.$apply();
                    break;
                }
            }
            
            for (var pausedEventIndex = 0; pausedEventIndex < eventsWhilePaused.length; pausedEventIndex++) {
                if (eventsWhilePaused[pausedEventIndex].id === event.id) {
                    eventsWhilePaused[pausedEventIndex].DesiredAmount = newCount;
                    $scope.$apply();
                    break;
                }
            }
        };

        eventsHub.client.eventCreated = function (event) {
            AlertService.showAlert('success', 'Created', 'Event Created!');
            goToEvent(event);
        };

        Mission.hostEvent = function (event) {
            // See if the user needs to be logged in
            UserService.getCurrentUser().then(
                function() {
                    showNewEventModal(event);
                },
                function () {
                    UserService.showLoginModal(event).then(
                        function () {
                            window.location.reload();
                        });
                });
        };

        Mission.platforms = ['All Platforms', 'Xbox 360', 'Xbox One', 'Playstation 3', 'Playstation 4'];
        Mission.platform = Mission.platforms[0];
        Mission.platformFilter = '*';
        Mission.filterPlatform = function (platform) {
            if (platform === 'All Platforms') {
                Mission.platformFilter = '*';
            } else {
                Mission.platformFilter = platform;
            }
        };

        function showNewEventModal(event) {
            $mdDialog.show({
                controller: eventDetailsModal,
                templateUrl: 'amountModal.html',
                parent: angular.element(document.body),
                targetEvent: event,
            }).then(function (event) {
                createNewHostedEvent(event);
            });
        }

        function eventDetailsModal($scope, $mdDialog) {
            $scope.platform = 'Xbox One';
            $scope.platforms = ['Xbox 360', 'Xbox One', 'Playstation 3', 'Playstation 4'];
            $scope.amount = 1;
            $scope.possibleAmounts = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
            $scope.language = 'English';
            $scope.languages = [
                'English',
                'Spanish',
                'French',
                'German'
            ];
            $scope.mic = true;
            $scope.notes = '';
            $scope.cancel = function () {
                $mdDialog.cancel();
            };
            $scope.complete = function () {
                $mdDialog.hide({
                    platform: $scope.platform,
                    desiredAmount: $scope.amount ? $scope.amount : 1, 
                    language: $scope.language ? $scope.language : $scope.languages[0], 
                    hasMic: $scope.mic,
                    notes: $scope.notes
                });
            };
        }

        function createNewHostedEvent(eventDetails) {
            if (Mission.allowHosting) {
                eventsHub.server.hostEvent($stateParams.missionId, eventDetails);
            } else {
                AlertService.showAlert('warning', 'Just a sec', 'Still establishing a connection');
            }
        }

        Mission.joinEvent = function (event) {
            UserService.getCurrentUser().then(
                function (user) {
                    goToEvent(event);
                }, function () {
                    AlertService.showAlert('error', 'Who Are You?', 'Sorry, you must be logged in to join events');
                    $state.go('login');
                });
        };

        function goToEvent(event) {
            $state.go('event', {
                communityId: $stateParams.communityId,
                missionId: $stateParams.missionId,
                eventId: event.id
            });
        }

        function goBackToCommunity() {
            $state.go('community', { communityId: $stateParams.communityId });
        }

        init();
    }
})();