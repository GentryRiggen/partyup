(function () {
  'use strict';

  /* jshint -W117 */
  var gameModel = require('../models/game.model'),
    Q = require('q');

  module.exports = function (dbPool) {
    var gameRepo = {},
      db = require('../services/db.service')(dbPool);


    gameRepo.getAll = function (skip, take, q, all) {
      var dfd = Q.defer(),
        whereAll = all ? '1 = 1' : 'published = 1',
        searchQuery = '"%' + q + '%"';
      db.query('SELECT * FROM game WHERE name LIKE ' + searchQuery + ' AND ' + whereAll).then(
        function (games) {
          var gameModels = [];
          for (var i = 0; i < games.length; i++) {
            gameModels.push(gameModel.toJson(games[i]));
          }
          dfd.resolve(gameModels);
        }, function (err) {
          dfd.reject(err);
        });

      return dfd.promise;
    };

    gameRepo.getById = function (id) {
      var dfd = Q.defer();
      db.query('SELECT * FROM game WHERE id = ' + id).then(
        function (games) {
          if (games.length > 0) {
            var game = gameModel.toJson((games[0]));
            dfd.resolve(game);
          } else {
            dfd.reject('Game not found');
          }
        }, function (err) {
          dfd.reject(err);
        });

      return dfd.promise;
    };

    gameRepo.save = function (id, game) {
      var dfd = Q.defer();
      var query = "UPDATE game SET " +
        "name = '" + dbPool.escape(game.name) + "'," +
        "description = '" + dbPool.escape(game.description) + "'," +
        "logo_url = '" + dbPool.escape(game.logoUrl) + "'," +
        "banner_url = '" + dbPool.escape(game.bannerUrl) + "'," +
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

    return gameRepo;
  };
})();
