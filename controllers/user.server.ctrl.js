(function () {
  'use strict';

  /* jshint -W117 */
  var express = require('express');

  module.exports = function (dbPool) {
    var userCtrl = express.Router();
    //var userRepo = require('../repositories/user.repo')(dbPool);

    userCtrl.route('/')
      .get(function (req, res) {
        if (req.currentUser) {
          return res.json(req.currentUser);
        } else {
          return res.status(401).send({error: 'Not logged in'});
        }
      });

    return userCtrl;
  };

})();
