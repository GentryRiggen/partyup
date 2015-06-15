(function () {
    'use strict';
    angular
        .module('partyUp')
        .service('FilesService', FilesService);

    FilesService.$inject = ['$http', 'API_URL'];
    function FilesService($http, API_URL) {
        var thisApiUrl = API_URL + '/files';
		var service = {};
        service.uploadFile = function (file) {
			var fd = new FormData();
			fd.append('file', file);
			return $http.post(thisApiUrl, fd, {
				withCredentials: true,
				headers: { 'Content-Type': undefined },
				transformRequest: angular.identity
			});
		};

        service.deleteFile = function (fileName) {
            return $http.delete(thisApiUrl + '?fileName=' + encodeURI(fileName));
        };
		
		return service;
	}
})();