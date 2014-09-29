SELECT * FROM Characters c
  INNER JOIN EncounterCharacters ec ON c.Id = ec.CharacterID
  WHERE ec.EncounterId = $encounterId
    AND c.UserId = $userId;
