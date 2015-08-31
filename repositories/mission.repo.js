(function () {
  'use strict';

  /* jshint -W117 */
  var missionModel = require('../models/mission.model'),
    Q = require('q');

  module.exports = function (dbPool) {
    var missionRepo = {};
    var db = require('../services/db.service')(dbPool);


    missionRepo.getAll = function (skip, take, q, all) {
      var dfd = Q.defer(),
        whereAll = all ? '1 = 1' : 'published = 1',
        searchQuery = '"%' + q + '%"';
      db.query('SELECT * FROM mission WHERE name LIKE ' + searchQuery + ' AND ' + whereAll).then(
        function (missions) {
          var missionModels = [];
          for (var i = 0; i < missions.length; i++) {
            missionModels.push(missionModel.toJson(missions[i]));
          }
          dfd.resolve(missionModels);
        }, function (err) {
          dfd.reject(err);
        });

      return dfd.promise;
    };

    missionRepo.getAllByGame = function (gameId, all) {
      var dfd = Q.defer(),
        whereAll = all ? '1 = 1' : 'published = 1';
      db.query('SELECT * FROM mission WHERE game_id = ' + gameId + ' AND ' + whereAll).then(
        function (missions) {
          var missionModels = [];
          for (var i = 0; i < missions.length; i++) {
            missionModels.push(missionModel.toJson(missions[i]));
          }
          dfd.resolve(missionModels);
        }, function (err) {
          dfd.reject(err);
        });

      return dfd.promise;
    };

    missionRepo.getById = function (id) {
      var dfd = Q.defer();
      db.query('SELECT * FROM mission WHERE id = ' + id).then(
        function (missions) {
          if (missions.length > 0) {
            dfd.resolve(missionModel.toJson(missions[0]));
          } else {
            dfd.reject('Mission not found');
          }
        }, function (err) {
          dfd.reject(err);
        });

      return dfd.promise;
    };

    missionRepo.save = function (id, mission) {
      var dfd = Q.defer();
      var query = "UPDATE mission SET " +
        "game_id = '" + dbPool.escape(mission.game_id) + "'," +
        "name = '" + dbPool.escape(mission.name) + "'," +
        "description = '" + dbPool.escape(mission.description) + "'," +
        "banner_url = '" + dbPool.escape(mission.bannerUrl) + "', " +
        "published = '" + dbPool.escape(mission.published) + "' " +
        "WHERE id = " + id;
      db.query(query).then(
        function () {
          dfd.resolve();
        }, function (err) {
          dfd.reject(err);
        });

      return dfd.promise;
    };

    return missionRepo;
  };
})();
