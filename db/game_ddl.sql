CREATE TABLE Bet (
    Id SERIAL PRIMARY KEY,
    AccountId int NOT NULL,
    GameEventId int NOT NULL,
    Amount BIGINT DEFAULT 0,
    BetStatusId int NOT NULL,
    Payout BIGINT DEFAULT 0,
    CreateDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE Bet
ADD CONSTRAINT FK_GameEventId FOREIGN KEY(GameEventId) REFERENCES GameEvent(Id);

ALTER TABLE Bet
ADD CONSTRAINT FK_BetStatusId FOREIGN KEY(BetStatusId) REFERENCES BetStatus(Id);

CREATE TABLE BetStatus (
    Id SERIAL PRIMARY KEY,
    Description VARCHAR(50) NOT NULL
);

CREATE TABLE GameEvent (
    Id SERIAL PRIMARY KEY,
    Description VARCHAR(50) NOT NULL
);

CREATE TABLE GameEventPayout (
    GameEventId int PRIMARY KEY,
    PayoutOddsLeft int NOT NULL,
    PayoutOddsRight int NOT NULL
);

ALTER TABLE GameEventPayout
ADD CONSTRAINT FK_GameEventId_Payout FOREIGN KEY(GameEventId) REFERENCES GameEvent(Id);