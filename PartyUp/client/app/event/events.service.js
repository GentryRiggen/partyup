(function () {
    'use strict';
    angular.module('partyUp').service('EventsService', EventsService);

    EventsService.$inject = ['API_URL', '$http'];
    function EventsService(API_URL, $http) {
        var service = {};
        var missionsUrl = API_URL + '/missions';
		var eventsUrl = API_URL + '/events';
		
		service.getAllByMission = function(missionId) {
			return $http.get(missionsUrl + '/' + missionId + '/events');	
		};
        
        service.getById = function(id) {
            return $http.get(eventsUrl + '/' + id);  
        };
		
        return service;
    }
})();