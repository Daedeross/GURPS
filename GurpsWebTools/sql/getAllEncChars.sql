CREATE PROCEDURE getGMEncounterCharacters
	@encId int
AS
SELECT * FROM Characters c
	INNER JOIN EncounterCharacters ec ON c.Id = ec.CharacterID
	WHERE ec.EncounterId = $encounterId;
GO