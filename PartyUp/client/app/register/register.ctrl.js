(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('RegisterCtrl', RegisterCtrl);

    RegisterCtrl.$inject = ['$state', 'UserService', 'AlertService'];
    function RegisterCtrl($state, UserService, AlertService) {
        var RegisterCtrl = this;
        RegisterCtrl.user = {
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
        
        RegisterCtrl.usernameOk = true;
        RegisterCtrl.checkUsername = function(username) {
            UserService.checkUsername(username).then(
                function(resp) {
                    if (resp.data == "OK") {
                        RegisterCtrl.usernameOk = true;
                    }
                    else {
                       RegisterCtrl.usernameOk = false; 
                    } 
                }, function() {
                    RegisterCtrl.usernameOk = false;
                });
        };
        
        RegisterCtrl.emailOk = true;
        RegisterCtrl.checkEmail = function(email) {
            UserService.checkEmail(email).then(
                function(resp) {
                    if (resp.data == "OK") {
                        RegisterCtrl.emailOk = true;
                    }
                    else {
                       RegisterCtrl.emailOk = false; 
                    } 
                }, function() {
                    RegisterCtrl.emailOk = false;
                });
        };

        RegisterCtrl.submit = function () {
            AlertService.showLoading("Registering User...");
            UserService.register(RegisterCtrl.user).then(
                function (resp) {
                    AlertService.showAlert('success', 'Account Created!', '');
                    UserService.login(RegisterCtrl.user.username, RegisterCtrl.user.password).then(
                        function(resp) {
                            AlertService.hideLoading();
                            $state.go('communities');
                        }, function() {
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
        }
    }
})();