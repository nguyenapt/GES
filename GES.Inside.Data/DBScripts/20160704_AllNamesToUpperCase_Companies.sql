UPDATE I_Companies SET 
  IssuerName = UPPER(IssuerName), 
  MsciName = UPPER(MsciName), 
  FtseName = UPPER(FtseName), 
  SixName = UPPER(SixName), 
  OtherName = UPPER(OtherName), 
  OldName = UPPER(OldName);