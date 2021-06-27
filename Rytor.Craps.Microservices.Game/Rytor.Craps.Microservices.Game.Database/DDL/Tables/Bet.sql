CREATE TABLE Bet (
    Id int IDENTITY(1,1) PRIMARY KEY,
    AccountId int NOT NULL,
    GameEventId int NOT NULL,
    Amount BIGINT DEFAULT 0,
    BetStatusId int NOT NULL,
    Payout BIGINT DEFAULT 0,
    CreateDate DATETIME DEFAULT GETDATE()
);

ALTER TABLE dbo.Bet
ADD CONSTRAINT FK_GameEventId FOREIGN KEY(GameEventId) REFERENCES dbo.GameEvent(Id);

ALTER TABLE dbo.Bet
ADD CONSTRAINT FK_BetStatusId FOREIGN KEY(BetStatusId) REFERENCES dbo.BetStatus(Id);