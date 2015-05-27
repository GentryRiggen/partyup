(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('homeCtrl', homeCtrl);

    homeCtrl.$inject = ['$scope'];
    function homeCtrl($scope) {
        var homeCtrl = this;
        homeCtrl.chatHub = $.connection.chat;

        // Define client functions first
        homeCtrl.chatHub.client.newMessage = function (newMessage) {
            console.log("Got New Message From server", homeCtrl.chatHub, newMessage);
            homeCtrl.chats.push({ message: newMessage });
            $scope.$apply();
        };

        $.connection.hub.start();
        

        homeCtrl.chats = [
            { message: "Hello World" },
            { message: "Let's party, bruh" }
        ]

        homeCtrl.newMessage = "";
        homeCtrl.addNewMessage = function () {
            console.log("Sending to server", homeCtrl.chatHub, homeCtrl.newMessage);
            homeCtrl.chatHub.server.sendMessage(homeCtrl.newMessage);
            homeCtrl.newMessage = "";
        };
    }
})();