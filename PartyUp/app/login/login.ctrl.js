(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('loginCtrl', loginCtrl);

    loginCtrl.$inject = ['API_URL', 'authToken', '$http', 'alert', '$state'];
    function loginCtrl(API_URL, authToken, $http, alert, $state) {
        var loginCtrl = this;
        
        loginCtrl.loginUser = {
            username: "",
            password: ""
        };

        loginCtrl.submit = function () {
            $http.post(API_URL + "/auth", loginCtrl.loginUser).
                success(function (data, status, headers, config) {
                    authToken.setToken(data);
                    alert.showMessage("success", "Success!", "You are now logged in " + authToken.getUsersName());

                    $state.go('home');
                }).
                error(function (data, status, headers, config) {
                    alert.showMessage("warning", "Oops!", "Wrong username/password");
                });
        };
    }
})();