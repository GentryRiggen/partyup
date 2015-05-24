(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('homeCtrl', homeCtrl);

    homeCtrl.$inject = [];
    function homeCtrl() {
        var ctrl = this;
        ctrl.welcome = "Welcome to Party Up!";
    }
})();