(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('MissionAdminCtrl', MissionAdminCtrl);

    MissionAdminCtrl.$inject = ['MissionsService', 'AlertService', '$stateParams', 'FilesService', '$mdDialog', '$state'];
    function MissionAdminCtrl(MissionsService, AlertService, $stateParams, FilesService, $mdDialog, $state) {
        var MissionAdminCtrl = this;
        
        function init() {
            AlertService.showLoading('Fetching mission...');
            MissionsService.getById($stateParams.missionId).then(
                function(resp) {
                    AlertService.hideLoading();
                    MissionAdminCtrl.mission = resp.data;
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                }
            );
        }
        
        MissionAdminCtrl.uploadFile = function(files, type) {
            AlertService.showAlert('info', 'Upload Started', '');
            FilesService.uploadFile(files[0]).then(
                function (resp) {
                    switch (type) {
                        case 'logo':
                            MissionAdminCtrl.mission.logoUrl = resp.data[0].uri;
                            break;
                        case 'banner':
                            MissionAdminCtrl.mission.bannerUrl = resp.data[0].uri;
                            break;
                    }
                    MissionAdminCtrl.update();
                },function () {
                    AlertService.showAlert('error', 'Failed', 'Failed to upload file');
                }
            );
        };
        
        MissionAdminCtrl.update = function() {
            MissionsService.update(MissionAdminCtrl.mission).then(
                function(resp) {
                    AlertService.showAlert('success', 'Success', 'Mission Updated');
                }, function() {
                    AlertService.showAlert('error', 'Error', 'Mission could not be updated');
                }  
            );
        };
        
        MissionAdminCtrl.delete = function() {
            var confirm = $mdDialog.confirm()
                      .parent(angular.element(document.body))
                      .title('Are you sure?')
                      .content('Are you sure you want to delete this mission?')
                      .ariaLabel('Delete Mission')
                      .ok('OK')
                      .cancel('Cancel')
                      .targetEvent(event);
            $mdDialog.show(confirm).then(function () {
                MissionsService.delete(MissionAdminCtrl.misson.id).then(
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