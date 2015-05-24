(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('headerCtrl', headerCtrl);

    headerCtrl.$inject = ['$mdSidenav'];
    function headerCtrl($mdSidenav) {
        var ctrl = this;
        ctrl.openMenu = function () {
            $mdSidenav("menuNav").toggle();
        };
        //ctrl.isAuthenticated = authToken.isAuthenticated;
    }
})();