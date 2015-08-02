(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunitiesCtrl', communitiesCtrl);

    communitiesCtrl.$inject = ['$state', 'CommunitiesService', 'AlertService'];
    function communitiesCtrl($state, CommunitiesService, AlertService) {
        /* jshint -W040 */
        var Communities = this;
        Communities.communities = [];

        function init() {
            AlertService.updateTitle('Communities');
            
            getAll();
        }
        
        function getAll() {
            AlertService.showLoading('Fetching Communities...');
            CommunitiesService.getAll().then(
                function(resp) {
                    AlertService.hideLoading();
                    Communities.communities = resp.data.communities;
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh Oh', 'Failed to get communities');
                }
            );
        }
        
        Communities.goToCommunity = function(community) {
          $state.go('community', {communityId: community.id});
        };
        
        init();
    }
})();