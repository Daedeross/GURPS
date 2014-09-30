CREATE TABLE [dbo].[Characters]
(
	CharacterId int IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES [dbo].[Users]([UserId]),
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
	InitiativeBonus int,
)

GO

CREATE INDEX [IX_Characters_Name] ON [dbo].[Characters] ([Name])
