CREATE TABLE [dbo].[InitiativeTokens]
(
	TokenId int IDENTITY(1,1) PRIMARY KEY,
	RoundId int NOT NULL FOREIGN KEY REFERENCES EncounterRounds(RoundId),
	CharacterId int NOT NULL,
	Initiative int NOT NULL,
	TieBreaker float NOT NULL,
	FinalInitiative AS Initiative + TieBreaker
)
