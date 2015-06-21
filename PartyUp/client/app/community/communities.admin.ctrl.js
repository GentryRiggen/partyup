(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunitiesAdminCtrl', communitiesAdminCtrl);

    communitiesAdminCtrl.$inject = ['$state', 'CommunitiesService', 'AlertService'];
    function communitiesAdminCtrl($state, CommunitiesService, AlertService) {
        var CommunitiesAdminCtrl = this;
        CommunitiesAdminCtrl.communities = [];
        
        function init() {
            AlertService.updateTitle('Admin - Communities');
            getAll();
        }
        
        function getAll() {
            AlertService.showLoading('Fetching Communities...');
            CommunitiesService.getAll().then(
                function(resp) {
                    AlertService.hideLoading();
                    CommunitiesAdminCtrl.communities = resp.data.communities;
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh Oh', 'Failed to get communities');
                }
            );
        }
        
        CommunitiesAdminCtrl.goToCommunity = function(community) {
          $state.go('admin.community', {communityId: community.id});
        };
        
        CommunitiesAdminCtrl.createNew = function() {
            AlertService.showAlert('info', 'Creating new Community', '');
            CommunitiesService.createNew().then(
                function(resp) {
                    CommunitiesAdminCtrl.goToCommunity(resp.data);
                }, function() {
                    AlertService.showAlert('error', 'Failed', 'Failed to create new community');
                }
            );
        };
        
        init();
    }
})();