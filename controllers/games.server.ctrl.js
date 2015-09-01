(function () {
  'use strict';

  /* jshint -W117 */
  var express = require('express');

  module.exports = function (dbPool) {
    var gameCtrl = express.Router(),
      gameModel = require('../models/game.model'),
      gameRepo = require('../repositories/game.repo')(dbPool),
      missionRepo = require('../repositories/mission.repo')(dbPool),
      eventRepo = require('../repositories/event.repo')(dbPool);

    gameCtrl.route('/')
      .get(function(req, res) {
        var query = 'q' in req.query ? req.query.q : '';
        gameRepo.getAll(0, 100, query, false).then(
          function(games) {
            res.json(games);
          }, function(err) {
            console.log(err);
          });
      });

    gameCtrl.route('/:id')
      .get(function(req, res) {
        gameRepo.getById(req.params.id).then(
          function(game) {
            game.missions = gameModel.getMissionsUrl(game.id, req.headers.host);
            res.json(game);
          }, function(err) {
            console.log(err);
          });
      });

    gameCtrl.route('/:id/missions')
      .get(function(req, res) {
        gameRepo.getById(req.params.id).then(
          function(game) {
            missionRepo.getAllByGame(game.id, false).then(
              function(missions) {
                game.missions = missions;
                res.json(game);
              }, function(err){
                console.log(err);
              });
          }, function(err) {
            console.log(err);
          });
      });

    gameCtrl.route('/:gameId/missions/:missionId')
      .get(function(req, res) {
        gameRepo.getById(req.params.gameId).then(
          function(game) {
            missionRepo.getById(req.params.missionId, false).then(
              function(mission) {
                eventRepo.getRecentByMission(mission.id).then(
                  function(events) {
                    mission.events = events;
                    game.mission = mission;
                    res.json(game);
                  }, function(err) {
                    console.log(err);
                    res.status(500).send({message: 'Failed to get mission events'});
                  });
              }, function(err){
                console.log(err);
                res.status(500).send({message: 'Failed to get mission'});
              });
          }, function(err) {
            console.log(err);
            res.status(500).send({message: 'Failed to get game'});
          });
      });

    return gameCtrl;
  };

})();
