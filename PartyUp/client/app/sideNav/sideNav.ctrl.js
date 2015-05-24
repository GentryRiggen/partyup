(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('sideNavCtrl', sideNavCtrl);

    sideNavCtrl.$inject = ['$mdSidenav'];
    function sideNavCtrl($mdSidenav) {
        var ctrl = this;
        ctrl.close = function () {
            $mdSidenav('sideNav').close();
        }
    }
})();