SELECT ec.Id, ec.Alias, ec.HpThreshold, ec.FpThreshold, ec.IsKO FROM Characters c
  INNER JOIN EncounterCharacters ec ON c.Id = ec.CharacterID
  WHERE ec.EncounterId = $encounterId
    AND c.Visible = TRUE
    AND c.UserId != $userId;
