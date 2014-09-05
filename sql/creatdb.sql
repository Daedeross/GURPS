CREATE TABLE Characters
{
    CharacterId int IDENTITY(1,1) PRIMARY KEY,
    UserId string FOREIGN KEY REFERENCES Users(Id),
    Name varchar(255),
};

CREATE TABLE EncounterCharacters
{
    Id int IDENTITY(1,1) PRIMARY KEY,
    EncounterId int NOT NULL FOREIGN KEY REFERENCES Encounters(EncounterId),
    CharacterId int NOT NULL FOREIGN KEY REFERENCES Characters(CharacterId),
    HpDamage int DEFAULT 0,
    FpDamage inr DEFAULT 0,
    IsVisible bit DEFAULT TRUE,
    IsKO bit DEFAULT FALSE,
};

CREATE FUNCTION getThreshold(@ecId int, @maxVal int, @curVal int)
RETURNS int
AS
BEGIN
    DECLARE @thresh int;
    IFF
    SET @thresh = CEILING((@maxVal - @curVal)/@maxVal;
    CASE 
END;

CREATE VIEW EncounterGMCharacters
{
    SELECT * FROM Characters c
        INNER JOIN EncounterCharacters ec ON c.Id = ec.CharacterID
};
