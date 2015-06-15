(function () {
    'use strict';
    angular
        .module('partyUp')
        .directive('communityCard', communityCard);

    communityCard.$inject = [];
    function communityCard() {
        return {
            restrict: 'E',
            scope: {
                community: '=community',
                whenClicked: '&'
            },
            templateUrl: '/client/app/community/communityCard.tmpl.html',
            link: function (scope, element, attrs) {
                scope.clicked = function () {
                    scope.whenClicked()(scope.community);
                };
            }
        };
    }
})();