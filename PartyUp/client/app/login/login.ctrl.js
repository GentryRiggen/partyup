(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('LoginCtrl', LoginCtrl);

    LoginCtrl.$inject = ['$http', '$state', 'UserService', 'AlertService'];
    function LoginCtrl($http, $state, UserService, AlertService) {
        var LoginCtrl = this;
        LoginCtrl.username = "";
        LoginCtrl.password = "";
        AlertService.hideLoading();
        UserService.getCurrentUser().then(function () {
            AlertService.showAlert('success', 'Already Logged In!', '');
            $state.go('communities');
        });
        LoginCtrl.submit = function () {
            UserService.login(LoginCtrl.username, LoginCtrl.password).then(
                function () {
                    AlertService.showAlert('success', 'Logged In!', '');
                    $state.go('communities');
                },
                function () {
                    AlertService.showAlert('warning', 'Uh Oh!', 'Could not log you in!');
                }
            );
        };
        
        LoginCtrl.createAccount = function() {
            $state.go('register');
        };
    }
})();