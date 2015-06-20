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
        var eventHub = SignalRService.getHub('event');

        function init() {
            AlertService.updateTitle('Event');
            AlertService.updateGoBack(goBackToMission);
            AlertService.showLoading('Fetching Event Details...');
            EventsService.getById($stateParams.eventId).then(
                function (resp) {
                    SignalRService.startConnection().then(function () {
                        eventHub.server.joinEventGroup(getGroupId());
                    });
                    AlertService.hideLoading();
                    EventCtrl.event = resp.data;
                    console.log(resp.data);
                    AlertService.updateTitle("Event By: " + EventCtrl.event.organizerName);
                }, function () {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find event');
                    goBackToMission();
                });
        }

        function getGroupId() {
            return $stateParams.missionId + "-" + $stateParams.eventId;
        }
        
        // Define client functions first
        eventHub.client.newMessage = function (fromUser, message) {
            EventCtrl.chatMessages.push({
                from: fromUser,
                message: message
            });
            $scope.$apply();
        };
        
        EventCtrl.newMessage = "";
        EventCtrl.addNewMessage = function () {
            if (SignalRService.connected) {
                eventHub.server.sendMessage(EventCtrl.newMessage, getGroupId());
                EventCtrl.newMessage = "";
            } else {
                AlertService.showAlert('warning', 'Still Connecting', 'Please try again in a few moments');
            }
        };

        function goBackToMission() {
            $state.go('mission', { missionId: $stateParams.missionId });
        };

        init();
    }
})();