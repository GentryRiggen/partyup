(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('homeCtrl', homeCtrl);

    homeCtrl.$inject = [];
    function homeCtrl() {
        var homeCtrl = this;
        homeCtrl.welcome = "Welcome to Party Up!";
    }
})();