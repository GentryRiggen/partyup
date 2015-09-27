#******************** USER ********************#
CREATE TABLE user
(
  id          INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  first_name  VARCHAR(64),
  last_name   VARCHAR(64),
  email       VARCHAR(128),
  username    VARCHAR(64)     NOT NULL,
  password    VARCHAR(256)    NOT NULL,
  xbl_tag     VARCHAR(64),
  psn_tag     VARCHAR(64),
  steam_tag   VARCHAR(64),
  created_on  TIMESTAMP,
  modified_on TIMESTAMP
);
ALTER TABLE user ADD CONSTRAINT unique_id UNIQUE (id);
ALTER TABLE user ADD CONSTRAINT unique_username UNIQUE (username);



#******************** ROLE ********************#
CREATE TABLE role
(
  id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  title VARCHAR(32) NOT NULL
);
CREATE UNIQUE INDEX unique_id ON role (id);
CREATE UNIQUE INDEX unique_title ON role (title);



#******************** USER_ROLE ********************#
CREATE TABLE user_role
(
  id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  user_id INT NOT NULL,
  role_id INT NOT NULL
);
CREATE UNIQUE INDEX unique_id ON user_role (id);
ALTER TABLE user_role ADD FOREIGN KEY (user_id) REFERENCES user(id);
ALTER TABLE user_role ADD FOREIGN KEY (role_id) REFERENCES role(id);



#******************** GAME ********************#
CREATE TABLE game
(
  id          INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  name        VARCHAR(256)    NOT NULL,
  description VARCHAR(1048),
  logo_url    VARCHAR(256),
  banner_url  VARCHAR(256),
  published   INT DEFAULT 0 NOT NULL
);
CREATE UNIQUE INDEX unique_id ON game(id);



#******************** PLATFORM ********************#
CREATE TABLE platform
(
  id   INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  name VARCHAR(256) DEFAULT '' NOT NULL
);
CREATE UNIQUE INDEX unique_id ON platform(id);
CREATE UNIQUE INDEX unique_title ON platform(name);



#******************** MISSION ********************#
CREATE TABLE mission
(
  id          INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  game_id     INT             NOT NULL,
  name        VARCHAR(256)    NOT NULL,
  description VARCHAR(1048),
  banner_url  VARCHAR(256),
  published   INT DEFAULT 0 NOT NULL
);
CREATE UNIQUE INDEX unique_id ON mission(id);
ALTER TABLE mission ADD FOREIGN KEY (game_id) REFERENCES game(id);



#******************** EVENT ********************#
CREATE TABLE event
(
  id             INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  mission_id     INT NOT NULL,
  user_id        INT NOT NULL,
  platform_id    INT NOT NULL,
  desired_amount INT DEFAULT 1 NOT NULL,
  notes          VARCHAR(1048) DEFAULT '' NOT NULL,
  has_mic        INT DEFAULT 1 NOT NULL,
  language       VARCHAR(64) DEFAULT 'ENGLISH' NOT NULL,
  created_on     TIMESTAMP,
  modified_on    TIMESTAMP
);
CREATE UNIQUE INDEX unique_id ON event(id);
ALTER TABLE event ADD FOREIGN KEY (mission_id) REFERENCES mission(id);
ALTER TABLE event ADD FOREIGN KEY (user_id) REFERENCES user(id);
ALTER TABLE event ADD FOREIGN KEY (platform_id) REFERENCES platform(id);


#******************** EVENT_USER ********************#
CREATE TABLE event_user
(
  id       INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
  user_id  INT NOT NULL,
  event_id INT NOT NULL,
  active   INT DEFAULT 1 NOT NULL,
  created_on  TIMESTAMP,
  modified_on TIMESTAMP
);
CREATE UNIQUE INDEX event_user ON event_user(id);
ALTER TABLE event_user ADD FOREIGN KEY (user_id) REFERENCES user(id);
ALTER TABLE event_user ADD FOREIGN KEY (event_id) REFERENCES event(id);



