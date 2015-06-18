(function () {
    'use strict';
    angular
        .module('partyUp')
        .directive('eventCard', eventCard);

    eventCard.$inject = [];
    function eventCard() {
        return {
            restrict: 'E',
            scope: {
                event: '=',
                whenClicked: '&'
            },
            templateUrl: '/client/app/event/eventCard.tmpl.html',
            link: function (scope, element, attrs) {
                scope.clicked = function () {
                    scope.whenClicked()(scope.event);
                };
            }
        };
    }
})();