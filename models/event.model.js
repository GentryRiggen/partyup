(function () {
  'use strict';

  /* jshint -W117 */
  exports.toJson = function (event) {
    return {
      id: event.id,
      missionId: event.mission_id,
      userId: event.user_id,
      platformId: event.platform_id,
      desiredAmount: event.desired_amount,
      notes: event.notes,
      hasMic: event.has_mic === 0 ? false : true,
      language: event.language,
      createdOn: event.created_on,
      modifiedOn: event.modified_on
    };
  };
})();
