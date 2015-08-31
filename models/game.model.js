(function () {
  'use strict';

  /* jshint -W117 */
  exports.toJson = function (game) {
    var gameModel = {
      id: game.id,
      name: game.name,
      description: game.description,
      logoUrl: game.logo_url,
      bannerUrl: game.banner_url,
      published: game.published
    };

    return gameModel;
  };

  /* jshint -W117 */
  exports.getMissionsUrl = function(id, hostname) {
    return 'http://' + hostname + '/api/games/' + id + '/missions';
  };
})();
