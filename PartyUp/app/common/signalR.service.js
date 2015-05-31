(function () {
    'use strict';
    angular.module('partyUp').service('signalRService', signalRService);

    signalRService.$inject = ['authTokenService'];
    function signalRService(authTokenService) {
        var signalRSvc = {};

        signalRSvc.getHub = function (hubName) {
            var hub = $.connection[hubName];
            if (angular.isDefined(hub)) {
                var qs = { token: authTokenService.getToken() };
                console.log("Setting signalR hub query strings", qs);
                hub.qs = qs;
                return hub;
            } else {
                return false;
            }
        };

        return signalRSvc;
    }
})();