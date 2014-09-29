CREATE TABLE [dbo].[EncounterPlayers]
(
	EncounterId int NOT NULL,
	PlayerID INT NOT NULL, 
    CONSTRAINT [PK_EncounterPlayers] PRIMARY KEY CLUSTERED ([EncounterID] ASC, [PlayerID] ASC), 
    CONSTRAINT [FK_EncounterPlayers_Encounter] FOREIGN KEY ([EncounterID]) REFERENCES [Encounters]([EncounterID]),
    CONSTRAINT [FK_EncounterPlayers_User] FOREIGN KEY ([PlayerID]) REFERENCES [Users]([UserID]), 
)
