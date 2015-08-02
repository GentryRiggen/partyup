(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('AdminCtrl', AdminCtrl);

    AdminCtrl.$inject = ['$state'];
    function AdminCtrl($state) {
        var Admin = this;
        Admin.sections = [
            { title: 'Communities', state: 'admin.communities' }
        ];

        function init() {
            Admin.selectedState = false;
            angular.forEach(Admin.sections, function (section) {
                if (!Admin.selectedState && section.state === $state.current.name) {
                    Admin.selectedState = section;
                }
            });
            if (!Admin.selectedState) {
                Admin.selectedState = Admin.sections[0];
            }
            
            if ($state.current.name === 'admin') {
                Admin.goTo(Admin.selectedState);
            }
        }

        Admin.goTo = function (section) {
            Admin.selectedState = section;
            $state.go(section.state);
        };

        init();
    }
})();