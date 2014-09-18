CREATE FUNCTION getThreshold(@maxVal int, @curVal int)
RETURNS int
AS
BEGIN
    DECLARE @ret int;
	DECLARE @thresh float;
	SET @thresh = (@maxVal - @curVal) / CONVERT (float, @maxVal);
	SET @ret = CASE
		WHEN @thresh > 0.3333 THEN 2
		WHEN @thresh > 0 THEN 1
		ELSE CEILING(@thresh)
	END;
	RETURN @ret;
END
GO

CREATE TABLE Characters
(
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
	HP int,
	FP int,
	Per int,
	Will int,
	-- Encounter Stuff
	BasicSpeed float(24),
	InitiativeBonus int
)

CREATE TABLE GmCharacters
(
	GmId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(Id),
	CharacterId int FOREIGN KEY REFERENCES Characters(CharacterId),
)

CREATE TABLE EncounterCharacters
(
    Id int IDENTITY(1,1) PRIMARY KEY,
    EncounterId int NOT NULL FOREIGN KEY REFERENCES Encounters(EncounterId),
    CharacterId int NOT NULL FOREIGN KEY REFERENCES Characters(CharacterId),
	HpMax int,
	FpMax int,
    HpDamage int DEFAULT 0,
    FpDamage int DEFAULT 0,
	HpCurrent AS HpMax - HpDamage,
	FpCurrent AS FpMax - FpDamage,
	HpThreshold AS getThreshold(HpMax, HpCurrent),
	FpThreshold AS getThreshold(FpMax, FpCurrent),
	InitiativeBonus int, 
    IsVisible bit DEFAULT TRUE,
    IsKO bit DEFAULT FALSE,
);

CREATE TABLE Encounters 
(
	EncounterId int IDENTITY(1,1) PRIMARy KEY,
	Gm UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(Id),
);

CREATE TABLE EcounterPlayers
(
    EncounterId int NOT NULL FOREIGN KEY REFERENCES Encounters(EncounterId),
	PlayerId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(Id),
);

CREATE TABLE EncounterGms
(
    EncounterId int NOT NULL FOREIGN KEY REFERENCES Encounters(EncounterId),
	Gm UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Users(Id),
);

CREATE TABLE EncounterRounds
(
	RoundId int IDENTITY(1,1) PRIMARY KEY,
	EncounterId int NOT NULL FOREIGN KEY REFERENCES Encounters(EncounterId),
	RoundNumer int
);

CREATE TABLE InitiativeTokens
(
	TokenId int IDENTITY(1,1) PRIMARY KEY,
	RoundId int NOT NULL FOREIGN KEY REFERENCES EncounterRounds(RoundId),
	CharacterId int NOT NULL,
	Initiative int NOT NULL,
	TieBreaker float NOT NULL,
	FinalInitiative AS Initiative + TieBreaker
);

--CREATE VIEW EncounterGMCharacters
--(
--    SELECT * FROM Characters c
--        INNER JOIN EncounterCharacters ec ON c.Id = ec.CharacterID
--);
