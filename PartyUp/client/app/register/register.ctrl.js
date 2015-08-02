(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('RegisterCtrl', RegisterCtrl);

    RegisterCtrl.$inject = ['$state', 'UserService', 'AlertService'];
    function RegisterCtrl($state, UserService, AlertService) {
        var Register = this;
        Register.user = {
            username: '',
            firstName: '',
            lastName: '',
            email: '',
            XBLTag: '',
            PSNTag: '',
            steamTag: '',
            password: '',
            confirmPassword: ''
        };
        AlertService.updateTitle('Register');
        
        Register.usernameOk = true;
        Register.checkUsername = function(username) {
            UserService.checkUsername(username).then(
                function(resp) {
                    if (resp.data === 'OK') {
                        Register.usernameOk = true;
                    } else {
                        Register.usernameOk = false;
                    }
                }, function() {
                    Register.usernameOk = false;
                });
        };
        
        Register.emailOk = true;
        Register.checkEmail = function(email) {
            UserService.checkEmail(email).then(
                function(resp) {
                    if (resp.data === 'OK') {
                        Register.emailOk = true;
                    } else {
                        Register.emailOk = false;
                    }
                }, function() {
                    Register.emailOk = false;
                });
        };

        Register.submit = function () {
            AlertService.showLoading('Registering User...');
            Register.user.username.trim();
            Register.user.firstName.trim();
            Register.user.lastName.trim();
            Register.user.email.trim();
            Register.user.XBLTag.trim();
            Register.user.PSNTag.trim();
            Register.user.steamTag.trim();
            UserService.register(Register.user).then(
                function (resp) {
                    AlertService.showAlert('success', 'Account Created!', '');
                    UserService.login(Register.user.username, Register.user.password).then(
                        function (resp) {
                            AlertService.hideLoading();
                            $state.go('communities');
                        }, function () {
                            AlertService.hideLoading();
                            AlertService.showAlert('warning', 'Uh Oh!', 'Could not create your account');
                        }
                    );

                },
                function () {
                    AlertService.hideLoading();
                    AlertService.showAlert('warning', 'Uh Oh!', 'Could not create your account');
                }
            );
        };
    }
})();