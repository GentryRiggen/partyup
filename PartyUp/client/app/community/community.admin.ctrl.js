(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunityAdminCtrl', communityAdminCtrl);

    communityAdminCtrl.$inject = ['CommunitiesService', 'MissionsService', 'AlertService',
        '$stateParams', 'FilesService', '$mdDialog', '$state'];
    function communityAdminCtrl(CommunitiesService, MissionsService, AlertService,
        $stateParams, FilesService, $mdDialog, $state) {
        /* jshint -W040 */
        var CommunityAdmin = this;
        CommunityAdmin.modelOptions = { debounce: { 'default': 500, 'blur': 0 }};
        
        function init() {
            AlertService.showLoading('Fetching community...');
            AlertService.updateTitle('Admin - Communiy...');
            AlertService.updateGoBack(goBackToCommunities);
            CommunitiesService.getById($stateParams.communityId).then(
                function(resp) {
                    AlertService.hideLoading();
                    CommunityAdmin.community = resp.data;
                    AlertService.updateTitle('Admin - ' + CommunityAdmin.community.name);
                    getMissions();
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find community');
                }
            );
        }
        
        function goBackToCommunities() {
            $state.go('admin.communities');
        }
        
        function getMissions() {
            MissionsService.getAllByCommunity(CommunityAdmin.community.id).then(
                function(resp) {
                    CommunityAdmin.missions = resp.data.missions;
                }, function() {
                    AlertService.showAlert('error', 'Failed', 'Failed to get missions');
                }
            );
        }
        
        CommunityAdmin.goToMission = function(mission) {
            $state.go('admin.mission', {communityId: $stateParams.communityId, missionId: mission.id});  
        };
        
        CommunityAdmin.createNewMission = function() {
            AlertService.showLoading('Creating Mission...');
            MissionsService.createNew(CommunityAdmin.community.id).then(
                function(resp) {
                    AlertService.hideLoading();
                    CommunityAdmin.goToMission(resp.data);      
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh Oh', 'Failed to create mission');
                }
            );
        };
        
        CommunityAdmin.uploadFile = function(files, type) {
            AlertService.showAlert('info', 'Upload Started', '');
            FilesService.uploadFile(files[0]).then(
                function (resp) {
                    switch (type) {
                        case 'logo':
                            CommunityAdmin.community.logoUrl = resp.data[0].uri;
                            break;
                        case 'banner':
                            CommunityAdmin.community.bannerUrl = resp.data[0].uri;
                            break;
                    }
                    CommunityAdmin.update();
                },function () {
                    AlertService.showAlert('error', 'Failed', 'Failed to upload file');
                }
            );
        };
        
        CommunityAdmin.update = function() {
            CommunitiesService.update(CommunityAdmin.community).then(
                function(resp) {
                    AlertService.showAlert('success', 'Success', 'Community Updated');
                }, function() {
                    AlertService.showAlert('error', 'Error', 'Community could not be updated');
                }  
            );
        };
        
        CommunityAdmin.delete = function() {
            var confirm = $mdDialog.confirm()
                      .parent(angular.element(document.body))
                      .title('Are you sure?')
                      .content('Are you sure you want to delete this community?')
                      .ariaLabel('Delete Community')
                      .ok('OK')
                      .cancel('Cancel')
                      .targetEvent(event);
            $mdDialog.show(confirm).then(function () {
                CommunitiesService.delete(CommunityAdmin.community.id).then(
                    function () {
                        AlertService.showAlert('success', 'Success!', 'Community has been deleted');
                        $state.go('admin.communities');
                    }, function (err) {
                        AlertService.showAlert('error', 'Failed', 'Failed to delete community');
                    });
            });
        };
        
        init();
    }
})();