#******************** DEFAULT USER AND ROLES ********************#
INSERT INTO role VALUES (DEFAULT, 'Admin');
SET @adminRoleId = LAST_INSERT_ID();

INSERT INTO role VALUES (DEFAULT, 'Moderator');
SET @moderatorRoleId = LAST_INSERT_ID();

INSERT INTO user VALUES (
  DEFAULT, 'Gentry', 'Riggen', 'gentry@gentryriggen.com', 'gentry',
  '$2a$10$EPhjnF1MAD1GDpTC0Ax/aOu01koEOu6EDwf.CigUGoXjoazSaJtGq',
  'ST3ALTHY PANDA', '', '', NOW(), NOW()
);
SET @defaultUserId = LAST_INSERT_ID();

INSERT INTO user_role VALUES
  (DEFAULT, @defaultUserId, @adminRoleId),
  (DEFAULT, @defaultUserId, @moderatorRoleId);



#******************** DEFAULT PLATFORMS ********************#
INSERT INTO platform VALUES (DEFAULT, 'Xbox One');
SET @platformXboxOneId = LAST_INSERT_ID();

INSERT INTO platform VALUES (DEFAULT, 'Xbox 360');
SET @platformXbox360Id = LAST_INSERT_ID();

INSERT INTO platform VALUES (DEFAULT, 'Playstation 4');
SET @platformPS4Id = LAST_INSERT_ID();

INSERT INTO platform VALUES (DEFAULT, 'Playstation 3');
SET @platformPS3Id = LAST_INSERT_ID();



#******************** DEFAULT GAMES ********************#
INSERT INTO game VALUES (DEFAULT, 'Destiny', 'Destiny by Bungie/Activtion',
                         'http://cdn.gentryriggen.com/partyup/destinyLogo.jpg',
                         'http://cdn.gentryriggen.com/partyup/destinyBanner.png', 1);
SET @gameDestinyId = LAST_INSERT_ID();



#******************** DEFAULT MISSIONS ********************#
INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Vault of Glass (NORMAL)',
                         'Vault of Glass RAID on Normal Mode.',
                         'http://cdn.gentryriggen.com/partyup/VoG.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Vault of Glass (HARD)',
                            'Vault of Glass RAID on Hard Mode.',
                            'http://cdn.gentryriggen.com/partyup/VoG.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Prison of Elders (lvl 28)',
                            'Prison of Elders (lvl 28).',
                            'http://cdn.gentryriggen.com/partyup/poe.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Prison of Elders (lvl 32)',
                            'Prison of Elders (lvl 32).',
                            'http://cdn.gentryriggen.com/partyup/poe.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Prison of Elders (lvl 34)',
                            'Prison of Elders (lvl 34).',
                            'http://cdn.gentryriggen.com/partyup/poe.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Prison of Elders (lvl 35)',
                            'Prison of Elders (lvl 35).',
                            'http://cdn.gentryriggen.com/partyup/poe.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, "Crota's End (NORMAL)",
                            "Crota's End Normal Mode.",
                            'http://cdn.gentryriggen.com/partyup/crota.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, "Crota's End (HARD)",
                            "Crota's End Hard Mode.",
                            'http://cdn.gentryriggen.com/partyup/crota.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Trials of Osiris',
                            'Trials of Osiris.',
                            'http://cdn.gentryriggen.com/partyup/trialsofosiris.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Weekly Heroic Strike (24)',
                            'Weekly Heroic Strike (24)',
                            'http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Weekly Heroic Strike (28)',
                            'Weekly Heroic Strike (28)',
                            'http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Weekly Heroic Strike (30)',
                            'Weekly Heroic Strike (30)',
                            'http://cdn.gentryriggen.com/partyup/weeklyheroicstrike.jpg', 1);

INSERT INTO mission VALUES (DEFAULT, @gameDestinyId, 'Weekly Nightfall Strike',
                            'Weekly Nightfall Strike',
                            'http://cdn.gentryriggen.com/partyup/weeklynightfallstrike.jpg', 1);
