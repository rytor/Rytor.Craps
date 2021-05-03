CREATE DATABASE Game;

USE Game; 

GO

CREATE TABLE BetStatus (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Description VARCHAR(50) NOT NULL
);

insert into dbo.BetStatus (Description) VALUES ('Open')
insert into dbo.BetStatus (Description) VALUES ('Cancelled')
insert into dbo.BetStatus (Description) VALUES ('Push')
insert into dbo.BetStatus (Description) VALUES ('Won')
insert into dbo.BetStatus (Description) VALUES ('Lost')

CREATE TABLE GameEvent (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Description VARCHAR(50) NOT NULL
);

insert into dbo.GameEvent (Description) VALUES ('Pass')
insert into dbo.GameEvent (Description) VALUES ('DontPass')
insert into dbo.GameEvent (Description) VALUES ('Field')
insert into dbo.GameEvent (Description) VALUES ('BigSix')
insert into dbo.GameEvent (Description) VALUES ('BigEight')
insert into dbo.GameEvent (Description) VALUES ('Come')
insert into dbo.GameEvent (Description) VALUES ('DontCome')
insert into dbo.GameEvent (Description) VALUES ('Four')
insert into dbo.GameEvent (Description) VALUES ('Five')
insert into dbo.GameEvent (Description) VALUES ('Six')
insert into dbo.GameEvent (Description) VALUES ('Eight')
insert into dbo.GameEvent (Description) VALUES ('Nine')
insert into dbo.GameEvent (Description) VALUES ('Ten')
insert into dbo.GameEvent (Description) VALUES ('C')
insert into dbo.GameEvent (Description) VALUES ('E')
insert into dbo.GameEvent (Description) VALUES ('Seven')
insert into dbo.GameEvent (Description) VALUES ('Craps')
insert into dbo.GameEvent (Description) VALUES ('HardFour')
insert into dbo.GameEvent (Description) VALUES ('HardSix')
insert into dbo.GameEvent (Description) VALUES ('HardEight')
insert into dbo.GameEvent (Description) VALUES ('HardTen')
insert into dbo.GameEvent (Description) VALUES ('Three')
insert into dbo.GameEvent (Description) VALUES ('Two')
insert into dbo.GameEvent (Description) VALUES ('Twelve')
insert into dbo.GameEvent (Description) VALUES ('Eleven')

CREATE TABLE GameEventPayout (
    GameEventId int PRIMARY KEY,
    PayoutOddsLeft int NOT NULL,
    PayoutOddsRight int NOT NULL
);

ALTER TABLE dbo.GameEventPayout
ADD CONSTRAINT FK_GameEventId_Payout FOREIGN KEY(GameEventId) REFERENCES dbo.GameEvent(Id);

insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (1, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (2, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (3, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (4, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (5, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (6, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (7, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (8, 9, 5);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (9, 7, 5);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (10, 7, 6);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (11, 7, 6);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (12, 7, 5);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (13, 9, 5);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (14, 3, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (15, 7, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (16, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (17, 1, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (18, 7, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (19, 9, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (20, 9, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (21, 7, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (22, 15, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (23, 30, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (24, 30, 1);
insert into dbo.GameEventPayout (GameEventId, PayoutOddsLeft, PayoutOddsRight) VALUES (25, 15, 1);

CREATE TABLE Bet (
    AccountId int NOT NULL PRIMARY KEY,
    GameEventId int NOT NULL,
    Amount BIGINT DEFAULT 0,
    BetStatusId int NOT NULL,
    CreateDate DATETIME DEFAULT GETDATE()
);

ALTER TABLE dbo.Bet
ADD CONSTRAINT FK_GameEventId FOREIGN KEY(GameEventId) REFERENCES dbo.GameEvent(Id);

ALTER TABLE dbo.Bet
ADD CONSTRAINT FK_BetStatusId FOREIGN KEY(BetStatusId) REFERENCES dbo.BetStatus(Id);