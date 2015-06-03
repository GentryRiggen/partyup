(function () {
    'use strict';
    angular.module('partyUp').service('signalRService', signalRService);

    signalRService.$inject = ['authTokenService'];
    function signalRService(authTokenService) {
        var signalRSvc = {};

        signalRSvc.getHub = function (hubName) {
            var qs = { 'token': authTokenService.getToken() };
            $.connection.hub.qs = qs;
            console.log("Setting signalR hub query strings", qs);

            var hub = $.connection[hubName];
            if (angular.isDefined(hub)) {
                
                return hub;
            } else {
                return false;
            }
        };

        return signalRSvc;
    }
})();