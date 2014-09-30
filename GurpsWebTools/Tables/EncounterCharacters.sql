CREATE TABLE [dbo].[EncounterCharacters]
(
	Id int IDENTITY(1,1) PRIMARY KEY,
    EncounterId int NOT NULL FOREIGN KEY REFERENCES [dbo].[Encounters]([EncounterId]),
    CharacterId int NOT NULL FOREIGN KEY REFERENCES Characters(CharacterId),
	HpMax int,
	FpMax int,
    HpDamage int DEFAULT 0,
    FpDamage int DEFAULT 0,
	HpCurrent AS HpMax - HpDamage,
	FpCurrent AS FpMax - FpDamage,
	HpThreshold AS [dbo].[udfGetThreshold](HpMax, HpCurrent),
	FpThreshold AS [dbo].[udfGetThreshold](FpMax, FpCurrent),
	InitiativeBonus int, 
    IsVisible bit DEFAULT 1,
    IsKO bit DEFAULT 0,
)
