(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('CommunityAdminCtrl', communityAdminCtrl);

    communityAdminCtrl.$inject = ['CommunitiesService', 'MissionsService', 'AlertService', '$stateParams', 'FilesService', '$mdDialog', '$state'];
    function communityAdminCtrl(CommunitiesService, MissionsService, AlertService, $stateParams, FilesService, $mdDialog, $state) {
        var CommunityAdminCtrl = this;
        CommunityAdminCtrl.modelOptions = { debounce: { 'default': 500, 'blur': 0 }};
        
        function init() {
            AlertService.showLoading('Fetching community...');
            CommunitiesService.getById($stateParams.communityId).then(
                function(resp) {
                    AlertService.hideLoading();
                    CommunityAdminCtrl.community = resp.data;
                    getMissions();
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find community');
                }
            );
        }
        
        function getMissions() {
            MissionsService.getAllByCommunity(CommunityAdminCtrl.community.id).then(
                function(resp) {
                    CommunityAdminCtrl.missions = resp.data.missions;
                }, function() {
                    AlertService.showAlert('error', 'Failed', 'Failed to get missions');
                }
            );
        }
        
        CommunityAdminCtrl.goToMission = function(mission) {
            $state.go('admin.mission', {communityId: $stateParams.communityId, missionId: mission.id});  
        };
        
        CommunityAdminCtrl.createNewMission = function() {
            AlertService.showLoading('Creating Mission...');
            MissionsService.createNew(CommunityAdminCtrl.community.id).then(
                function(resp) {
                    AlertService.hideLoading();
                    CommunityAdminCtrl.goToMission(resp.data);      
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh Oh', 'Failed to create mission');
                }
            );
        };
        
        CommunityAdminCtrl.uploadFile = function(files, type) {
            AlertService.showAlert('info', 'Upload Started', '');
            FilesService.uploadFile(files[0]).then(
                function (resp) {
                    switch (type) {
                        case 'logo':
                            CommunityAdminCtrl.community.logoUrl = resp.data[0].uri;
                            break;
                        case 'banner':
                            CommunityAdminCtrl.community.bannerUrl = resp.data[0].uri;
                            break;
                    }
                    CommunityAdminCtrl.update();
                },function () {
                    AlertService.showAlert('error', 'Failed', 'Failed to upload file');
                }
            );
        };
        
        CommunityAdminCtrl.update = function() {
            CommunitiesService.update(CommunityAdminCtrl.community).then(
                function(resp) {
                    AlertService.showAlert('success', 'Success', 'Community Updated');
                }, function() {
                    AlertService.showAlert('error', 'Error', 'Community could not be updated');
                }  
            );
        };
        
        CommunityAdminCtrl.delete = function() {
            var confirm = $mdDialog.confirm()
                      .parent(angular.element(document.body))
                      .title('Are you sure?')
                      .content('Are you sure you want to delete this community?')
                      .ariaLabel('Delete Community')
                      .ok('OK')
                      .cancel('Cancel')
                      .targetEvent(event);
            $mdDialog.show(confirm).then(function () {
                CommunitiesService.delete(CommunityAdminCtrl.community.id).then(
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