(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('homeCtrl', homeCtrl);

    homeCtrl.$inject = ['$scope', 'userService', 'signalRService', '$q'];
    function homeCtrl($scope, userService, signalRService, $q) {
        var homeCtrl = this;
        var chatHub = signalRService.getHub('chat');
        homeCtrl.chats = [];

        // Define client functions first
        chatHub.client.newMessage = function (firstname, newMessage) {
            homeCtrl.chats.push({ name: firstname, message: newMessage });
            $scope.$apply();
        };

        homeCtrl.newMessage = "";
        homeCtrl.addNewMessage = function () {
            if (hubConnected) {
                console.log("Hub Connected, sending message");
                chatHub.server.sendMessage(homeCtrl.newMessage);
                homeCtrl.newMessage = "";
            } else {
                console.log("Hub NOT Connected, connecting then sending");
                connectToHub().then(function () {
                    chatHub.server.sendMessage(homeCtrl.newMessage);
                    homeCtrl.newMessage = "";
                });
            }
            
        };

        var hubConnected = false;
        var connectToHub = function () {
            var deferred = $q.defer();
            console.log("Connecting to SignalR Hub");
            $.connection.hub.start().done(function () {
                console.log("Connected to SignalR Hub");
                hubConnected = true;
                deferred.resolve("Connected");
            });

            return deferred.promise;
        }
    }
})();