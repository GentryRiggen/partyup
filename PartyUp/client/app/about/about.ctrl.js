(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('AboutCtrl', AboutCtrl);

    AboutCtrl.$inject = ['$http', 'AlertService', 'API_URL'];
    function AboutCtrl($http, AlertService, API_URL) {
        var AboutCtrl = this;
        AlertService.updateTitle('About Party Up');
        AboutCtrl.user = {
            name: '',
            email: '',
            message: ''
        };
        
        AboutCtrl.submit = function() {
            $http.post(API_URL + '/misc/sendfeedback', AboutCtrl.user).then(
                function() {
                    AlertService.showAlert('success', 'Feedback Sent!', '');
                }, function() {
                    AlertService.showAlert('error', 'Uh Oh!', 'Unable to send feedback. Please try again.');
                });
        };
    }
})();