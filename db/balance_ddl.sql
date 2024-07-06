CREATE TABLE Balance (
    AccountId SERIAL PRIMARY KEY,
    CurrentBalance BIGINT DEFAULT 0,
    CurrentFloor BIGINT DEFAULT 0,
    CreateDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ActivityType (
    Id SERIAL PRIMARY KEY,
    Description VARCHAR(50) NOT NULL
);

CREATE TABLE Activity (
    Id SERIAL PRIMARY KEY,
    AccountId INT NOT NULL,
    ActivityTypeId INT NOT NULL,
    Amount BIGINT NOT NULL,
    CreateDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AccountId) REFERENCES Balance(AccountId),
    FOREIGN KEY (ActivityTypeId) REFERENCES ActivityType(Id)
);