CREATE TABLE GameEventPayout (
    GameEventId int PRIMARY KEY,
    PayoutOddsLeft int NOT NULL,
    PayoutOddsRight int NOT NULL
);

ALTER TABLE dbo.GameEventPayout
ADD CONSTRAINT FK_GameEventId_Payout FOREIGN KEY(GameEventId) REFERENCES dbo.GameEvent(Id);