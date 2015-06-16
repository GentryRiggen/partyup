(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('LogoutCtrl', LogoutCtrl);

    LogoutCtrl.$inject = ['$state', 'UserService'];
    function LogoutCtrl($state, UserService) {
        UserService.logout();
        $state.go('communities');
    }
})();