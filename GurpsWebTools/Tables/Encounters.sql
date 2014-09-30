CREATE TABLE [dbo].[Encounters]
(
	EncounterID int IDENTITY(1,1) PRIMARY KEY,
	GmID INT NOT NULL, 
    CONSTRAINT [FK_Encounters_GM] FOREIGN KEY ([GmID]) REFERENCES [dbo].[Users]([UserID]),
)
