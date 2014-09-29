CREATE TABLE [dbo].[GMCharacters]
(
	[GmID] INT NOT NULL,
	[CharacterID] INT NOT NULL,
	CONSTRAINT [PK_GMCharacters_GmID_RoleID] PRIMARY KEY CLUSTERED ([GmID] ASC, [CharacterID] ASC),
	CONSTRAINT [FK_GMCharacters_GmID] FOREIGN KEY ([GmID]) REFERENCES [dbo].[Users] ([UserID]),
	CONSTRAINT [FK_GMCharacters_CharacterID] FOREIGN KEY ([CharacterID]) REFERENCES [dbo].[Characters] ([CharacterID]),
)
