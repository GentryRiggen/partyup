(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('MissionCtrl', MissionCtrl);

    MissionCtrl.$inject = ['$scope', 'MissionsService', 'EventsService', 'AlertService', 'SignalRService', '$stateParams', '$state'];
    function MissionCtrl($scope, MissionsService, EventsService, AlertService, SignalRService, $stateParams, $state) {
        var MissionCtrl = this;
        MissionCtrl.events = false;
        MissionCtrl.allowHosting = false;
        var eventsHub = SignalRService.getHub('events');
        
        // Define client functions first
        eventsHub.client.newHostedEvent = function (event) {
            if (MissionCtrl.events !== false) {
                var e = {
                    createdOn: Date.parse(event.CreatedOn).toString(),
                    eventParticipants: event.EventParticipants,
                    id: event.Id,
                    missionId: event.MissionId,
                    missionName: event.MissionName,
                    organizerId: event.OrganizerId,
                    organizerName: event.OrganizerName,
                    organizerUserName: event.OrganizerUserName
                }
                MissionCtrl.events.unshift(e);
                $scope.$apply();
            }
        };

        eventsHub.client.eventCreated = function (event) {
            // Navigate to Event
            AlertService.showAlert("success", 'Created', 'Event Created!');
            console.log(event);
        };

        MissionCtrl.hostEvent = function () {
            if (MissionCtrl.allowHosting) {
                eventsHub.server.hostEvent($stateParams.missionId);
            } else {
                AlertService.showAlert('warning', 'Just a sec', 'Still establishing a connection');
            }
        };

        function init() {
            AlertService.showLoading('Fetching mission...');
            MissionsService.getById($stateParams.missionId).then(
                function (resp) {
                    AlertService.hideLoading();
                    MissionCtrl.mission = resp.data;
                    getCurrentEvents();
                }, function () {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                }
            );
        }

        function getCurrentEvents() {
            EventsService.getAllByMission($stateParams.missionId).then(
                function (resp) {
                    MissionCtrl.events = [];
                    angular.forEach(resp.data, function(event) {
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

        MissionCtrl.goBackToCommunity = function () {
            $state.go('community', { communityId: $stateParams.communityId });
        };

        init();
    }
})();