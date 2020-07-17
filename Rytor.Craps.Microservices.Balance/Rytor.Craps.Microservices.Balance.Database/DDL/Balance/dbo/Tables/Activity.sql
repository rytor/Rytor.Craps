CREATE TABLE Activity (
    Id int IDENTITY(1,1) PRIMARY KEY,
    AccountId int NOT NULL,
    ActivityTypeId int NOT NULL,
    Amount BIGINT NOT NULL,
    CreateDate DATETIME DEFAULT GETDATE()
);

ALTER TABLE dbo.Activity
ADD CONSTRAINT FK_AccountId FOREIGN KEY(AccountId) REFERENCES dbo.Balance(AccountId);

ALTER TABLE dbo.Activity
ADD CONSTRAINT FK_ActivityTypeId FOREIGN KEY(ActivityTypeId) REFERENCES dbo.ActivityType(Id);