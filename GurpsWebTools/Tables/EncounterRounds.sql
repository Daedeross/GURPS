CREATE TABLE [dbo].[EncounterRounds]
(
	RoundId int IDENTITY(1,1) PRIMARY KEY,
	EncounterId int NOT NULL FOREIGN KEY REFERENCES Encounters(EncounterId),
	RoundNumber int NOT NULL, 
    CONSTRAINT [AK_EncounterRounds] UNIQUE CLUSTERED ([EncounterId] ASC, [RoundNumber] ASC), 
)
