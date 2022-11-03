CREATE TRIGGER [dbo].[GesUNGPAssessmentForm_Insert]
ON [dbo].[GesUNGPAssessmentForm]
FOR INSERT
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


  IF (@newTheExtentOfHarmesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreId', NULL, dbo.GetUNGPSCores(@newTheExtentOfHarmesScoreId), 'Insert')
  END
  IF (@newTheNumberOfPeopleAffectedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreId', NULL, dbo.GetUNGPSCores(@newTheNumberOfPeopleAffectedScoreId), 'Insert')
  END
  IF (@newOverSeveralYearsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreId', NULL, dbo.GetUNGPSCores(@newOverSeveralYearsScoreId), 'Insert')
  END
  IF (@newSeveralLocationsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreId', NULL, dbo.GetUNGPSCores(@newSeveralLocationsScoreId), 'Insert')
  END
  IF (@newIsViolationScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreId', NULL, dbo.GetUNGPSCores(@newIsViolationScoreId), 'Insert')
  END
  IF (@newGesConfirmedViolationScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreId', NULL, dbo.GetUNGPSCores(@newGesConfirmedViolationScoreId), 'Insert')
  END
  IF (@newHumanRightsPolicyPubliclyDisclosedAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosedAddScoreId', NULL, dbo.GetUNGPSCores(@newHumanRightsPolicyPubliclyDisclosedAddScoreId), 'Insert')
  END
  IF (@newHumanRightsPolicyCommunicatedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicatedScoreId', NULL, dbo.GetUNGPSCores(@newHumanRightsPolicyCommunicatedScoreId), 'Insert')
  END
  IF (@newHumanRightsPolicyStipulatesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulatesScoreId', NULL, dbo.GetUNGPSCores(@newHumanRightsPolicyStipulatesScoreId), 'Insert')
  END
  IF (@newHumanRightsPolicyApprovedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApprovedScoreId', NULL, dbo.GetUNGPSCores(@newHumanRightsPolicyApprovedScoreId), 'Insert')
  END
  IF (@newGovernanceCommitmentScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitmentScoreId', NULL, dbo.GetUNGPSCores(@newGovernanceCommitmentScoreId), 'Insert')
  END
  IF (@newGovernanceExamplesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamplesScoreId', NULL, dbo.GetUNGPSCores(@newGovernanceExamplesScoreId), 'Insert')
  END
  IF (@newGovernanceClearDivisionScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivisionScoreId', NULL, dbo.GetUNGPSCores(@newGovernanceClearDivisionScoreId), 'Insert')
  END
  IF (@newBusinessPartnersAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartnersAddScoreId', NULL, dbo.GetUNGPSCores(@newBusinessPartnersAddScoreId), 'Insert')
  END
  IF (@newIdentificationAndCommitmentScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitmentScoreId', NULL, dbo.GetUNGPSCores(@newIdentificationAndCommitmentScoreId), 'Insert')
  END
  IF (@newStakeholderEngagementAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagementAddScoreId', NULL, dbo.GetUNGPSCores(@newStakeholderEngagementAddScoreId), 'Insert')
  END
  IF (@newHumanRightsTrainingScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTrainingScoreId', NULL, dbo.GetUNGPSCores(@newHumanRightsTrainingScoreId), 'Insert')
  END
  IF (@newRemedyProcessInPlaceScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlaceScoreId', NULL, dbo.GetUNGPSCores(@newRemedyProcessInPlaceScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismHasOperationalLevelScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevelScoreId', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismHasOperationalLevelScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismExistenceOfOperationalLevelScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevelScoreId', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismExistenceOfOperationalLevelScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismClearProcessScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcessScoreId', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismClearProcessScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismRightsNormsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNormsScoreId', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismRightsNormsScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismFilingGrievanceScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievanceScoreId', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismFilingGrievanceScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismReoccurringGrievancesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievancesScoreId', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismReoccurringGrievancesScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismFormatAndProcesseScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesseScoreId', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismFormatAndProcesseScoreId), 'Insert')
  END
  IF (@newSalientHumanRightsPotentialViolationTotalScore IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('SalientHumanRightsPotentialViolationTotalScore', NULL, @newSalientHumanRightsPotentialViolationTotalScore, 'Insert')
  END
  IF (@newHumanRightsPolicyTotalScore IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyTotalScore', NULL, @newHumanRightsPolicyTotalScore, 'Insert')
  END
  IF (@newTotalScoreForHumanRightsDueDiligence IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForHumanRightsDueDiligence', NULL, @newTotalScoreForHumanRightsDueDiligence, 'Insert')
  END
  IF (@newTotalScoreForRemediationOfAdverseHumanRightsImpacts IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForRemediationOfAdverseHumanRightsImpacts', NULL, @newTotalScoreForRemediationOfAdverseHumanRightsImpacts, 'Insert')
  END
  IF (@newTheExtentOfHarmesScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreComment', NULL, @newTheExtentOfHarmesScoreComment, 'Insert')
  END
  IF (@newTheNumberOfPeopleAffectedScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreComment', NULL, @newTheNumberOfPeopleAffectedScoreComment, 'Insert')
  END
  IF (@newOverSeveralYearsScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreComment', NULL, @newOverSeveralYearsScoreComment, 'Insert')
  END
  IF (@newSeveralLocationsScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreComment', NULL, @newSeveralLocationsScoreComment, 'Insert')
  END
  IF (@newIsViolationScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreComment', NULL, @newIsViolationScoreComment, 'Insert')
  END
  IF (@newGesConfirmedViolationScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreComment', NULL, @newGesConfirmedViolationScoreComment, 'Insert')
  END
  IF (@newGesCommentSalientHumanRight IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentSalientHumanRight', NULL, @newGesCommentSalientHumanRight, 'Insert')
  END
  IF (@newHumanRightsPolicyPubliclyDisclosed IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosed', NULL, @newHumanRightsPolicyPubliclyDisclosed, 'Insert')
  END
  IF (@newHumanRightsPolicyCommunicated IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicated', NULL, @newHumanRightsPolicyCommunicated, 'Insert')
  END
  IF (@newHumanRightsPolicyStipulates IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulates', NULL, @newHumanRightsPolicyStipulates, 'Insert')
  END
  IF (@newHumanRightsPolicyApproved IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApproved', NULL, @newHumanRightsPolicyApproved, 'Insert')
  END
  IF (@newGovernanceCommitment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitment', NULL, @newGovernanceCommitment, 'Insert')
  END
  IF (@newGovernanceExamples IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamples', NULL, @newGovernanceExamples, 'Insert')
  END
  IF (@newGovernanceClearDivision IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivision', NULL, @newGovernanceClearDivision, 'Insert')
  END
  IF (@newBusinessPartners IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartners', NULL, @newBusinessPartners, 'Insert')
  END
  IF (@newIdentificationAndCommitment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitment', NULL, @newIdentificationAndCommitment, 'Insert')
  END
  IF (@newStakeholderEngagement IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagement', NULL, @newStakeholderEngagement, 'Insert')
  END
  IF (@newHumanRightsTraining IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTraining', NULL, @newHumanRightsTraining, 'Insert')
  END
  IF (@newRemedyProcessInPlace IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlace', NULL, @newRemedyProcessInPlace, 'Insert')
  END
  IF (@newGrievanceMechanismHasOperationalLevel IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevel', NULL, @newGrievanceMechanismHasOperationalLevel, 'Insert')
  END
  IF (@newGrievanceMechanismExistenceOfOperationalLevel IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevel', NULL, @newGrievanceMechanismExistenceOfOperationalLevel, 'Insert')
  END
  IF (@newGrievanceMechanismClearProcess IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcess', NULL, @newGrievanceMechanismClearProcess, 'Insert')
  END
  IF (@newGrievanceMechanismRightsNorms IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNorms', NULL, @newGrievanceMechanismRightsNorms, 'Insert')
  END
  IF (@newGrievanceMechanismFilingGrievance IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievance', NULL, @newGrievanceMechanismFilingGrievance, 'Insert')
  END
  IF (@newGrievanceMechanismReoccurringGrievances IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievances', NULL, @newGrievanceMechanismReoccurringGrievances, 'Insert')
  END
  IF (@newGrievanceMechanismFormatAndProcesse IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesse', NULL, @newGrievanceMechanismFormatAndProcesse, 'Insert')
  END
  IF (@newGesCommentCompanyPreparedness IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentCompanyPreparedness', NULL, @newGesCommentCompanyPreparedness, 'Insert')
  END
  IF (@newTotalScoreForCompanyPreparedness IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForCompanyPreparedness', NULL, @newTotalScoreForCompanyPreparedness, 'Insert')
  END
  IF (@newIsPublished IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IsPublished', NULL, @newIsPublished, 'Insert')
  END

  IF (@hasChangedValue = 'true')
  BEGIN
    EXEC UpdateGesUNGPAssessmentForm_Audit @newGesUNGPAssessmentFormId,
                                           @newI_GesCaseReports_Id,
                                           @audit_columns_list,
                                           'Insert',
                                           @newModifiedBy
  END