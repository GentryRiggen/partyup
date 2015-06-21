(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('EventCtrl', EventCtrl);

    EventCtrl.$inject = ['$scope', 'EventsService', 'AlertService', 'SignalRService', '$stateParams', '$state', '$mdDialog'];
    function EventCtrl($scope, EventsService, AlertService, SignalRService, $stateParams, $state, $mdDialog) {
        var EventCtrl = this;
        EventCtrl.chatMessages = [];
        EventCtrl.participants = [];
        var eventsHub = SignalRService.getHub('events');
        EventCtrl.status = "Open";

        function init() {
            AlertService.updateTitle('Event');
            AlertService.updateGoBack(goBackToMission);
            AlertService.showLoading('Fetching Event Details...');
            SignalRService.startConnection().then(function () {
                eventsHub.server.joinEventGroup(getGroupId());
            });
        }

        function getGroupId() {
            return $stateParams.missionId + "-" + $stateParams.eventId;
        }
        
        // Define client functions first
        eventsHub.client.newMessage = function(fromUser, message) {
            EventCtrl.chatMessages.push({
                from: fromUser,
                message: message
            });
            $scope.$apply();
        };
        
        eventsHub.client.userJoined = function(user) {
            console.log("User Joined Group!", user);
            if (user.Id != EventCtrl.event.OrganizerId) {
                EventCtrl.event.EventParticipants.push(user);
                AlertService.showAlert('success', 'New Teammate', user.FirstName + " joined!");
            }
            $scope.$apply();
        };
        
        eventsHub.client.userLeft = function(user) {
            console.log("userLeft", user);
            for (var i = 0; i < EventCtrl.event.EventParticipants.length; i++) {
                if (EventCtrl.event.EventParticipants[i].Id == user.Id) {
                    EventCtrl.event.EventParticipants.splice(i, 1);
                    AlertService.showAlert('warning', 'Teammate Left', user.FirstName + " Left :(");
                    $scope.$apply();
                    break;
                }
            }
        };
        
        var allowedInGroup = false;
        eventsHub.client.successfullyJoinedGroup = function(event) {
            console.log("successfullyJoinedGroup", event);
            allowedInGroup = true;
            EventCtrl.event = event;
            AlertService.updateTitle("Event By: " + EventCtrl.event.OrganizerName);
            AlertService.hideLoading();
        };
        
        eventsHub.client.failedToJoinGroup = function(groupId) {
            console.log("failedToJoinGroup", groupId);
            AlertService.hideLoading();
            AlertService.showAlert('error', 'Sorry', 'Looks like that event has closed. Please try another or host your own.');
            goBackToMission();
        };
        
        EventCtrl.newMessage = "";
        EventCtrl.addNewMessage = function () {
            if (!allowedInGroup) {
                displayConnectingMessage();
            }
            if (SignalRService.connected) {
                eventsHub.server.sendMessage(EventCtrl.newMessage, getGroupId());
                EventCtrl.newMessage = "";
            } else {
                displayConnectingMessage();
            }
        };
        
        function displayConnectingMessage() {
            AlertService.showAlert('warning', 'Still Connecting', 'Please try again in a few moments');
        }

        function goBackToMission() {
            $state.go('mission', { communityId: $stateParams.communityId, missionId: $stateParams.missionId });
        };

        init();
    }
})();