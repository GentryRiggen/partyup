(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('EventCtrl', EventCtrl);

    EventCtrl.$inject = ['$scope', 'EventsService', 'AlertService', 'SignalRService',
        'UserService', '$stateParams', '$state', '$mdDialog', '$rootScope'];
    function EventCtrl($scope, EventsService, AlertService, SignalRService,
        UserService, $stateParams, $state, $mdDialog, $rootScope) {
        /* jshint -W040 */
        var Event = this;
        Event.chatMessages = [];
        Event.participants = [];
        var eventsHub = SignalRService.getHub('events');
        Event.status = 'Looking for more';

        Event.currentUser = false;
        function init() {
            AlertService.updateTitle('Event');
            AlertService.updateGoBack(goBackToMission);
            AlertService.showLoading('Fetching Event Details...');
            UserService.getCurrentUser().then(function (user) {
                Event.currentUser = user;
                SignalRService.startConnection().then(function () {
                    eventsHub.server.joinEventGroup(getGroupId());
                });
            });
        }

        function getGroupId() {
            return $stateParams.missionId + '-' + $stateParams.eventId;
        }
        
        // Define client functions first
        var allowedInGroup = false;
        eventsHub.client.successfullyJoinedGroup = function (event) {
            allowedInGroup = true;
            Event.event = event;
            if (Event.event.organizer.id === Event.currentUser.user.id) {
                $rootScope.$broadcast('partyup.hostedEvent', Event.event);
            } else {
                $rootScope.$broadcast('partyup.joinedEvent', Event.event);
            }
            AlertService.updateTitle(Event.event.organizer.username + ' - ' + Event.event.mission.name);
            AlertService.hideLoading();
        };
        
        eventsHub.client.failedToJoinGroup = function(groupId) {
            AlertService.hideLoading();
            AlertService.showAlert('error', '',
                'Looks like that event has closed. Please try another or host your own.');
            goBackToMission();
        };
        
        eventsHub.client.newMessage = function(fromUser, message) {
            Event.chatMessages.push({
                from: fromUser,
                message: message
            });
            if (Event.currentUser && Event.currentUser.user.firstName !== fromUser) {
                AlertService.showAlert('info', fromUser + ' sent a message', '');
            }
            $scope.$apply();
        };
        
        eventsHub.client.userJoined = function(user) {
            // make sure they are not already in the group
            var found = false;
            angular.forEach(Event.event.eventParticipants, function (part) {
                if (!found && user.id === part.id) {
                    found = true;
                }
            });
            
            if (!found && user.id !== Event.event.organizer.id) {
                Event.event.eventParticipants.push(user);
                AlertService.showAlert('success', 'New Teammate', user.username + ' joined!');
                if (Event.event.eventParticipants.length === Event.event.desiredAmount) {
                    Event.status = 'Closed';
                }
            }
            $scope.$apply();
        };
        
        eventsHub.client.userLeft = function(user) {
            for (var i = 0; i < Event.event.eventParticipants.length; i++) {
                if (Event.event.eventParticipants[i].id === user.id) {
                    Event.event.eventParticipants.splice(i, 1);
                    AlertService.showAlert('warning', 'Teammate Left', user.username + ' Left :(');
                    $scope.$apply();
                    break;
                }
            }
            
            // If the organizer is the who left, cancel event
            if (user.id === Event.event.organizer.Id) {
                AlertService.showAlert('warning', 'Event Ended', 'The organizer has left the event.');
                goBackToMission();
            }
        };
        
        Event.newMessage = '';
        Event.addNewMessage = function () {
            if (!allowedInGroup) {
                displayConnectingMessage();
            }
            if (SignalRService.connected) {
                eventsHub.server.sendMessage(Event.newMessage, getGroupId());
                Event.newMessage = '';
            } else {
                displayConnectingMessage();
            }
        };
        
        function displayConnectingMessage() {
            AlertService.showAlert('warning', 'Still Connecting', 'Please try again in a few moments');
        }

        function goBackToMission() {
            $state.go('mission', { communityId: $stateParams.communityId, missionId: $stateParams.missionId });
        }
        
        Event.showXBLInfo = function(user) {
            window.open('https://account.xbox.com/Messages?gamerTag=' + user.xblTag, '_blank');
        };

        init();
    }
})();