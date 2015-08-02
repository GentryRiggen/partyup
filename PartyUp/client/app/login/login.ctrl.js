(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('LoginCtrl', LoginCtrl);

    LoginCtrl.$inject = ['$http', '$state', 'UserService', 'AlertService'];
    function LoginCtrl($http, $state, UserService, AlertService) {
        var Login = this;
        Login.username = '';
        Login.password = '';
        AlertService.hideLoading();
        UserService.getCurrentUser().then(function (resp) {
            console.log(resp);
            AlertService.showAlert('success', 'Already Logged In!', '');
            $state.go('communities');
        });
        Login.submit = function () {
            AlertService.showLoading('Logging you in...');
            UserService.login(Login.username, Login.password).then(
                function (resp) {
                    AlertService.hideLoading();
                    AlertService.showAlert('success', 'Hello ' + resp.user.firstName, '');
                    $state.go('communities');
                },
                function () {
                    AlertService.showAlert('warning', 'Uh Oh!', 'Could not log you in!');
                }
            );
        };
        
        Login.createAccount = function() {
            $state.go('register');
        };
    }
})();