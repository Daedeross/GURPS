--SP Create EncounterCharacter

CREATE PROCEDURE createEncounterCharacter
	@encId int,
	@charId int
AS
	INSERT INTO EncounterCharacters (EncounterId, CharacterId, 	HpMax, FpMax, InitiativeBonus)
	SELECT TOP(1) @encId, @charId, HP, FP, InitiativeBonus FROM Characters
	WHERE CharacterId = @charId;
GO