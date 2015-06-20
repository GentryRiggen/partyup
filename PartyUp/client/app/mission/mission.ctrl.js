(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('MissionCtrl', MissionCtrl);

    MissionCtrl.$inject = ['UserService', '$scope', 'MissionsService', 'EventsService', 'AlertService', 'SignalRService', '$stateParams', '$state', '$mdDialog'];
    function MissionCtrl(UserService, $scope, MissionsService, EventsService, AlertService, SignalRService, $stateParams, $state, $mdDialog) {
        var MissionCtrl = this;
        MissionCtrl.events = false;
        MissionCtrl.allowHosting = false;
        var eventsHub = SignalRService.getHub('events');
        
        function init() {
            console.log("Connected to SignalR ", SignalRService.connected);
            AlertService.updateTitle('Mission...');
            AlertService.updateGoBack(goBackToCommunity);
            AlertService.showLoading('Fetching mission...');
            MissionsService.getById($stateParams.missionId).then(
                function (resp) {
                    AlertService.hideLoading();
                    MissionCtrl.mission = resp.data;
                    AlertService.updateTitle(MissionCtrl.mission.name);
                    MissionCtrl.events = [];
                    SignalRService.startConnection().then(function () {
                        MissionCtrl.allowHosting = true;
                        eventsHub.server.joinMissionGroup($stateParams.missionId);
                    });
                    //getCurrentEvents();
                }, function () {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                }
                );
        }

        function getCurrentEvents() {
            console.log("Getting Current Events");
            EventsService.getAllByMission($stateParams.missionId).then(
                function (resp) {
                    MissionCtrl.events = [];
                    angular.forEach(resp.data, function (event) {
                        event.createdOn = Date.parse(event.createdOn).toString();
                        MissionCtrl.events.push(event);
                    });
                    SignalRService.startConnection().then(function () {
                        MissionCtrl.allowHosting = true;
                        eventsHub.server.joinMissionGroup($stateParams.missionId);
                    });
                }, function () {
                    AlertService.showAlert('error', 'Failed', 'Failed to get events');
                }
                );
        }
        
        // Define client functions first
        eventsHub.client.newHostedEvent = function (event) {
            console.log(event);
            if (MissionCtrl.events !== false) {
                var e = {
                    createdOn: event.CreatedOn,
                    eventParticipants: event.EventParticipants,
                    id: event.Id,
                    missionId: event.MissionId,
                    missionName: event.MissionName,
                    desiredAmount: event.DesiredAmount,
                    organizerId: event.OrganizerId,
                    organizerName: event.OrganizerName,
                    organizerUserName: event.OrganizerUserName
                };
                MissionCtrl.events.unshift(e);
                angular.forEach(MissionCtrl.events, function(event) {
                    event.timeAgo = moment(event.createdOn).fromNow();
                });
                $scope.$apply();
            }
        };

        eventsHub.client.eventCreated = function (event) {
            // Navigate to Event
            AlertService.showAlert("success", 'Created', 'Event Created!');
            console.log(event);
        };

        MissionCtrl.hostEvent = function (event) {
            $mdDialog.show({
                controller: amountModal,
                templateUrl: 'amountModal.html',
                parent: angular.element(document.body),
                targetEvent: event,
            }).then(function (amount) {
                createNewHostedEvent(amount);
            });
        };

        function amountModal($scope, $mdDialog) {
            $scope.amount = 1;
            $scope.possibleAmounts = [1,2,3,4,5,6,7,8,9,10];
            $scope.cancel = function () {
                $mdDialog.cancel();
            };
            $scope.complete = function () {
                $mdDialog.hide($scope.amount);
            };
        }

        function createNewHostedEvent(desiredAmount) {
            if (MissionCtrl.allowHosting) {
                UserService.getCurrentUser().then(
                function(user) {
                    eventsHub.server.hostEvent($stateParams.missionId, desiredAmount);
                }, function() {
                   AlertService.showAlert('error', 'Who Are You?', 'Sorry, you must be logged in to host events');
                   $state.go('login'); 
                });
            } else {
                AlertService.showAlert('warning', 'Just a sec', 'Still establishing a connection');
            }
        };
        
        MissionCtrl.joinEvent = function(event) {
            console.log("Seeing if user can join event");
            UserService.getCurrentUser().then(
                function(user) {
                    $state.go('event', { 
                        communityId: $stateParams.communityId,
                        missionId: $stateParams.missionId,
                        eventId: event.id    
                    });
                }, function() {
                   AlertService.showAlert('error', 'Who Are You?', 'Sorry, you must be logged in to join events');
                   $state.go('login'); 
                });
        };

        function goBackToCommunity() {
            $state.go('community', { communityId: $stateParams.communityId });
        };

        init();
    }
})();