(function () {
  'use strict';

  /* jshint -W117 */
  var eventModel = require('../models/event.model'),
    Q = require('q'),
    moment = require('moment');

  module.exports = function (dbPool) {
    var eventRepo = {};
    var db = require('../services/db.service')(dbPool);


    eventRepo.getRecentByMission = function (missionId, date) {
      var dfd = Q.defer(),
        recentDate = date ? date : moment.utc().subtract(10, 'minutes').format("YYYY-MM-DD HH:mm:ss"),
        query = 'SELECT e.* ' +
          'FROM event e ' +
          'LEFT JOIN event_user eu ON (eu.event_id = e.id) ' +
          'WHERE e.created_on >= "' + recentDate + '" AND mission_id = ' + missionId +
          ' HAVING COUNT(eu.user_id) > 0;';

      console.log(query);
      db.query(query).then(
        function (events) {
          var eventModels = [];
          for (var i = 0; i < events.length; i++) {
            eventModels.push(eventModel.toJson(events[i]));
          }
          dfd.resolve(eventModels);
        }, function (err) {
          dfd.reject(err);
        });

      return dfd.promise;
    };

    return eventRepo;
  };
})();
