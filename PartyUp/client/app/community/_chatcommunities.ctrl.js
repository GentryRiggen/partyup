(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunitiesCtrl', communitiesCtrl);

    communitiesCtrl.$inject = ['$scope', 'SignalRService', '$q'];
    function communitiesCtrl($scope, SignalRService, $q) {
        var CommunitiesCtrl = this;
        var chatHub = SignalRService.getHub('chat');
        CommunitiesCtrl.chats = [];

        // Define client functions first
        chatHub.client.newMessage = function (firstname, newMessage) {
            CommunitiesCtrl.chats.push({ name: firstname, message: newMessage });
            $scope.$apply();
        };

        CommunitiesCtrl.newMessage = "";
        CommunitiesCtrl.addNewMessage = function () {
            if (hubConnected) {
                console.log("Hub Connected, sending message");
                chatHub.server.sendMessage(CommunitiesCtrl.newMessage);
                CommunitiesCtrl.newMessage = "";
            } else {
                console.log("Hub NOT Connected, connecting then sending");
                connectToHub().then(function () {
                    chatHub.server.sendMessage(CommunitiesCtrl.newMessage);
                    CommunitiesCtrl.newMessage = "";
                });
            }
            
        };

        var hubConnected = false;
        var connectToHub = function () {
            var deferred = $q.defer();
            $.connection.hub.start().done(function () {
                hubConnected = true;
                deferred.resolve("Connected");
            });

            return deferred.promise;
        }
    }
})();