(function () {
    'use strict';
    angular.module('partyUp').service('SignalRService', signalRService);

    signalRService.$inject = ['AuthTokenService'];
    function signalRService(AuthTokenService) {
        var signalRSvc = {};

        signalRSvc.getHub = function (hubName) {
            var qs = { 'token': AuthTokenService.getToken() };
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