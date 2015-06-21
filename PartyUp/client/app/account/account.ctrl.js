(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('AccountCtrl', AccountCtrl);

    AccountCtrl.$inject = ['$state', 'UserService', 'AlertService'];
    function AccountCtrl($state, UserService, AlertService) {
        var AccountCtrl = this;
        
        function init() {
            AlertService.updateTitle('My Account');
            AlertService.showLoading('Getting Account...');
            UserService.getCurrentUser().then(
                function(user) {
                   AccountCtrl.user = user;
                   console.log("user ", user);
                   AlertService.hideLoading();
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Error', 'Unable to to get account');
                });
        }
        
        init();
        
        AccountCtrl.usernameOk = true;
        AccountCtrl.checkUsername = function(username) {
            console.log("Checking username");
            UserService.checkUsername(username, true).then(
                function(resp) {
                    if (resp.data == "OK") {
                        AccountCtrl.usernameOk = true;
                    }
                    else {
                       AccountCtrl.usernameOk = false; 
                    } 
                }, function() {
                    AccountCtrl.usernameOk = false;
                });
        };
        
        AccountCtrl.emailOk = true;
        AccountCtrl.checkEmail = function(email) {
            UserService.checkEmail(email, true).then(
                function(resp) {
                    if (resp.data == "OK") {
                        AccountCtrl.emailOk = true;
                    }
                    else {
                       AccountCtrl.emailOk = false; 
                    } 
                }, function() {
                    AccountCtrl.emailOk = false;
                });
        };

        AccountCtrl.update = function () {
            UserService.updateUser(AccountCtrl.user).then(
                function () {
                    AlertService.showAlert('success', 'Account Updated!', '');
                },
                function () {
                    AlertService.showAlert('error', 'Uh Oh!', 'Could not update your account');
                }
            );
        };
    }
})();