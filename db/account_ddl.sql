CREATE TABLE Account (
    Id SERIAL PRIMARY KEY,
    TwitchId VARCHAR(50) NOT NULL,
    CreateDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);