(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('homeCtrl', homeCtrl);

    homeCtrl.$inject = ['$scope', 'userService', 'signalRService'];
    function homeCtrl($scope, userService, signalRService) {
        var homeCtrl = this;
        var chatHub = signalRService.getHub('chat');
        homeCtrl.chats = [];

        // Define client functions first
        chatHub.client.newMessage = function (newMessage) {
            homeCtrl.chats.push({ message: newMessage });
            $scope.$apply();
        };

        homeCtrl.newMessage = "";
        homeCtrl.addNewMessage = function () {
            chatHub.server.sendMessage(homeCtrl.newMessage);
            homeCtrl.newMessage = "";
        };

        // Then start it
        $.connection.hub.start();
    }
})();