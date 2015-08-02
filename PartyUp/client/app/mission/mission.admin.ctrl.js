(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('MissionAdminCtrl', MissionAdminCtrl);

    MissionAdminCtrl.$inject = ['MissionsService', 'AlertService', '$stateParams',
        'FilesService', '$mdDialog', '$state'];
    function MissionAdminCtrl(MissionsService, AlertService, $stateParams, FilesService, $mdDialog, $state) {
        var MissionAdmin = this;
        MissionAdmin.modelOptions = { debounce: { 'default': 500, 'blur': 0 }};
        
        function init() {
            AlertService.showLoading('Fetching mission...');
            AlertService.updateTitle('Admin - Mission...');
            AlertService.updateGoBack(MissionAdmin.backToCommunity);
            MissionsService.getById($stateParams.missionId).then(
                function(resp) {
                    AlertService.hideLoading();
                    MissionAdmin.mission = resp.data;
                    AlertService.updateTitle('Admin - ' + MissionAdmin.mission.name);
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                }
            );
        }
        
        MissionAdmin.uploadFile = function(files, type) {
            AlertService.showAlert('info', 'Upload Started', '');
            FilesService.uploadFile(files[0]).then(
                function (resp) {
                    switch (type) {
                        case 'logo':
                            MissionAdmin.mission.logoUrl = resp.data[0].uri;
                            break;
                        case 'banner':
                            MissionAdmin.mission.bannerUrl = resp.data[0].uri;
                            break;
                    }
                    MissionAdmin.update();
                },function () {
                    AlertService.showAlert('error', 'Failed', 'Failed to upload file');
                }
            );
        };
        
        MissionAdmin.update = function() {
            MissionsService.update(MissionAdmin.mission).then(
                function(resp) {
                    AlertService.showAlert('success', 'Success', 'Mission Updated');
                }, function() {
                    AlertService.showAlert('error', 'Error', 'Mission could not be updated');
                }  
            );
        };
        
        MissionAdmin.backToCommunity = function() {
            $state.go('admin.community', {communityId: $stateParams.communityId});
        };
        
        MissionAdmin.delete = function() {
            var confirm = $mdDialog.confirm()
                      .parent(angular.element(document.body))
                      .title('Are you sure?')
                      .content('Are you sure you want to delete this mission?')
                      .ariaLabel('Delete Mission')
                      .ok('OK')
                      .cancel('Cancel')
                      .targetEvent(event);
            $mdDialog.show(confirm).then(function () {
                MissionsService.delete(MissionAdmin.mission.id).then(
                    function () {
                        AlertService.showAlert('success', 'Success!', 'Mission has been deleted');
                        $state.go('admin.community', {communityId: $stateParams.communityId});
                    }, function (err) {
                        AlertService.showAlert('error', 'Failed', 'Failed to delete mission');
                    });
            });
        };
        
        init();
    }
})();