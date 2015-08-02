(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunitiesAdminCtrl', communitiesAdminCtrl);

    communitiesAdminCtrl.$inject = ['$state', 'CommunitiesService', 'AlertService'];
    function communitiesAdminCtrl($state, CommunitiesService, AlertService) {
        /* jshint -W040 */
        var CommunitiesAdmin = this;
        CommunitiesAdmin.communities = [];
        
        function init() {
            AlertService.updateTitle('Admin - Communities');
            getAll();
        }
        
        function getAll() {
            AlertService.showLoading('Fetching Communities...');
            CommunitiesService.getAll().then(
                function(resp) {
                    AlertService.hideLoading();
                    CommunitiesAdmin.communities = resp.data.communities;
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh Oh', 'Failed to get communities');
                }
            );
        }
        
        CommunitiesAdmin.goToCommunity = function(community) {
          $state.go('admin.community', {communityId: community.id});
        };
        
        CommunitiesAdmin.createNew = function() {
            AlertService.showAlert('info', 'Creating new Community', '');
            CommunitiesService.createNew().then(
                function(resp) {
                    CommunitiesAdmin.goToCommunity(resp.data);
                }, function() {
                    AlertService.showAlert('error', 'Failed', 'Failed to create new community');
                }
            );
        };
        
        init();
    }
})();