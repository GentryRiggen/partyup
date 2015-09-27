(function () {
  'use strict';

  /* jshint -W117 */
  var express = require('express');

  module.exports = function (dbPool) {
    var authCtrl = express.Router();
    var userRepo = require('../repositories/user.repo')(dbPool);

    authCtrl.route('/')
      .post(function (req, res) {
        userRepo.authorizeUser(req.body.username, req.body.password).then(
          function(user) {
            res.json(user);
          }, function() {
            res.status(401).send({message: 'Invalid username and/or password'});
          });
      });

    authCtrl.route('/register')
      .post(function(req, res) {
        userRepo.create(req.body).then(
          function(result) {
            if (result.errors) {
              res.status(400).send({message: 'Failed to register new user', errors: result.errors});
            } else {
              userRepo.authorizeUser(req.body.username, req.body.password).then(
                function(user) {
                  res.json(user);
                }, function() {
                  res.status(401).send({message: 'Invalid username and/or password'});
                });
            }
          }, function(err) {
            console.log(err);
            res.status(500).send({message: 'Failed to register new user'});
          });
      });

    return authCtrl;
  };

})();
