(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('headerCtrl', headerCtrl);

    headerCtrl.$inject = ['$mdSidenav', 'userService'];
    function headerCtrl($mdSidenav, userService) {
        var headerCtrl = this;
        headerCtrl.openMenu = function () {
            $mdSidenav("sideNav").toggle();
        };
        headerCtrl.isAuthenticated = userService.isAuthenticated;
    }
})();