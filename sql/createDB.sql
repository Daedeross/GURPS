CREATE TABLE Characters
{
    CharacterId int IDENTITY(1,1) PRIMARY KEY,
    UserId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(Id),
    Name nvarchar(255),
	Age nvarchar(16),
	Height nvarchar(10),
	Weight nvarchar(10),
	Description nvarchar(max),
	-- Basic Stuff
	ST int,
	DX int,
	IQ int,
	HT int,
	HP int
	FP int,
	Per int,
	Will int,
	-- Encounter Stuff
	BasicSpeed float(24),
	InitiativeBonus int
};

CREATE TABLE EncounterCharacters
{
    Id int IDENTITY(1,1) PRIMARY KEY,
    EncounterId int NOT NULL FOREIGN KEY REFERENCES Encounters(EncounterId),
    CharacterId int NOT NULL FOREIGN KEY REFERENCES Characters(CharacterId),
	HpMax int,
	FpMax int,
    HpDamage int DEFAULT 0,
    FpDamage int DEFAULT 0,
	HpCurrent AS HpMax - HpDamage,
	FpCurrent AS FpMax - FpDamage,
	InitiativeBonus int,
    IsVisible bit DEFAULT TRUE,
    IsKO bit DEFAULT FALSE,
};

CREATE FUNCTION getThreshold(@ecId int, @maxVal int, @curVal int)
RETURNS int
AS
BEGIN
    DECLARE @ret int;
	DECLARE @thresh float;
	SET @thresh = (@maxVal - @curVal) / CONVERT (float, @maxVal);
	SET @ret = CASE
		WHERE @thresh > 0.3333 THEN 2
		WHERE @thresh > 0 THEN 1
		ELSE CEILING(@thresh)
	END;
	RETURN @ret;
END;

CREATE VIEW EncounterGMCharacters
{
    SELECT * FROM Characters c
        INNER JOIN EncounterCharacters ec ON c.Id = ec.CharacterID
};