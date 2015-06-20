(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunityCtrl', CommunityCtrl);

    CommunityCtrl.$inject = ['CommunitiesService', 'MissionsService', 'AlertService', '$stateParams', '$state'];
    function CommunityCtrl(CommunitiesService, MissionsService, AlertService, $stateParams, $state) {
        var CommunityCtrl = this;
        CommunityCtrl.missions = false;
        
        function init() {
            AlertService.updateTitle('Communiy...');
            AlertService.updateGoBack(goBackToCommunities);
            AlertService.showLoading('Fetching community...');
            CommunitiesService.getById($stateParams.communityId).then(
                function(resp) {
                    AlertService.hideLoading();
                    CommunityCtrl.community = resp.data;
                    AlertService.updateTitle(CommunityCtrl.community.name);
                    getMissions();
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find community');
                }
            );
        }
        
        function getMissions() {
            MissionsService.getAllByCommunity(CommunityCtrl.community.id).then(
                function(resp) {
                    CommunityCtrl.missions = resp.data.missions;
                }, function() {
                    AlertService.showAlert('error', 'Failed', 'Failed to get missions');
                }
            );
        }
        
        CommunityCtrl.goToMission = function(mission) {
            $state.go('mission', {communityId: $stateParams.communityId, missionId: mission.id});  
        };
        
        function goBackToCommunities() {
            $state.go('communities');
        }
        
        init();
    }
})();