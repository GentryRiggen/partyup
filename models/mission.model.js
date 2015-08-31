(function () {
  'use strict';

  /* jshint -W117 */
  exports.toJson = function (mission) {
    return {
      id: mission.id,
      gameId: mission.game_id,
      name: mission.name,
      description: mission.description,
      bannerUrl: mission.banner_url,
      published: mission.published
    };
  };
})();
