CREATE DATABASE Balance;

USE Balance;

GO

CREATE TABLE Balance (
    AccountId int NOT NULL PRIMARY KEY,
    CurrentBalance BIGINT DEFAULT 0,
    CurrentFloor BIGINT DEFAULT 0,
    CreateDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE ActivityType (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Description VARCHAR(50) NOT NULL
);

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
