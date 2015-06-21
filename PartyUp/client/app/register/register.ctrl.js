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
        
        RegisterCtrl.formOk = true;
        RegisterCtrl.checkUsername = function(username) {
            console.log('Checking Username: ', username);
            UserService.checkUsername(username).then(
                function(resp) {
                    if (resp.data == "OK") {
                        console.log("Ok");
                        RegisterCtrl.formOk = true;
                    }
                    else {
                        console.log("Not Ok");
                       RegisterCtrl.formOk = false; 
                    } 
                }, function() {
                    console.log("Not Ok");
                    RegisterCtrl.formOk = false;
                });
        };

        RegisterCtrl.submit = function () {
            AlertService.showLoading("Registering User...");
            UserService.register(RegisterCtrl.user).then(
                function () {
                    AlertService.showAlert('success', 'Account Created!', '');
                    UserService.logout();
                    UserService.login(RegisterCtrl.user.username, RegisterCtrl.user.password).then(
                        function() {
                            AlertService.hideLoading();
                            $state.go('communities');
                            window.reload();
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