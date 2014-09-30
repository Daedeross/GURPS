CREATE TABLE [dbo].[EncounterGMs]
(
	EncounterId int NOT NULL,
	GmID INT NOT NULL, 
    CONSTRAINT [PK_EncounterGMs] PRIMARY KEY CLUSTERED ([EncounterID] ASC, [GmID] ASC), 
    CONSTRAINT [FK_EncounterGMs_Encounter] FOREIGN KEY ([EncounterID]) REFERENCES [Encounters]([EncounterID]),
    CONSTRAINT [FK_EncounterGMs_User] FOREIGN KEY ([GmID]) REFERENCES [Users]([UserID]), 
)
