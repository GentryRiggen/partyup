(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('headerCtrl', headerCtrl);

    headerCtrl.$inject = ['$mdSidenav', 'authToken'];
    function headerCtrl($mdSidenav, authToken) {
        var headerCtrl = this;
        headerCtrl.openMenu = function () {
            $mdSidenav("sideNav").toggle();
        };
        headerCtrl.isAuthenticated = authToken.isAuthenticated;
    }
})();