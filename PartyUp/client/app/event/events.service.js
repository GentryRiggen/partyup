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

        service.esnureEventModel = function (event) {
            var validEvent = {};

            if (angular.isDefined(event.Id)) validEvent.id = event.Id;
            else validEvent.id = event.Id;

            if (angular.isDefined(event.CreatedOn)) validEvent.createdOn = event.CreatedOn;
            else validEvent.createdOn = event.createdOn;

            if (angular.isDefined(event.DesiredAmount)) validEvent.desiredAmount = event.DesiredAmount;
            else validEvent.desiredAmount = event.desiredAmount;

            if (angular.isDefined(event.EventParticipants)) validEvent.eventParticipants = event.EventParticipants;
            else validEvent.eventParticipants = event.eventParticipants;

            if (angular.isDefined(event.HasMic)) validEvent.hasMic = event.HasMic;
            else validEvent.hasMic = event.hasMic;

            if (angular.isDefined(event.Language)) validEvent.language = event.Language;
            else validEvent.language = event.language;

            if (angular.isDefined(event.Mission)) validEvent.mission = event.Mission;
            else validEvent.mission = event.mission;

            if (angular.isDefined(event.Notes)) validEvent.notes = event.Notes;
            else validEvent.notes = event.notes;

            if (angular.isDefined(event.Organizer)) validEvent.organizer = event.Organizer;
            else validEvent.organizer = event.organizer;

            if (angular.isDefined(event.Platform)) validEvent.platform = event.Platform;
            else validEvent.platform = event.platform;

            return event;
        };

        service.esnureEventModels = function (events) {
            var validEvents = [];
            angular.forEach(events, function (event) {
                validEvents.push(service.esnureEventModel(event));
            });

            return validEvents;
        };
		
        return service;
    }
})();