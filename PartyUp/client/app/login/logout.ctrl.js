(function () {
    'use strict';
    angular
        .module('partyUppartyup')
        .controller('LogoutCtrl', LogoutCtrl);

    LogoutCtrl.$inject = ['$state', 'UserService'];
    function LogoutCtrl($state, UserService) {
        UserService.logout();
        $state.go('login');
    }
})();