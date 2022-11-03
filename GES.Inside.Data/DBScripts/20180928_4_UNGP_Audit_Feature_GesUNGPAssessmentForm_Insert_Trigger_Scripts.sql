
ALTER TRIGGER [dbo].[GesUNGPAssessmentForm_Insert]
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
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreId','The extent of harm''s score value', NULL, dbo.GetUNGPSCores(@newTheExtentOfHarmesScoreId), 'Insert')
  END
  IF (@newTheNumberOfPeopleAffectedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreId','The number of people affected''s score value', NULL, dbo.GetUNGPSCores(@newTheNumberOfPeopleAffectedScoreId), 'Insert')
  END
  IF (@newOverSeveralYearsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreId', 'Over several years''s score value', NULL, dbo.GetUNGPSCores(@newOverSeveralYearsScoreId), 'Insert')
  END
  IF (@newSeveralLocationsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreId', 'Several locations''s score value', NULL, dbo.GetUNGPSCores(@newSeveralLocationsScoreId), 'Insert')
  END
  IF (@newIsViolationScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreId', 'The (alleged) violation still occurring''s score value', NULL, dbo.GetUNGPSCores(@newIsViolationScoreId), 'Insert')
  END
  IF (@newGesConfirmedViolationScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreId', 'The case a GES’ confirmed violation of international norms''s score value', NULL, dbo.GetUNGPSCores(@newGesConfirmedViolationScoreId), 'Insert')
  END
  IF (@newHumanRightsPolicyPubliclyDisclosedAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosedAddScoreId', 'Human rights policy - A publicly disclosed human rights policy''s score value', NULL, dbo.GetUNGPSCores(@newHumanRightsPolicyPubliclyDisclosedAddScoreId), 'Insert')
  END
  IF (@newHumanRightsPolicyCommunicatedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicatedScoreId', 'Human rights policy - The company states''s score value', NULL, dbo.GetUNGPSCores(@newHumanRightsPolicyCommunicatedScoreId), 'Insert')
  END
  IF (@newHumanRightsPolicyStipulatesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulatesScoreId', 'Human rights policy - The policy stipulates''s score value', NULL, dbo.GetUNGPSCores(@newHumanRightsPolicyStipulatesScoreId), 'Insert')
  END
  IF (@newHumanRightsPolicyApprovedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApprovedScoreId', 'Human rights policy - The policy is approved ''s score value', NULL, dbo.GetUNGPSCores(@newHumanRightsPolicyApprovedScoreId), 'Insert')
  END
  IF (@newGovernanceCommitmentScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitmentScoreId', 'Human rights due diligence - Governance - A written commitment''s score value', NULL, dbo.GetUNGPSCores(@newGovernanceCommitmentScoreId), 'Insert')
  END
  IF (@newGovernanceExamplesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamplesScoreId', 'Human rights due diligence - Governance - The company provides examples''s score value', NULL, dbo.GetUNGPSCores(@newGovernanceExamplesScoreId), 'Insert')
  END
  IF (@newGovernanceClearDivisionScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivisionScoreId', 'Human rights due diligence - Governance - The company provides a clear division of responsibility''s score value', NULL, dbo.GetUNGPSCores(@newGovernanceClearDivisionScoreId), 'Insert')
  END
  IF (@newBusinessPartnersAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartnersAddScoreId', 'Human rights due diligence - Business partners -  The company takes human rights considerations into account''s score value', NULL, dbo.GetUNGPSCores(@newBusinessPartnersAddScoreId), 'Insert')
  END
  IF (@newIdentificationAndCommitmentScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitmentScoreId', 'Human rights due diligence - Identification and commitment''s score value', NULL, dbo.GetUNGPSCores(@newIdentificationAndCommitmentScoreId), 'Insert')
  END
  IF (@newStakeholderEngagementAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagementAddScoreId', 'Human rights due diligence - Stakeholder engagement''s score value', NULL, dbo.GetUNGPSCores(@newStakeholderEngagementAddScoreId), 'Insert')
  END
  IF (@newHumanRightsTrainingScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTrainingScoreId', 'Human rights due diligence - Human rights training''s score value', NULL, dbo.GetUNGPSCores(@newHumanRightsTrainingScoreId), 'Insert')
  END
  IF (@newRemedyProcessInPlaceScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlaceScoreId', 'Remediation of adverse human rights impacts - Remedy process in place''s score value', NULL, dbo.GetUNGPSCores(@newRemedyProcessInPlaceScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismHasOperationalLevelScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevelScoreId', 'Remediation of adverse human rights impacts - Grievance mechanism''s score value', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismHasOperationalLevelScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismExistenceOfOperationalLevelScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevelScoreId', 'Remediation of adverse human rights impacts - The existence of operational-level grievance mechanism''s score value', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismExistenceOfOperationalLevelScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismClearProcessScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcessScoreId', 'Remediation of adverse human rights impacts - The company discloses a clear process''s score value', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismClearProcessScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismRightsNormsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNormsScoreId', 'Remediation of adverse human rights impacts - The company addresses a grievance''s score value', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismRightsNormsScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismFilingGrievanceScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievanceScoreId', 'Remediation of adverse human rights impacts - The people filing grievance''s score value', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismFilingGrievanceScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismReoccurringGrievancesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievancesScoreId', 'Remediation of adverse human rights impacts - Reoccurring grievances on similar matters''s score value', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismReoccurringGrievancesScoreId), 'Insert')
  END
  IF (@newGrievanceMechanismFormatAndProcesseScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesseScoreId', 'Remediation of adverse human rights impacts - The format and processes related to the grievance mechanism''s score value', NULL, dbo.GetUNGPSCores(@newGrievanceMechanismFormatAndProcesseScoreId), 'Insert')
  END
  IF (@newSalientHumanRightsPotentialViolationTotalScore IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('SalientHumanRightsPotentialViolationTotalScore', 'Salient human rights potential violation total''s score value', NULL, @newSalientHumanRightsPotentialViolationTotalScore, 'Insert')
  END
  IF (@newHumanRightsPolicyTotalScore IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyTotalScore', 'Human rights policy total''s score value', NULL, @newHumanRightsPolicyTotalScore, 'Insert')
  END
  IF (@newTotalScoreForHumanRightsDueDiligence IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForHumanRightsDueDiligence', 'Human rights due diligence total''s score value', NULL, @newTotalScoreForHumanRightsDueDiligence, 'Insert')
  END
  IF (@newTotalScoreForRemediationOfAdverseHumanRightsImpacts IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForRemediationOfAdverseHumanRightsImpacts', 'Remediation of adverse human rights impacts total''s score value', NULL, @newTotalScoreForRemediationOfAdverseHumanRightsImpacts, 'Insert')
  END
  IF (@newTheExtentOfHarmesScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreComment', 'The extent of harm''s comments', NULL, @newTheExtentOfHarmesScoreComment, 'Insert')
  END
  IF (@newTheNumberOfPeopleAffectedScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreComment', 'The number of people affected''s comments', NULL, @newTheNumberOfPeopleAffectedScoreComment, 'Insert')
  END
  IF (@newOverSeveralYearsScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreComment', 'Over several years''s comments', NULL, @newOverSeveralYearsScoreComment, 'Insert')
  END
  IF (@newSeveralLocationsScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreComment', 'Several locations''s comments', NULL, @newSeveralLocationsScoreComment, 'Insert')
  END
  IF (@newIsViolationScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreComment', 'The (alleged) violation still occurring''s comments', NULL, @newIsViolationScoreComment, 'Insert')
  END
  IF (@newGesConfirmedViolationScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreComment', 'The case a GES’ confirmed violation of international norms''s comments', NULL, @newGesConfirmedViolationScoreComment, 'Insert')
  END
  IF (@newGesCommentSalientHumanRight IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentSalientHumanRight', 'Level of Human rights salience - GES general comment''s comments', NULL, @newGesCommentSalientHumanRight, 'Insert')
  END
  IF (@newHumanRightsPolicyPubliclyDisclosed IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosed', 'Human rights policy - A publicly disclosed human rights policy''s comments', NULL, @newHumanRightsPolicyPubliclyDisclosed, 'Insert')
  END
  IF (@newHumanRightsPolicyCommunicated IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicated', 'Human rights policy - The company states''s comments', NULL, @newHumanRightsPolicyCommunicated, 'Insert')
  END
  IF (@newHumanRightsPolicyStipulates IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulates', 'Human rights policy - The policy stipulates''s comments', NULL, @newHumanRightsPolicyStipulates, 'Insert')
  END
  IF (@newHumanRightsPolicyApproved IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApproved', 'Human rights policy - The policy is approved''s comments', NULL, @newHumanRightsPolicyApproved, 'Insert')
  END
  IF (@newGovernanceCommitment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitment', 'Human rights due diligence - Governance - A written commitment''s comments', NULL, @newGovernanceCommitment, 'Insert')
  END
  IF (@newGovernanceExamples IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamples', 'Human rights due diligence - Governance - The company provides examples''s comments', NULL, @newGovernanceExamples, 'Insert')
  END
  IF (@newGovernanceClearDivision IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivision', 'Human rights due diligence - Governance - The company provides a clear division of responsibility''s comments', NULL, @newGovernanceClearDivision, 'Insert')
  END
  IF (@newBusinessPartners IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartners', 'Human rights due diligence - Business partners -  The company takes human rights considerations into account''s comments', NULL, @newBusinessPartners, 'Insert')
  END
  IF (@newIdentificationAndCommitment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitment', 'Human rights due diligence - Identification and commitment''s comments', NULL, @newIdentificationAndCommitment, 'Insert')
  END
  IF (@newStakeholderEngagement IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagement', 'Human rights due diligence - Stakeholder engagement''s comments', NULL, @newStakeholderEngagement, 'Insert')
  END
  IF (@newHumanRightsTraining IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTraining', 'Human rights due diligence - Human rights training''s comments', NULL, @newHumanRightsTraining, 'Insert')
  END
  IF (@newRemedyProcessInPlace IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlace', 'Remediation of adverse human rights impacts - Remedy process in place''s comments', NULL, @newRemedyProcessInPlace, 'Insert')
  END
  IF (@newGrievanceMechanismHasOperationalLevel IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevel', 'Remediation of adverse human rights impacts - Grievance mechanism''s comments', NULL, @newGrievanceMechanismHasOperationalLevel, 'Insert')
  END
  IF (@newGrievanceMechanismExistenceOfOperationalLevel IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevel', 'Remediation of adverse human rights impacts - The existence of operational-level grievance mechanism''s comments', NULL, @newGrievanceMechanismExistenceOfOperationalLevel, 'Insert')
  END
  IF (@newGrievanceMechanismClearProcess IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcess', 'Remediation of adverse human rights impacts - The company discloses a clear process''s comments', NULL, @newGrievanceMechanismClearProcess, 'Insert')
  END
  IF (@newGrievanceMechanismRightsNorms IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNorms', 'Remediation of adverse human rights impacts - The company addresses a grievance''s comments', NULL, @newGrievanceMechanismRightsNorms, 'Insert')
  END
  IF (@newGrievanceMechanismFilingGrievance IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievance', 'Remediation of adverse human rights impacts - The people filing grievance''s comments', NULL, @newGrievanceMechanismFilingGrievance, 'Insert')
  END
  IF (@newGrievanceMechanismReoccurringGrievances IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievances', 'Remediation of adverse human rights impacts - Reoccurring grievances on similar matters''s comments', NULL, @newGrievanceMechanismReoccurringGrievances, 'Insert')
  END
  IF (@newGrievanceMechanismFormatAndProcesse IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesse', 'Remediation of adverse human rights impacts - The format and processes related to the grievance mechanism''s comments', NULL, @newGrievanceMechanismFormatAndProcesse, 'Insert')
  END
  IF (@newGesCommentCompanyPreparedness IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentCompanyPreparedness', 'Salient human rights potential violation total''s comments', NULL, @newGesCommentCompanyPreparedness, 'Insert')
  END
  IF (@newTotalScoreForCompanyPreparedness IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForCompanyPreparedness', 'Company Preparedness total''s score value', NULL, @newTotalScoreForCompanyPreparedness, 'Insert')
  END
  IF (@newIsPublished IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'

	declare @newValueString  as nvarchar(200);

	  set @newValueString = 'No'
	  if (@newIsPublished = 1)
		set @newValueString = 'Yes'


	  INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription, OldValue, NewValue, AuditDataState)
		VALUES ('IsPublished', 'Show in client', NULL, @newValueString, 'Insert')
	

  END

  IF (@hasChangedValue = 'true')
  BEGIN
    EXEC UpdateGesUNGPAssessmentForm_Audit @newGesUNGPAssessmentFormId,
                                           @newI_GesCaseReports_Id,
                                           @audit_columns_list,
                                           'Insert',
                                           @newModifiedBy
  END