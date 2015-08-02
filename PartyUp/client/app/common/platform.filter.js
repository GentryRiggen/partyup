(function () {
    'use strict';
    angular
        .module('partyUp')
        .filter('platformFilter', platformFilter);
    function platformFilter() {
        return function (items, name) {
            var filtered = [];
            angular.forEach(items, function (item) {
                var compare;
                if (angular.isDefined(item.platform)) {
                    compare = item.platform;
                } else {
                    compare = item.Platform;
                }
                if (name === '*' || name === compare) {
                    filtered.push(item);
                }
            });

            return filtered;
        };
    }
})();