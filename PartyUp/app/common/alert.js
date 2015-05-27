(function () {
    'use strict';
    angular
        .module('partyUp')
        .service('alert', alert);

    alert.$inject = ['$rootScope', '$timeout'];
    function alert($rootScope, $timeout) {
        return {
            showMessage: function (type, title, message, timeout) {
                var alertTimeout;
                $rootScope.alert = {
                    hasBeenShown: false,
                    show: true,
                    type: type,
                    message: message,
                    title: title
                };
                if (timeout !== false) {
                    alertTimeout = $timeout(function () {
                        $rootScope.alert.show = false;
                        $timeout(function () {
                            $rootScope.alert.hasBeenShown = true;
                        }, 750);
                    }, timeout || 2000);
                }
            },
            hideMessage: function () {
                $timeout(function () {
                    $rootScope.alert.show = false;
                }, 500);

            }
        }
    }
})();