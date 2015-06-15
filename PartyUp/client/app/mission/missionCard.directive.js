(function () {
    'use strict';
    angular
        .module('partyUp')
        .directive('missionCard', missionCard);

    missionCard.$inject = [];
    function missionCard() {
        return {
            restrict: 'E',
            scope: {
                mission: '=mission',
                whenClicked: '&'
            },
            templateUrl: '/client/app/mission/missionCard.tmpl.html',
            link: function (scope, element, attrs) {
                scope.clicked = function () {
                    scope.whenClicked()(scope.mission);
                };
            }
        };
    }
})();