(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunityCtrl', CommunityCtrl);

    CommunityCtrl.$inject = ['CommunitiesService', 'MissionsService', 'AlertService', '$stateParams', '$state'];
    function CommunityCtrl(CommunitiesService, MissionsService, AlertService, $stateParams, $state) {
        var Community = this;
        Community.missions = false;
        
        function init() {
            AlertService.updateTitle('Communiy...');
            AlertService.updateGoBack(goBackToCommunities);
            AlertService.showLoading('Fetching community...');
            CommunitiesService.getById($stateParams.communityId).then(
                function (resp) {
                    AlertService.showLoading('Fetching Missions...');
                    Community.community = resp.data;
                    AlertService.updateTitle(Community.community.name);
                    getMissions();
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find community');
                }
            );
        }
        
        function getMissions() {
            MissionsService.getAllByCommunity(Community.community.id).then(
                function(resp) {
                    Community.missions = resp.data.missions;
                    AlertService.hideLoading();
                }, function() {
                    AlertService.showAlert('error', 'Failed', 'Failed to get missions');
                }
            );
        }
        
        Community.search = function(q) {
            MissionsService.search(Community.community.id, q).then(
                function(resp) {
                    Community.missions = resp.data.missions;
                }, function() {
                    AlertService.showAlert('error', 'Failed', 'Failed to search missions');
                });
        };
        
        Community.goToMission = function(mission) {
            $state.go('mission', {communityId: $stateParams.communityId, missionId: mission.id});  
        };
        
        function goBackToCommunities() {
            $state.go('communities');
        }
        
        init();
    }
})();