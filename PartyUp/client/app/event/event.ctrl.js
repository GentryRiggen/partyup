(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('EventCtrl', EventCtrl);

    EventCtrl.$inject = ['$scope', 'EventsService', 'AlertService', 'SignalRService', 'UserService', '$stateParams', '$state', '$mdDialog'];
    function EventCtrl($scope, EventsService, AlertService, SignalRService, UserService, $stateParams, $state, $mdDialog) {
        var EventCtrl = this;
        EventCtrl.chatMessages = [];
        EventCtrl.participants = [];
        var eventsHub = SignalRService.getHub('events');
        EventCtrl.status = "Looking for more";

        EventCtrl.currentUser = false;
        function init() {
            AlertService.updateTitle('Event');
            AlertService.updateGoBack(goBackToMission);
            AlertService.showLoading('Fetching Event Details...');
            SignalRService.startConnection().then(function () {
                eventsHub.server.joinEventGroup(getGroupId());
            });
            
            UserService.getCurrentUser().then(function (user) {
                EventCtrl.currentUser = user;
            });
        }

        function getGroupId() {
            return $stateParams.missionId + "-" + $stateParams.eventId;
        }
        
        // Define client functions first
        var allowedInGroup = false;
        eventsHub.client.successfullyJoinedGroup = function(event) {
            console.log("successfullyJoinedGroup", event);
            allowedInGroup = true;
            EventCtrl.event = event;
            AlertService.updateTitle("Event By: " + EventCtrl.event.Organizer.FirstName);
            AlertService.hideLoading();
        };
        
        eventsHub.client.failedToJoinGroup = function(groupId) {
            console.log("failedToJoinGroup", groupId);
            AlertService.hideLoading();
            AlertService.showAlert('error', '', 'Looks like that event has closed. Please try another or host your own.');
            goBackToMission();
        };
        
        

        eventsHub.client.newMessage = function(fromUser, message) {
            EventCtrl.chatMessages.push({
                from: fromUser,
                message: message
            });
            console.log("New message fom user", fromUser, EventCtrl.currentUser);
            if (EventCtrl.currentUser && EventCtrl.currentUser.firstName != fromUser) {
                AlertService.showAlert('info', fromUser + ' sent a message', '');
            }
            $scope.$apply();
        };
        
        eventsHub.client.userJoined = function(user) {
            console.log("User Joined Group!", user);
            // make sure they are not already in the group
            var found = false;
            angular.forEach(EventCtrl.event.EventParticipants, function (part) {
                console.log(user, part);
                if (!found && user.Id == part.Id) {
                    found = true;
                }
            });
            
            if (!found && user.Id != EventCtrl.event.Organizer.Id) {
                EventCtrl.event.EventParticipants.push(user);
                AlertService.showAlert('success', 'New Teammate', user.FirstName + " joined!");
                if (EventCtrl.event.EventParticipants.length == EventCtrl.event.DesiredAmount) {
                    EventCtrl.status = "Closed";
                }
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
            
            // If the organizer is the who left, cancel event
            if (user.Id == EventCtrl.event.Organizer.Id) {
                AlertService.showAlert('warning', 'Event Ended', 'The organizer has left the event.');
                goBackToMission();
            }
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
        
        EventCtrl.showXBLInfo = function(user) {
            window.open('https://account.xbox.com/Messages?gamerTag=' + user.XBLTag, '_blank');
        };

        init();
    }
})();