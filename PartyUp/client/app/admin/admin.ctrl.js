(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('AdminCtrl', AdminCtrl);

    AdminCtrl.$inject = ['$state'];
    function AdminCtrl($state) {
        var AdminCtrl = this;
        AdminCtrl.sections = [
            { title: "Communities", state: "admin.communities" }
        ];

        function init() {
            AdminCtrl.selectedState = false;
            angular.forEach(AdminCtrl.sections, function (section) {
                if (!AdminCtrl.selectedState && section.state == $state.current.name) AdminCtrl.selectedState = section;
            });
            if (!AdminCtrl.selectedState) AdminCtrl.selectedState = AdminCtrl.sections[0];
            
            if ($state.current.name === "admin") AdminCtrl.goTo(AdminCtrl.selectedState);
        }

        AdminCtrl.goTo = function (section) {
            AdminCtrl.selectedState = section;
            $state.go(section.state);
        };

        init();
    }
})();