(function () {
    'use strict';
    angular.module('partyUp').service('CommunitiesService', CommunitiesService);

    CommunitiesService.$inject = ['API_URL', '$http', '$q'];
    function CommunitiesService(API_URL, $http, $q) {
        var service = {};
		service.url = API_URL + '/communities';
		
		service.getAll = function() {
			return $http.get(service.url);	
		};
        
        service.getById = function(id) {
            return $http.get(service.url + '/' + id);  
        };
        
        service.update = function(community) {
            return $http.put(service.url + '/' + community.id, community);  
        };
        
        service.createNew = function() {
            return $http.post(service.url + '/createnew', {
                name: 'New Community',
                description: '',
                logoUrl: '',
                bannerUrl: '',
                createdOn: new Date(),
                modifiedOn: new Date()
            });
        };
        
        service.delete = function(id) {
            return $http.delete(service.url + '/' + id);
        };

        return service;
    }
})();