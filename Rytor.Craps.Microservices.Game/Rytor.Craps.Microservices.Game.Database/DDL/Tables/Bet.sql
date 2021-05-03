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