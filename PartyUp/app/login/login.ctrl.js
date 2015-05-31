(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('loginCtrl', loginCtrl);

    loginCtrl.$inject = ['userService', '$state', 'alertService'];
    function loginCtrl(userService, $state, alertService) {
        var loginCtrl = this;

        loginCtrl.loginUser = {
            username: "",
            password: ""
        };

        loginCtrl.submit = function () {
            userService.login(loginCtrl.loginUser.username, loginCtrl.loginUser.password)
                .then(function () {
                    $state.go('home');
                },
                function (data, status, headers, config) {
                    alertService.showMessage("warning", "Oops!", "Wrong username/password");
                });
        };
    }
})();