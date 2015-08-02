(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('AboutCtrl', AboutCtrl);

    AboutCtrl.$inject = ['$http', 'AlertService', 'API_URL'];
    function AboutCtrl($http, AlertService, API_URL) {
        var About = this;
        AlertService.updateTitle('About Party Up');
        About.user = {
            name: '',
            email: '',
            message: ''
        };
        
        About.submit = function () {
            $http.post(API_URL + '/misc/sendfeedback', About.user).then(
                function() {
                    AlertService.showAlert('success', 'Feedback Sent!', '');
                }, function() {
                    AlertService.showAlert('error', 'Uh Oh!', 'Unable to send feedback. Please try again.');
                });
        };
    }
})();