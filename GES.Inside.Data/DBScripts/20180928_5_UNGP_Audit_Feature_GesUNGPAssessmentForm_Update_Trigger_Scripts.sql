
ALTER TRIGGER [dbo].[GesUNGPAssessmentForm_Update]
ON [dbo].[GesUNGPAssessmentForm]
FOR UPDATE
AS
  DECLARE @audit_columns_list [GesUNGPAssessmentForm_Audit_Columns_list];
  DECLARE @newGesUNGPAssessmentFormId AS varchar(200);
  DECLARE @newI_GesCaseReports_Id AS varchar(200);

  DECLARE @newTheExtentOfHarmesScoreId AS varchar(200);
  DECLARE @newTheNumberOfPeopleAffectedScoreId AS varchar(200);
  DECLARE @newOverSeveralYearsScoreId AS varchar(200);
  DECLARE @newSeveralLocationsScoreId AS varchar(200);
  DECLARE @newIsViolationScoreId AS varchar(200);
  DECLARE @newGesConfirmedViolationScoreId AS varchar(200);
  DECLARE @newHumanRightsPolicyPubliclyDisclosedAddScoreId AS varchar(200);
  DECLARE @newHumanRightsPolicyCommunicatedScoreId AS varchar(200);
  DECLARE @newHumanRightsPolicyStipulatesScoreId AS varchar(200);
  DECLARE @newHumanRightsPolicyApprovedScoreId AS varchar(200);
  DECLARE @newGovernanceCommitmentScoreId AS varchar(200);
  DECLARE @newGovernanceExamplesScoreId AS varchar(200);
  DECLARE @newGovernanceClearDivisionScoreId AS varchar(200);
  DECLARE @newBusinessPartnersAddScoreId AS varchar(200);
  DECLARE @newIdentificationAndCommitmentScoreId AS varchar(200);
  DECLARE @newStakeholderEngagementAddScoreId AS varchar(200);
  DECLARE @newHumanRightsTrainingScoreId AS varchar(200);
  DECLARE @newRemedyProcessInPlaceScoreId AS varchar(200);
  DECLARE @newGrievanceMechanismHasOperationalLevelScoreId AS varchar(200);
  DECLARE @newGrievanceMechanismExistenceOfOperationalLevelScoreId AS varchar(200);
  DECLARE @newGrievanceMechanismClearProcessScoreId AS varchar(200);
  DECLARE @newGrievanceMechanismRightsNormsScoreId AS varchar(200);
  DECLARE @newGrievanceMechanismFilingGrievanceScoreId AS varchar(200);
  DECLARE @newGrievanceMechanismReoccurringGrievancesScoreId AS varchar(200);
  DECLARE @newGrievanceMechanismFormatAndProcesseScoreId AS varchar(200);

  DECLARE @newSalientHumanRightsPotentialViolationTotalScore AS nvarchar(max);
  DECLARE @newHumanRightsPolicyTotalScore AS nvarchar(max);
  DECLARE @newTotalScoreForHumanRightsDueDiligence AS nvarchar(max);
  DECLARE @newTotalScoreForRemediationOfAdverseHumanRightsImpacts AS nvarchar(max);
  DECLARE @newTheExtentOfHarmesScoreComment AS nvarchar(max);
  DECLARE @newTheNumberOfPeopleAffectedScoreComment AS nvarchar(max);
  DECLARE @newOverSeveralYearsScoreComment AS nvarchar(max);
  DECLARE @newSeveralLocationsScoreComment AS nvarchar(max);
  DECLARE @newIsViolationScoreComment AS nvarchar(max);
  DECLARE @newGesConfirmedViolationScoreComment AS nvarchar(max);
  DECLARE @newGesCommentSalientHumanRight AS nvarchar(max);
  DECLARE @newHumanRightsPolicyPubliclyDisclosed AS nvarchar(max);
  DECLARE @newHumanRightsPolicyCommunicated AS nvarchar(max);
  DECLARE @newHumanRightsPolicyStipulates AS nvarchar(max);
  DECLARE @newHumanRightsPolicyApproved AS nvarchar(max);
  DECLARE @newGovernanceCommitment AS nvarchar(max);
  DECLARE @newGovernanceExamples AS nvarchar(max);
  DECLARE @newGovernanceClearDivision AS nvarchar(max);
  DECLARE @newBusinessPartners AS nvarchar(max);
  DECLARE @newIdentificationAndCommitment AS nvarchar(max);
  DECLARE @newStakeholderEngagement AS nvarchar(max);
  DECLARE @newHumanRightsTraining AS nvarchar(max);
  DECLARE @newRemedyProcessInPlace AS nvarchar(max);
  DECLARE @newGrievanceMechanismHasOperationalLevel AS nvarchar(max);
  DECLARE @newGrievanceMechanismExistenceOfOperationalLevel AS nvarchar(max);
  DECLARE @newGrievanceMechanismClearProcess AS nvarchar(max);
  DECLARE @newGrievanceMechanismRightsNorms AS nvarchar(max);
  DECLARE @newGrievanceMechanismFilingGrievance AS nvarchar(max);
  DECLARE @newGrievanceMechanismReoccurringGrievances AS nvarchar(max);
  DECLARE @newGrievanceMechanismFormatAndProcesse AS nvarchar(max);
  DECLARE @newGesCommentCompanyPreparedness AS nvarchar(max);
  DECLARE @newTotalScoreForCompanyPreparedness AS nvarchar(max);
  DECLARE @newIsPublished AS nvarchar(10);
  DECLARE @newModifiedBy AS nvarchar(200);

  DECLARE @oldTheExtentOfHarmesScoreId AS varchar(200);
  DECLARE @oldTheNumberOfPeopleAffectedScoreId AS varchar(200);
  DECLARE @oldOverSeveralYearsScoreId AS varchar(200);
  DECLARE @oldSeveralLocationsScoreId AS varchar(200);
  DECLARE @oldIsViolationScoreId AS varchar(200);
  DECLARE @oldGesConfirmedViolationScoreId AS varchar(200);
  DECLARE @oldHumanRightsPolicyPubliclyDisclosedAddScoreId AS varchar(200);
  DECLARE @oldHumanRightsPolicyCommunicatedScoreId AS varchar(200);
  DECLARE @oldHumanRightsPolicyStipulatesScoreId AS varchar(200);
  DECLARE @oldHumanRightsPolicyApprovedScoreId AS varchar(200);
  DECLARE @oldGovernanceCommitmentScoreId AS varchar(200);
  DECLARE @oldGovernanceExamplesScoreId AS varchar(200);
  DECLARE @oldGovernanceClearDivisionScoreId AS varchar(200);
  DECLARE @oldBusinessPartnersAddScoreId AS varchar(200);
  DECLARE @oldIdentificationAndCommitmentScoreId AS varchar(200);
  DECLARE @oldStakeholderEngagementAddScoreId AS varchar(200);
  DECLARE @oldHumanRightsTrainingScoreId AS varchar(200);
  DECLARE @oldRemedyProcessInPlaceScoreId AS varchar(200);
  DECLARE @oldGrievanceMechanismHasOperationalLevelScoreId AS varchar(200);
  DECLARE @oldGrievanceMechanismExistenceOfOperationalLevelScoreId AS varchar(200);
  DECLARE @oldGrievanceMechanismClearProcessScoreId AS varchar(200);
  DECLARE @oldGrievanceMechanismRightsNormsScoreId AS varchar(200);
  DECLARE @oldGrievanceMechanismFilingGrievanceScoreId AS varchar(200);
  DECLARE @oldGrievanceMechanismReoccurringGrievancesScoreId AS varchar(200);
  DECLARE @oldGrievanceMechanismFormatAndProcesseScoreId AS varchar(200);
  DECLARE @oldSalientHumanRightsPotentialViolationTotalScore AS nvarchar(max);
  DECLARE @oldHumanRightsPolicyTotalScore AS nvarchar(max);
  DECLARE @oldTotalScoreForHumanRightsDueDiligence AS nvarchar(max);
  DECLARE @oldTotalScoreForRemediationOfAdverseHumanRightsImpacts AS nvarchar(max);
  DECLARE @oldTheExtentOfHarmesScoreComment AS nvarchar(max);
  DECLARE @oldTheNumberOfPeopleAffectedScoreComment AS nvarchar(max);
  DECLARE @oldOverSeveralYearsScoreComment AS nvarchar(max);
  DECLARE @oldSeveralLocationsScoreComment AS nvarchar(max);
  DECLARE @oldIsViolationScoreComment AS nvarchar(max);
  DECLARE @oldGesConfirmedViolationScoreComment AS nvarchar(max);
  DECLARE @oldGesCommentSalientHumanRight AS nvarchar(max);
  DECLARE @oldHumanRightsPolicyPubliclyDisclosed AS nvarchar(max);
  DECLARE @oldHumanRightsPolicyCommunicated AS nvarchar(max);
  DECLARE @oldHumanRightsPolicyStipulates AS nvarchar(max);
  DECLARE @oldHumanRightsPolicyApproved AS nvarchar(max);
  DECLARE @oldGovernanceCommitment AS nvarchar(max);
  DECLARE @oldGovernanceExamples AS nvarchar(max);
  DECLARE @oldGovernanceClearDivision AS nvarchar(max);
  DECLARE @oldBusinessPartners AS nvarchar(max);
  DECLARE @oldIdentificationAndCommitment AS nvarchar(max);
  DECLARE @oldStakeholderEngagement AS nvarchar(max);
  DECLARE @oldHumanRightsTraining AS nvarchar(max);
  DECLARE @oldRemedyProcessInPlace AS nvarchar(max);
  DECLARE @oldGrievanceMechanismHasOperationalLevel AS nvarchar(max);
  DECLARE @oldGrievanceMechanismExistenceOfOperationalLevel AS nvarchar(max);
  DECLARE @oldGrievanceMechanismClearProcess AS nvarchar(max);
  DECLARE @oldGrievanceMechanismRightsNorms AS nvarchar(max);
  DECLARE @oldGrievanceMechanismFilingGrievance AS nvarchar(max);
  DECLARE @oldGrievanceMechanismReoccurringGrievances AS nvarchar(max);
  DECLARE @oldGrievanceMechanismFormatAndProcesse AS nvarchar(max);
  DECLARE @oldGesCommentCompanyPreparedness AS nvarchar(max);
  DECLARE @oldTotalScoreForCompanyPreparedness AS nvarchar(max);
  DECLARE @oldIsPublished AS nvarchar(10);
  DECLARE @oldModifiedBy AS nvarchar(200);

  DECLARE @mode AS nvarchar(20);

  DECLARE @hasChangedValue AS bit = 'false';

  SELECT
    @newGesUNGPAssessmentFormId = CAST([GesUNGPAssessmentFormId] AS uniqueidentifier),
    @newI_GesCaseReports_Id = [I_GesCaseReports_Id],
    @newTheExtentOfHarmesScoreId = [TheExtentOfHarmesScoreId],
    @newTheExtentOfHarmesScoreComment = [TheExtentOfHarmesScoreComment],
    @newTheNumberOfPeopleAffectedScoreId = [TheNumberOfPeopleAffectedScoreId],
    @newTheNumberOfPeopleAffectedScoreComment = [TheNumberOfPeopleAffectedScoreComment],
    @newOverSeveralYearsScoreId = [OverSeveralYearsScoreId],
    @newOverSeveralYearsScoreComment = [OverSeveralYearsScoreComment],
    @newSeveralLocationsScoreId = [SeveralLocationsScoreId],
    @newSeveralLocationsScoreComment = [SeveralLocationsScoreComment],
    @newIsViolationScoreId = [IsViolationScoreId],
    @newIsViolationScoreComment = [IsViolationScoreComment],
    @newGesConfirmedViolationScoreId = [GesConfirmedViolationScoreId],
    @newGesConfirmedViolationScoreComment = [GesConfirmedViolationScoreComment],
    @newSalientHumanRightsPotentialViolationTotalScore = [SalientHumanRightsPotentialViolationTotalScore],
    @newGesCommentSalientHumanRight = [GesCommentSalientHumanRight],
    @newHumanRightsPolicyPubliclyDisclosedAddScoreId = [HumanRightsPolicyPubliclyDisclosedAddScoreId],
    @newHumanRightsPolicyPubliclyDisclosed = [HumanRightsPolicyPubliclyDisclosed],
    @newHumanRightsPolicyCommunicatedScoreId = [HumanRightsPolicyCommunicatedScoreId],
    @newHumanRightsPolicyCommunicated = [HumanRightsPolicyCommunicated],
    @newHumanRightsPolicyStipulatesScoreId = [HumanRightsPolicyStipulatesScoreId],
    @newHumanRightsPolicyStipulates = [HumanRightsPolicyStipulates],
    @newHumanRightsPolicyApprovedScoreId = [HumanRightsPolicyApprovedScoreId],
    @newHumanRightsPolicyApproved = [HumanRightsPolicyApproved],
    @newHumanRightsPolicyTotalScore = [HumanRightsPolicyTotalScore],
    @newGovernanceCommitmentScoreId = [GovernanceCommitmentScoreId],
    @newGovernanceCommitment = [GovernanceCommitment],
    @newGovernanceExamplesScoreId = [GovernanceExamplesScoreId],
    @newGovernanceExamples = [GovernanceExamples],
    @newGovernanceClearDivisionScoreId = [GovernanceClearDivisionScoreId],
    @newGovernanceClearDivision = [GovernanceClearDivision],
    @newBusinessPartners = [BusinessPartners],
    @newBusinessPartnersAddScoreId = [BusinessPartnersAddScoreId],
    @newIdentificationAndCommitmentScoreId = [IdentificationAndCommitmentScoreId],
    @newIdentificationAndCommitment = [IdentificationAndCommitment],
    @newStakeholderEngagement = [StakeholderEngagement],
    @newStakeholderEngagementAddScoreId = [StakeholderEngagementAddScoreId],
    @newHumanRightsTraining = [HumanRightsTraining],
    @newHumanRightsTrainingScoreId = [HumanRightsTrainingScoreId],
    @newTotalScoreForHumanRightsDueDiligence = [TotalScoreForHumanRightsDueDiligence],
    @newRemedyProcessInPlaceScoreId = [RemedyProcessInPlaceScoreId],
    @newRemedyProcessInPlace = [RemedyProcessInPlace],
    @newGrievanceMechanismHasOperationalLevelScoreId = [GrievanceMechanismHasOperationalLevelScoreId],
    @newGrievanceMechanismHasOperationalLevel = [GrievanceMechanismHasOperationalLevel],
    @newGrievanceMechanismExistenceOfOperationalLevelScoreId = [GrievanceMechanismExistenceOfOperationalLevelScoreId],
    @newGrievanceMechanismExistenceOfOperationalLevel = [GrievanceMechanismExistenceOfOperationalLevel],
    @newGrievanceMechanismClearProcessScoreId = [GrievanceMechanismClearProcessScoreId],
    @newGrievanceMechanismClearProcess = [GrievanceMechanismClearProcess],
    @newGrievanceMechanismRightsNormsScoreId = [GrievanceMechanismRightsNormsScoreId],
    @newGrievanceMechanismRightsNorms = [GrievanceMechanismRightsNorms],
    @newGrievanceMechanismFilingGrievanceScoreId = [GrievanceMechanismFilingGrievanceScoreId],
    @newGrievanceMechanismFilingGrievance = [GrievanceMechanismFilingGrievance],
    @newGrievanceMechanismReoccurringGrievancesScoreId = [GrievanceMechanismReoccurringGrievancesScoreId],
    @newGrievanceMechanismReoccurringGrievances = [GrievanceMechanismReoccurringGrievances],
    @newGrievanceMechanismFormatAndProcesseScoreId = [GrievanceMechanismFormatAndProcesseScoreId],
    @newGrievanceMechanismFormatAndProcesse = [GrievanceMechanismFormatAndProcesse],
    @newTotalScoreForRemediationOfAdverseHumanRightsImpacts = [TotalScoreForRemediationOfAdverseHumanRightsImpacts],
    @newGesCommentCompanyPreparedness = [GesCommentCompanyPreparedness],
    @newTotalScoreForCompanyPreparedness = [TotalScoreForCompanyPreparedness],
    @newIsPublished = [IsPublished],
    @newModifiedBy = [ModifiedBy]
  FROM INSERTED;

  SELECT
    @oldTheExtentOfHarmesScoreId = [TheExtentOfHarmesScoreId],
    @oldTheExtentOfHarmesScoreComment = [TheExtentOfHarmesScoreComment],
    @oldTheNumberOfPeopleAffectedScoreId = [TheNumberOfPeopleAffectedScoreId],
    @oldTheNumberOfPeopleAffectedScoreComment = [TheNumberOfPeopleAffectedScoreComment],
    @oldOverSeveralYearsScoreId = [OverSeveralYearsScoreId],
    @oldOverSeveralYearsScoreComment = [OverSeveralYearsScoreComment],
    @oldSeveralLocationsScoreId = [SeveralLocationsScoreId],
    @oldSeveralLocationsScoreComment = [SeveralLocationsScoreComment],
    @oldIsViolationScoreId = [IsViolationScoreId],
    @oldIsViolationScoreComment = [IsViolationScoreComment],
    @oldGesConfirmedViolationScoreId = [GesConfirmedViolationScoreId],
    @oldGesConfirmedViolationScoreComment = [GesConfirmedViolationScoreComment],
    @oldSalientHumanRightsPotentialViolationTotalScore = [SalientHumanRightsPotentialViolationTotalScore],
    @oldGesCommentSalientHumanRight = [GesCommentSalientHumanRight],
    @oldHumanRightsPolicyPubliclyDisclosedAddScoreId = [HumanRightsPolicyPubliclyDisclosedAddScoreId],
    @oldHumanRightsPolicyPubliclyDisclosed = [HumanRightsPolicyPubliclyDisclosed],
    @oldHumanRightsPolicyCommunicatedScoreId = [HumanRightsPolicyCommunicatedScoreId],
    @oldHumanRightsPolicyCommunicated = [HumanRightsPolicyCommunicated],
    @oldHumanRightsPolicyStipulatesScoreId = [HumanRightsPolicyStipulatesScoreId],
    @oldHumanRightsPolicyStipulates = [HumanRightsPolicyStipulates],
    @oldHumanRightsPolicyApprovedScoreId = [HumanRightsPolicyApprovedScoreId],
    @oldHumanRightsPolicyApproved = [HumanRightsPolicyApproved],
    @oldHumanRightsPolicyTotalScore = [HumanRightsPolicyTotalScore],
    @oldGovernanceCommitmentScoreId = [GovernanceCommitmentScoreId],
    @oldGovernanceCommitment = [GovernanceCommitment],
    @oldGovernanceExamplesScoreId = [GovernanceExamplesScoreId],
    @oldGovernanceExamples = [GovernanceExamples],
    @oldGovernanceClearDivisionScoreId = [GovernanceClearDivisionScoreId],
    @oldGovernanceClearDivision = [GovernanceClearDivision],
    @oldBusinessPartners = [BusinessPartners],
    @oldBusinessPartnersAddScoreId = [BusinessPartnersAddScoreId],
    @oldIdentificationAndCommitmentScoreId = [IdentificationAndCommitmentScoreId],
    @oldIdentificationAndCommitment = [IdentificationAndCommitment],
    @oldStakeholderEngagement = [StakeholderEngagement],
    @oldStakeholderEngagementAddScoreId = [StakeholderEngagementAddScoreId],
    @oldHumanRightsTraining = [HumanRightsTraining],
    @oldHumanRightsTrainingScoreId = [HumanRightsTrainingScoreId],
    @oldTotalScoreForHumanRightsDueDiligence = [TotalScoreForHumanRightsDueDiligence],
    @oldRemedyProcessInPlaceScoreId = [RemedyProcessInPlaceScoreId],
    @oldRemedyProcessInPlace = [RemedyProcessInPlace],
    @oldGrievanceMechanismHasOperationalLevelScoreId = [GrievanceMechanismHasOperationalLevelScoreId],
    @oldGrievanceMechanismHasOperationalLevel = [GrievanceMechanismHasOperationalLevel],
    @oldGrievanceMechanismExistenceOfOperationalLevelScoreId = [GrievanceMechanismExistenceOfOperationalLevelScoreId],
    @oldGrievanceMechanismExistenceOfOperationalLevel = [GrievanceMechanismExistenceOfOperationalLevel],
    @oldGrievanceMechanismClearProcessScoreId = [GrievanceMechanismClearProcessScoreId],
    @oldGrievanceMechanismClearProcess = [GrievanceMechanismClearProcess],
    @oldGrievanceMechanismRightsNormsScoreId = [GrievanceMechanismRightsNormsScoreId],
    @oldGrievanceMechanismRightsNorms = [GrievanceMechanismRightsNorms],
    @oldGrievanceMechanismFilingGrievanceScoreId = [GrievanceMechanismFilingGrievanceScoreId],
    @oldGrievanceMechanismFilingGrievance = [GrievanceMechanismFilingGrievance],
    @oldGrievanceMechanismReoccurringGrievancesScoreId = [GrievanceMechanismReoccurringGrievancesScoreId],
    @oldGrievanceMechanismReoccurringGrievances = [GrievanceMechanismReoccurringGrievances],
    @oldGrievanceMechanismFormatAndProcesseScoreId = [GrievanceMechanismFormatAndProcesseScoreId],
    @oldGrievanceMechanismFormatAndProcesse = [GrievanceMechanismFormatAndProcesse],
    @oldTotalScoreForRemediationOfAdverseHumanRightsImpacts = [TotalScoreForRemediationOfAdverseHumanRightsImpacts],
    @oldGesCommentCompanyPreparedness = [GesCommentCompanyPreparedness],
    @oldTotalScoreForCompanyPreparedness = [TotalScoreForCompanyPreparedness],
    @oldIsPublished = [IsPublished],
    @oldModifiedBy = [ModifiedBy]
  FROM DELETED;

  IF (dbo.isUNGPSCoreChanged(@newTheExtentOfHarmesScoreId, @oldTheExtentOfHarmesScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldTheExtentOfHarmesScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState) 
	VALUES ('TheExtentOfHarmesScoreId','The extent of harm''s score value', dbo.GetUNGPSCores(@oldTheExtentOfHarmesScoreId), dbo.GetUNGPSCores(@newTheExtentOfHarmesScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newTheNumberOfPeopleAffectedScoreId, @oldTheNumberOfPeopleAffectedScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldTheNumberOfPeopleAffectedScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreId','The number of people affected''s score value', dbo.GetUNGPSCores(@oldTheNumberOfPeopleAffectedScoreId), dbo.GetUNGPSCores(@newTheNumberOfPeopleAffectedScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newOverSeveralYearsScoreId, @oldOverSeveralYearsScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldOverSeveralYearsScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreId','Over several years''s score value', dbo.GetUNGPSCores(@oldOverSeveralYearsScoreId), dbo.GetUNGPSCores(@newOverSeveralYearsScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newSeveralLocationsScoreId, @oldSeveralLocationsScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldSeveralLocationsScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreId','Several locations''s score value', dbo.GetUNGPSCores(@oldSeveralLocationsScoreId), dbo.GetUNGPSCores(@newSeveralLocationsScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newIsViolationScoreId, @oldIsViolationScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldIsViolationScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreId','The (alleged) violation still occurring''s score value', dbo.GetUNGPSCores(@oldIsViolationScoreId), dbo.GetUNGPSCores(@newIsViolationScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGesConfirmedViolationScoreId, @oldGesConfirmedViolationScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGesConfirmedViolationScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreId','The case a GES’ confirmed violation of international norms''s score value', dbo.GetUNGPSCores(@oldGesConfirmedViolationScoreId), dbo.GetUNGPSCores(@newGesConfirmedViolationScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newHumanRightsPolicyPubliclyDisclosedAddScoreId, @oldHumanRightsPolicyPubliclyDisclosedAddScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyPubliclyDisclosedAddScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosedAddScoreId','Human rights policy - A publicly disclosed human rights policy''s score value', dbo.GetUNGPSCores(@oldHumanRightsPolicyPubliclyDisclosedAddScoreId), dbo.GetUNGPSCores(@newHumanRightsPolicyPubliclyDisclosedAddScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newHumanRightsPolicyCommunicatedScoreId, @oldHumanRightsPolicyCommunicatedScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyCommunicatedScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicatedScoreId','Human rights policy - The company states''s score value', dbo.GetUNGPSCores(@oldHumanRightsPolicyCommunicatedScoreId), dbo.GetUNGPSCores(@newHumanRightsPolicyCommunicatedScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newHumanRightsPolicyStipulatesScoreId, @oldHumanRightsPolicyStipulatesScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyStipulatesScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulatesScoreId','Human rights policy - The policy stipulates''s score value', dbo.GetUNGPSCores(@oldHumanRightsPolicyStipulatesScoreId), dbo.GetUNGPSCores(@newHumanRightsPolicyStipulatesScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newHumanRightsPolicyApprovedScoreId, @oldHumanRightsPolicyApprovedScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyApprovedScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApprovedScoreId','Human rights policy - The policy is approved ''s score value', dbo.GetUNGPSCores(@oldHumanRightsPolicyApprovedScoreId), dbo.GetUNGPSCores(@newHumanRightsPolicyApprovedScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGovernanceCommitmentScoreId, @oldGovernanceCommitmentScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGovernanceCommitmentScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitmentScoreId','Human rights due diligence - Governance - A written commitment''s score value', dbo.GetUNGPSCores(@oldGovernanceCommitmentScoreId), dbo.GetUNGPSCores(@newGovernanceCommitmentScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGovernanceExamplesScoreId, @oldGovernanceExamplesScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGovernanceExamplesScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamplesScoreId','Human rights due diligence - Governance - The company provides examples''s score value', dbo.GetUNGPSCores(@oldGovernanceExamplesScoreId), dbo.GetUNGPSCores(@newGovernanceExamplesScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGovernanceClearDivisionScoreId, @oldGovernanceClearDivisionScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGovernanceClearDivisionScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivisionScoreId','Human rights due diligence - Governance - The company provides a clear division of responsibility''s score value', dbo.GetUNGPSCores(@oldGovernanceClearDivisionScoreId), dbo.GetUNGPSCores(@newGovernanceClearDivisionScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newBusinessPartnersAddScoreId, @oldBusinessPartnersAddScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldBusinessPartnersAddScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartnersAddScoreId','Human rights due diligence - Business partners -  The company takes human rights considerations into account''s score value', dbo.GetUNGPSCores(@oldBusinessPartnersAddScoreId), dbo.GetUNGPSCores(@newBusinessPartnersAddScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newIdentificationAndCommitmentScoreId, @oldIdentificationAndCommitmentScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldIdentificationAndCommitmentScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitmentScoreId','Human rights due diligence - Identification and commitment''s score value', dbo.GetUNGPSCores(@oldIdentificationAndCommitmentScoreId), dbo.GetUNGPSCores(@newIdentificationAndCommitmentScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newStakeholderEngagementAddScoreId, @oldStakeholderEngagementAddScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldStakeholderEngagementAddScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagementAddScoreId','Human rights due diligence - Stakeholder engagement''s score value', dbo.GetUNGPSCores(@oldStakeholderEngagementAddScoreId), dbo.GetUNGPSCores(@newStakeholderEngagementAddScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newHumanRightsTrainingScoreId, @oldHumanRightsTrainingScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsTrainingScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTrainingScoreId','Human rights due diligence - Human rights training''s score value', dbo.GetUNGPSCores(@oldHumanRightsTrainingScoreId), dbo.GetUNGPSCores(@newHumanRightsTrainingScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newRemedyProcessInPlaceScoreId, @oldRemedyProcessInPlaceScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldRemedyProcessInPlaceScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlaceScoreId','Remediation of adverse human rights impacts - Remedy process in place''s score value', dbo.GetUNGPSCores(@oldRemedyProcessInPlaceScoreId), dbo.GetUNGPSCores(@newRemedyProcessInPlaceScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGrievanceMechanismHasOperationalLevelScoreId, @oldGrievanceMechanismHasOperationalLevelScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismHasOperationalLevelScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevelScoreId','Remediation of adverse human rights impacts - Grievance mechanism''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismHasOperationalLevelScoreId), dbo.GetUNGPSCores(@newGrievanceMechanismHasOperationalLevelScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGrievanceMechanismExistenceOfOperationalLevelScoreId, @oldGrievanceMechanismExistenceOfOperationalLevelScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismExistenceOfOperationalLevelScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevelScoreId','Remediation of adverse human rights impacts - The existence of operational-level grievance mechanism''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismExistenceOfOperationalLevelScoreId), dbo.GetUNGPSCores(@newGrievanceMechanismExistenceOfOperationalLevelScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGrievanceMechanismClearProcessScoreId, @oldGrievanceMechanismClearProcessScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismClearProcessScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcessScoreId','Remediation of adverse human rights impacts - The company discloses a clear process''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismClearProcessScoreId), dbo.GetUNGPSCores(@newGrievanceMechanismClearProcessScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGrievanceMechanismRightsNormsScoreId, @oldGrievanceMechanismRightsNormsScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismRightsNormsScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNormsScoreId','Remediation of adverse human rights impacts - The company addresses a grievance''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismRightsNormsScoreId), dbo.GetUNGPSCores(@newGrievanceMechanismRightsNormsScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGrievanceMechanismFilingGrievanceScoreId, @oldGrievanceMechanismFilingGrievanceScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismFilingGrievanceScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievanceScoreId','Remediation of adverse human rights impacts - The people filing grievance''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismFilingGrievanceScoreId), dbo.GetUNGPSCores(@newGrievanceMechanismFilingGrievanceScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGrievanceMechanismReoccurringGrievancesScoreId, @oldGrievanceMechanismReoccurringGrievancesScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismReoccurringGrievancesScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievancesScoreId','Remediation of adverse human rights impacts - Reoccurring grievances on similar matters''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismReoccurringGrievancesScoreId), dbo.GetUNGPSCores(@newGrievanceMechanismReoccurringGrievancesScoreId), @mode)
  END

  IF (dbo.isUNGPSCoreChanged(@newGrievanceMechanismFormatAndProcesseScoreId, @oldGrievanceMechanismFormatAndProcesseScoreId) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismFormatAndProcesseScoreId IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesseScoreId','Remediation of adverse human rights impacts - The format and processes related to the grievance mechanism''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismFormatAndProcesseScoreId), dbo.GetUNGPSCores(@newGrievanceMechanismFormatAndProcesseScoreId), @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newSalientHumanRightsPotentialViolationTotalScore, @oldSalientHumanRightsPotentialViolationTotalScore) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldSalientHumanRightsPotentialViolationTotalScore IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('SalientHumanRightsPotentialViolationTotalScore','Salient human rights potential violation total''s score value', @oldSalientHumanRightsPotentialViolationTotalScore, @newSalientHumanRightsPotentialViolationTotalScore, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newHumanRightsPolicyTotalScore, @oldHumanRightsPolicyTotalScore) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyTotalScore IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyTotalScore','Human rights policy total''s score value', @oldHumanRightsPolicyTotalScore, @newHumanRightsPolicyTotalScore, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newTotalScoreForHumanRightsDueDiligence, @oldTotalScoreForHumanRightsDueDiligence) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldTotalScoreForHumanRightsDueDiligence IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForHumanRightsDueDiligence','Human rights due diligence total''s score value', @oldTotalScoreForHumanRightsDueDiligence, @newTotalScoreForHumanRightsDueDiligence, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newTotalScoreForRemediationOfAdverseHumanRightsImpacts, @oldTotalScoreForRemediationOfAdverseHumanRightsImpacts) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldTotalScoreForRemediationOfAdverseHumanRightsImpacts IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForRemediationOfAdverseHumanRightsImpacts','Remediation of adverse human rights impacts total''s score value', @oldTotalScoreForRemediationOfAdverseHumanRightsImpacts, @newTotalScoreForRemediationOfAdverseHumanRightsImpacts, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newTheExtentOfHarmesScoreComment, @oldTheExtentOfHarmesScoreComment) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldTheExtentOfHarmesScoreComment IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreComment','The extent of harm''s comments', @oldTheExtentOfHarmesScoreComment, @newTheExtentOfHarmesScoreComment, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newTheNumberOfPeopleAffectedScoreComment, @oldTheNumberOfPeopleAffectedScoreComment) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldTheNumberOfPeopleAffectedScoreComment IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreComment','The number of people affected''s comments', @oldTheNumberOfPeopleAffectedScoreComment, @newTheNumberOfPeopleAffectedScoreComment, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newOverSeveralYearsScoreComment, @oldOverSeveralYearsScoreComment) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldOverSeveralYearsScoreComment IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreComment','Over several years''s comments', @oldOverSeveralYearsScoreComment, @newOverSeveralYearsScoreComment, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newSeveralLocationsScoreComment, @oldSeveralLocationsScoreComment) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldSeveralLocationsScoreComment IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreComment','Several locations''s comments', @oldSeveralLocationsScoreComment, @newSeveralLocationsScoreComment, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newIsViolationScoreComment, @oldIsViolationScoreComment) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldIsViolationScoreComment IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreComment','The (alleged) violation still occurring''s comments', @oldIsViolationScoreComment, @newIsViolationScoreComment, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGesConfirmedViolationScoreComment, @oldGesConfirmedViolationScoreComment) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGesConfirmedViolationScoreComment IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreComment','The case a GES’ confirmed violation of international norms''s comments', @oldGesConfirmedViolationScoreComment, @newGesConfirmedViolationScoreComment, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGesCommentSalientHumanRight, @oldGesCommentSalientHumanRight) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGesCommentSalientHumanRight IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentSalientHumanRight', 'Level of Human rights salience - GES general comment''s comments', @oldGesCommentSalientHumanRight, @newGesCommentSalientHumanRight, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newHumanRightsPolicyPubliclyDisclosed, @oldHumanRightsPolicyPubliclyDisclosed) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyPubliclyDisclosed IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosed','Human rights policy - A publicly disclosed human rights policy''s comments', @oldHumanRightsPolicyPubliclyDisclosed, @newHumanRightsPolicyPubliclyDisclosed, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newHumanRightsPolicyCommunicated, @oldHumanRightsPolicyCommunicated) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyCommunicated IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicated','Human rights policy - The company states''s comments', @oldHumanRightsPolicyCommunicated, @newHumanRightsPolicyCommunicated, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newHumanRightsPolicyStipulates, @oldHumanRightsPolicyStipulates) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyStipulates IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulates','Human rights policy - The policy stipulates''s comments', @oldHumanRightsPolicyStipulates, @newHumanRightsPolicyStipulates, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newHumanRightsPolicyApproved, @oldHumanRightsPolicyApproved) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsPolicyApproved IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApproved','Human rights policy - The policy is approved''s comments', @oldHumanRightsPolicyApproved, @newHumanRightsPolicyApproved, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGovernanceCommitment, @oldGovernanceCommitment) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGovernanceCommitment IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitment','Human rights due diligence - Governance - A written commitment''s comments', @oldGovernanceCommitment, @newGovernanceCommitment, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGovernanceExamples, @oldGovernanceExamples) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGovernanceExamples IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamples','Human rights due diligence - Governance - The company provides examples''s comments', @oldGovernanceExamples, @newGovernanceExamples, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGovernanceClearDivision, @oldGovernanceClearDivision) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGovernanceClearDivision IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivision','Human rights due diligence - Governance - The company provides a clear division of responsibility''s comments', @oldGovernanceClearDivision, @newGovernanceClearDivision, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newBusinessPartners, @oldBusinessPartners) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldBusinessPartners IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartners','Human rights due diligence - Business partners -  The company takes human rights considerations into account''s comments', @oldBusinessPartners, @newBusinessPartners, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newIdentificationAndCommitment, @oldIdentificationAndCommitment) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldIdentificationAndCommitment IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitment','Human rights due diligence - Identification and commitment''s comments', @oldIdentificationAndCommitment, @newIdentificationAndCommitment, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newStakeholderEngagement, @oldStakeholderEngagement) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldStakeholderEngagement IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagement','Human rights due diligence - Stakeholder engagement''s comments', @oldStakeholderEngagement, @newStakeholderEngagement, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newHumanRightsTraining, @oldHumanRightsTraining) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldHumanRightsTraining IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTraining','Human rights due diligence - Human rights training''s comments', @oldHumanRightsTraining, @newHumanRightsTraining, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newRemedyProcessInPlace, @oldRemedyProcessInPlace) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldRemedyProcessInPlace IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlace','Remediation of adverse human rights impacts - Remedy process in place''s comments', @oldRemedyProcessInPlace, @newRemedyProcessInPlace, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGrievanceMechanismHasOperationalLevel, @oldGrievanceMechanismHasOperationalLevel) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismHasOperationalLevel IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevel','Remediation of adverse human rights impacts - Grievance mechanism''s comments', @oldGrievanceMechanismHasOperationalLevel, @newGrievanceMechanismHasOperationalLevel, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGrievanceMechanismExistenceOfOperationalLevel, @oldGrievanceMechanismExistenceOfOperationalLevel) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismExistenceOfOperationalLevel IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevel','Remediation of adverse human rights impacts - The existence of operational-level grievance mechanism''s comments', @oldGrievanceMechanismExistenceOfOperationalLevel, @newGrievanceMechanismExistenceOfOperationalLevel, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGrievanceMechanismClearProcess, @oldGrievanceMechanismClearProcess) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismClearProcess IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcess','Remediation of adverse human rights impacts - The company discloses a clear process''s comments', @oldGrievanceMechanismClearProcess, @newGrievanceMechanismClearProcess, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGrievanceMechanismRightsNorms, @oldGrievanceMechanismRightsNorms) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismRightsNorms IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNorms','Remediation of adverse human rights impacts - The company addresses a grievance''s comments', @oldGrievanceMechanismRightsNorms, @newGrievanceMechanismRightsNorms, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGrievanceMechanismFilingGrievance, @oldGrievanceMechanismFilingGrievance) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismFilingGrievance IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievance','Remediation of adverse human rights impacts - The people filing grievance''s comments', @oldGrievanceMechanismFilingGrievance, @newGrievanceMechanismFilingGrievance, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGrievanceMechanismReoccurringGrievances, @oldGrievanceMechanismReoccurringGrievances) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismReoccurringGrievances IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievances','Remediation of adverse human rights impacts - Reoccurring grievances on similar matters''s comments', @oldGrievanceMechanismReoccurringGrievances, @newGrievanceMechanismReoccurringGrievances, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGrievanceMechanismFormatAndProcesse, @oldGrievanceMechanismFormatAndProcesse) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGrievanceMechanismFormatAndProcesse IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesse','Remediation of adverse human rights impacts - The format and processes related to the grievance mechanism''s comments', @oldGrievanceMechanismFormatAndProcesse, @newGrievanceMechanismFormatAndProcesse, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newGesCommentCompanyPreparedness, @oldGesCommentCompanyPreparedness) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldGesCommentCompanyPreparedness IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentCompanyPreparedness','Salient human rights potential violation total''s comments', @oldGesCommentCompanyPreparedness, @newGesCommentCompanyPreparedness, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newTotalScoreForCompanyPreparedness, @oldTotalScoreForCompanyPreparedness) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldTotalScoreForCompanyPreparedness IS NULL)
      SET @mode = 'Insert';
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForCompanyPreparedness','Company Preparedness total''s score value', @oldTotalScoreForCompanyPreparedness, @newTotalScoreForCompanyPreparedness, @mode)
  END

  IF (dbo.isUNGPStringValueChanged(@newIsPublished, @oldIsPublished) = 1)
  BEGIN
    SET @hasChangedValue = 'true'
    SET @mode = 'Update';
    IF (@oldIsPublished IS NULL)
      SET @mode = 'Insert';
	  declare @oldValueString as nvarchar(200), @newValueString  as nvarchar(200);

	  set @newValueString = 'No';
	  if (@newIsPublished = 1)
		set @newValueString = 'Yes';

	  set @oldValueString = 'No';
	  if (@oldIsPublished = 1)
		set @oldValueString = 'Yes';

		INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState) VALUES ('IsPublished','Show in client', @oldValueString, @newValueString, @mode)
  END




  IF (@hasChangedValue = 'true')
  BEGIN
    EXEC UpdateGesUNGPAssessmentForm_Audit @newGesUNGPAssessmentFormId,
                                           @newI_GesCaseReports_Id,
                                           @audit_columns_list,
                                           'Update',
                                           @newModifiedBy
  END