CREATE TABLE Balance (
    AccountId int NOT NULL PRIMARY KEY,
    CurrentBalance BIGINT DEFAULT 0,
    CurrentFloor BIGINT DEFAULT 0,
    CreateDate DATETIME DEFAULT GETDATE()
);