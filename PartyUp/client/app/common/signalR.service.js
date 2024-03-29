﻿(function () {
    'use strict';
    angular.module('partyUp').service('SignalRService', signalRService);

    signalRService.$inject = ['AuthTokenService', '$q', '$rootScope', '$'];
    function signalRService(AuthTokenService, $q, $rootScope, $) {
        var signalRSvc = {};

        signalRSvc.getHub = function (hubName) {
            var qs = { 'token': AuthTokenService.getToken() };
            /* jshint -W117 */
            $.connection.hub.qs = qs;

            /* jshint -W117 */
            var hub = $.connection[hubName];
            if (angular.isDefined(hub)) {
                return hub;
            } else {
                return false;
            }
        };
        
        signalRSvc.connected = false;
        signalRSvc.startConnection = function() {
            var deferred = $q.defer();
            /* jshint -W117 */
            $.connection.hub.start().done(function () {
                signalRSvc.connected = true;
                deferred.resolve('Connected');
            });

            return deferred.promise;
        };
        
        signalRSvc.disconnect = function () {
            /* jshint -W117 */
            $.connection.hub.stop();
            signalRSvc.connected = false;
        };
        
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            if (signalRSvc.connected) {
                signalRSvc.disconnect();
            }
        });
        
        return signalRSvc;
    }
})();