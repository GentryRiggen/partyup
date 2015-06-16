(function () {
    'use strict';
    angular
        .module('partyUp')
        .controller('MissionCtrl', MissionCtrl);

    MissionCtrl.$inject = ['MissionsService', 'AlertService', '$stateParams', '$state'];
    function MissionCtrl(MissionsService, AlertService, $stateParams, $state) {
        var MissionCtrl = this;
        
        function init() {
            AlertService.showLoading('Fetching mission...');
            MissionsService.getById($stateParams.missionId).then(
                function(resp) {
                    AlertService.hideLoading();
                    MissionCtrl.mission = resp.data;
                }, function() {
                    AlertService.hideLoading();
                    AlertService.showAlert('error', 'Uh oh', 'Could not find mission');
                }
            );
        }
        
        MissionCtrl.goBackToCommunity = function() {
            $state.go('community', {communityId: $stateParams.communityId});
        };
        
        init();
    }
})();