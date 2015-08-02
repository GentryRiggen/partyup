(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('AccountCtrl', AccountCtrl);

    AccountCtrl.$inject = ['$state', 'UserService', 'AlertService'];
    function AccountCtrl($state, UserService, AlertService) {
        var Account = this;
        
        function init() {
            AlertService.updateTitle('My Account');
            AlertService.showLoading('Getting Account...');
            UserService.getCurrentUser().then(
                function(user) {
                   Account.user = user;
                   AlertService.hideLoading();
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Error', 'Unable to to get account');
                });
        }
        
        init();
        
        Account.usernameOk = true;
        Account.checkUsername = function(username) {
            UserService.checkUsername(username, true).then(
                function(resp) {
                    if (resp.data === 'OK') {
                        Account.usernameOk = true;
                    }
                    else {
                       Account.usernameOk = false; 
                    } 
                }, function() {
                    Account.usernameOk = false;
                });
        };
        
        Account.emailOk = true;
        Account.checkEmail = function(email) {
            UserService.checkEmail(email, true).then(
                function(resp) {
                    if (resp.data === 'OK') {
                        Account.emailOk = true;
                    }
                    else {
                       Account.emailOk = false; 
                    } 
                }, function() {
                    Account.emailOk = false;
                });
        };

        Account.update = function () {
            UserService.updateUser(Account.user).then(
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