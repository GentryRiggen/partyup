(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('MissionCtrl', MissionCtrl);

    MissionCtrl.$inject = ['$timeout', 'UserService', '$scope', 'MissionsService', 'EventsService', 'AlertService', 'SignalRService', '$stateParams', '$state', '$mdDialog'];
    function MissionCtrl($timeout, UserService, $scope, MissionsService, EventsService, AlertService, SignalRService, $stateParams, $state, $mdDialog) {
        var MissionCtrl = this;
        MissionCtrl.events = false;
        MissionCtrl.allowHosting = false;
        MissionCtrl.play = true;
        var eventsHub = SignalRService.getHub('events');

        function init() {
            AlertService.updateTitle('Mission...');
            AlertService.updateGoBack(goBackToCommunity);
            AlertService.showLoading('Fetching mission...');
            MissionsService.getById($stateParams.missionId).then(
                function (resp) {
                    AlertService.hideLoading();
                    MissionCtrl.mission = resp.data;
                    AlertService.updateTitle(MissionCtrl.mission.name);
                    EventsService.getAllByMission($stateParams.missionId).then(
                        function (resp) {
                            MissionCtrl.events = resp.data
                            updateEventTimes();
                            SignalRService.startConnection().then(function () {
                                MissionCtrl.allowHosting = true;
                                eventsHub.server.joinMissionGroup($stateParams.missionId);
                                if (angular.isDefined($stateParams.host) && $stateParams.host == 'true') {
                                    MissionCtrl.hostEvent();
                                }
                            });
                        }, function () {
                            AlertService.hideLoading();
                            AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                        });
                    
                }, function () {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                });
        }
        
        // Define client functions first
        var eventsWhilePaused = [];
        eventsHub.client.newHostedEvent = function (event) {
            console.log("New hosted event! ", event);
            // Only add new ones if paused
            if (!MissionCtrl.play) {
                eventsWhilePaused.push(event);
                return;
            }

            var found = false;
            for (var i = 0; i < MissionCtrl.events.length; i++) {
                if (MissionCtrl.events[i].id == event.id) {
                    found = true;
                    break;
                }
            }
            if (!found && MissionCtrl.events !== false) {
                MissionCtrl.events.unshift(event);
                updateEventTimes(true);
            }
        };

        function updateEventTimes(runApply) {
            angular.forEach(MissionCtrl.events, function (event) {
                event.timeAgo = moment.utc(event.createdOn).fromNow();
            });
            if (runApply) $scope.$apply();
        }

        MissionCtrl.togglePlay = function () {
            MissionCtrl.play = !MissionCtrl.play;
            if (MissionCtrl.play) {
                // Add any while paused
                for (var i = eventsWhilePaused.length - 1; i >= 0; i--) {
                    MissionCtrl.events.push(eventsWhilePaused[i]);
                    if (i == 0) eventsWhilePaused = [];
                }
                updateEventTimes(false);
            }
        };

        eventsHub.client.removeEvent = function (event) {
            console.log("Was told to remove event", event);
            // Remove from current Events
            var currentMissoinsIndex = -1;
            for (var i = 0; i < MissionCtrl.events.length; i++) {
                if (MissionCtrl.events[i].id == event.id) {
                    currentMissoinsIndex = i;
                    break;
                }
            }
            if (currentMissoinsIndex != -1) {
                MissionCtrl.events[currentMissoinsIndex].closed = true;
                $scope.$apply();
                $timeout(function () {
                    MissionCtrl.events.splice(currentMissoinsIndex, 1);
                    $scope.$apply();

                }, 2000);
            }
            
            // Remove From Paused Events
            var pausedMissionsIndex = -1;
            for (var i = 0; i < eventsWhilePaused.length; i++) {
                if (eventsWhilePaused[i].id == event.id) {
                    pausedMissionsIndex = i;
                    break;
                }
            }
            if (pausedMissionsIndex != -1) {
                eventsWhilePaused.splice(currentMissoinsIndex, 1);
            }
        };

        eventsHub.client.updateLookingForCount = function (event, newCount) {
            for (var i = 0; i < MissionCtrl.events.length; i++) {
                if (MissionCtrl.events[i].id == event.id) {
                    MissionCtrl.events[i].DesiredAmount = newCount;
                    $scope.$apply();
                    break;
                }
            }
            
            for (var i = 0; i < eventsWhilePaused.length; i++) {
                if (eventsWhilePaused[i].id == event.id) {
                    eventsWhilePaused[i].DesiredAmount = newCount;
                    $scope.$apply();
                    break;
                }
            }
        };

        eventsHub.client.eventCreated = function (event) {
            AlertService.showAlert("success", 'Created', 'Event Created!');
            goToEvent(event);
        };

        MissionCtrl.hostEvent = function (event) {
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

        MissionCtrl.platforms = ['All Platforms', 'Xbox 360', 'Xbox One', 'Playstation 3', 'Playstation 4'];
        MissionCtrl.platform = MissionCtrl.platforms[0];
        MissionCtrl.platformFilter = "*";
        MissionCtrl.filterPlatform = function (platform) {
            if (platform == "All Platforms") {
                MissionCtrl.platformFilter = "*";
            } else {
                MissionCtrl.platformFilter = platform;
            }
        };

        function showNewEventModal(event) {
            $mdDialog.show({
                controller: eventDetailsModal,
                templateUrl: 'amountModal.html',
                parent: angular.element(document.body),
                targetEvent: event,
            }).then(function (event) {
                console.log("Creating new hosted event!", event);
                createNewHostedEvent(event);
            });
        };

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
            if (MissionCtrl.allowHosting) {
                eventsHub.server.hostEvent($stateParams.missionId, eventDetails);
            } else {
                AlertService.showAlert('warning', 'Just a sec', 'Still establishing a connection');
            }
        };

        MissionCtrl.joinEvent = function (event) {
            console.log("Seeing if user can join event");
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
        };

        init();
    }
})();