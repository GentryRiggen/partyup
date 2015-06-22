(function () {
    'use strict';
    angular.module('partyUp').service('MissionsService', MissionsService);

    MissionsService.$inject = ['API_URL', '$http'];
    function MissionsService(API_URL, $http) {
        var service = {};
		var communityMissions = API_URL + '/communities';
        var missionsUrl = API_URL + '/missions';
		
		service.getAllByCommunity = function(communityId) {
			return $http.get(communityMissions + '/' + communityId + '/missions');	
		};
        
        service.getById = function(id) {
            return $http.get(missionsUrl + '/' + id);  
        };
        
        service.update = function(mission) {
            return $http.put(missionsUrl + '/' + mission.id, mission);  
        };
        
        service.createNew = function(communityId) {
            return $http.post(communityMissions + '/' + communityId + '/missions/createnew', {
                name: 'New Mission',
                description: '',
                logoUrl: '',
                bannerUrl: '',
                createdOn: new Date(),
                modifiedOn: new Date()
            });
        };
        
        service.delete = function(id) {
            return $http.delete(missionsUrl + '/' + id);
        };
        
        service.search = function(communityId, q) {
            return $http.get(communityMissions + '/' + communityId + '/missions/search?q=' + encodeURI(q));
        };

        return service;
    }
})();