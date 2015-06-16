(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunitiesCtrl', communitiesCtrl);

    communitiesCtrl.$inject = ['$state', 'CommunitiesService', 'AlertService'];
    function communitiesCtrl($state, CommunitiesService, AlertService) {
        var CommunitiesCtrl = this;
        CommunitiesCtrl.communities = [];

        function init() {
            getAll();
        }
        
        function getAll() {
            AlertService.showLoading('Fetching Communities...');
            CommunitiesService.getAll().then(
                function(resp) {
                    AlertService.hideLoading();
                    CommunitiesCtrl.communities = resp.data.communities;
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh Oh', 'Failed to get communities');
                }
            );
        }
        
        CommunitiesCtrl.goToCommunity = function(community) {
          $state.go('community', {communityId: community.id});
        };
        
        init();
    }
})();