(function () {
  'use strict';
  /* jshint -W117 */
  var express = require('express'),
    mysql = require('mysql'),
    conf = require('./config/conf'),
    jwt = require('./services/jwt.service'),
    bodyParser = require('body-parser');

  // ENVIRONMENT SETUP
  var app = express(),
    port = process.env.PORT || 8888;
  app.use(bodyParser.json());
  app.use(bodyParser.urlencoded({
    extended: true
  }));

  // DB CONNECTIONS
  var dbPool = mysql.createPool(conf.databaseConfig);

  // CORS
  app.use('/api', function(req, res, next) {
    res.header('Access-Control-Allow-Origin', '*');
    res.header('Access-Control-Allow-Headers', 'X-Requested-With, Content-Type, Authorization');
    next();
  });

  // TOKEN FILTER
  app.use('/api', function (req, res, next) {
    jwt.tokenFilter(req, dbPool).then(
      function () {
        next();
      }, function () {
        next();
      });
  });

  // ROUTES
  app.use('/api/user', require('./controllers/user.server.ctrl.js')(dbPool));
  app.use('/api/auth', require('./controllers/auth.server.ctrl.js')(dbPool));
  app.use('/api/games', require('./controllers/games.server.ctrl.js')(dbPool));

  // MISC
  function twoDigits(d) {
    if (0 <= d && d < 10) return "0" + d.toString();
    if (-10 < d && d < 0) return "-0" + (-1 * d).toString();
    return d.toString();
  }

  Date.prototype.toMysqlFormat = function () {
    return this.getUTCFullYear() + "-" + twoDigits(1 + this.getUTCMonth()) + "-" + twoDigits(this.getUTCDate()) + " " + twoDigits(this.getUTCHours()) + ":" + twoDigits(this.getUTCMinutes()) + ":" + twoDigits(this.getUTCSeconds());
  };

  // START THE APP
  app.listen(port, function () {
    console.log('Listening on PORT: ', port);
  });
})();
