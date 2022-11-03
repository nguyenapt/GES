CREATE TRIGGER [dbo].[GesUNGPAssessmentForm_Delete]
ON [dbo].[GesUNGPAssessmentForm]
FOR DELETE
AS
  DECLARE @audit_columns_list [GesUNGPAssessmentForm_Audit_Columns_list];
  DECLARE @oldGesUNGPAssessmentFormId AS varchar(200);
  DECLARE @oldI_GesCaseReports_Id AS varchar(200);

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
    @oldGesUNGPAssessmentFormId = CAST([GesUNGPAssessmentFormId] AS uniqueidentifier),
    @oldI_GesCaseReports_Id = [I_GesCaseReports_Id],
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

  IF (@oldTheExtentOfHarmesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreId', dbo.GetUNGPSCores(@oldTheExtentOfHarmesScoreId), NULL, 'Delete')
  END
  IF (@oldTheNumberOfPeopleAffectedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreId', dbo.GetUNGPSCores(@oldTheNumberOfPeopleAffectedScoreId), NULL, 'Delete')
  END
  IF (@oldOverSeveralYearsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreId', dbo.GetUNGPSCores(@oldOverSeveralYearsScoreId), NULL, 'Delete')
  END
  IF (@oldSeveralLocationsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreId', dbo.GetUNGPSCores(@oldSeveralLocationsScoreId), NULL, 'Delete')
  END
  IF (@oldIsViolationScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreId', dbo.GetUNGPSCores(@oldIsViolationScoreId), NULL, 'Delete')
  END
  IF (@oldGesConfirmedViolationScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreId', dbo.GetUNGPSCores(@oldGesConfirmedViolationScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyPubliclyDisclosedAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosedAddScoreId', dbo.GetUNGPSCores(@oldHumanRightsPolicyPubliclyDisclosedAddScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyCommunicatedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicatedScoreId', dbo.GetUNGPSCores(@oldHumanRightsPolicyCommunicatedScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyStipulatesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulatesScoreId', dbo.GetUNGPSCores(@oldHumanRightsPolicyStipulatesScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyApprovedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApprovedScoreId', dbo.GetUNGPSCores(@oldHumanRightsPolicyApprovedScoreId), NULL, 'Delete')
  END
  IF (@oldGovernanceCommitmentScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitmentScoreId', dbo.GetUNGPSCores(@oldGovernanceCommitmentScoreId), NULL, 'Delete')
  END
  IF (@oldGovernanceExamplesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamplesScoreId', dbo.GetUNGPSCores(@oldGovernanceExamplesScoreId), NULL, 'Delete')
  END
  IF (@oldGovernanceClearDivisionScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivisionScoreId', dbo.GetUNGPSCores(@oldGovernanceClearDivisionScoreId), NULL, 'Delete')
  END
  IF (@oldBusinessPartnersAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartnersAddScoreId', dbo.GetUNGPSCores(@oldBusinessPartnersAddScoreId), NULL, 'Delete')
  END
  IF (@oldIdentificationAndCommitmentScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitmentScoreId', dbo.GetUNGPSCores(@oldIdentificationAndCommitmentScoreId), NULL, 'Delete')
  END
  IF (@oldStakeholderEngagementAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagementAddScoreId', dbo.GetUNGPSCores(@oldStakeholderEngagementAddScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsTrainingScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTrainingScoreId', dbo.GetUNGPSCores(@oldHumanRightsTrainingScoreId), NULL, 'Delete')
  END
  IF (@oldRemedyProcessInPlaceScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlaceScoreId', dbo.GetUNGPSCores(@oldRemedyProcessInPlaceScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismHasOperationalLevelScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevelScoreId', dbo.GetUNGPSCores(@oldGrievanceMechanismHasOperationalLevelScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismExistenceOfOperationalLevelScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevelScoreId', dbo.GetUNGPSCores(@oldGrievanceMechanismExistenceOfOperationalLevelScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismClearProcessScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcessScoreId', dbo.GetUNGPSCores(@oldGrievanceMechanismClearProcessScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismRightsNormsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNormsScoreId', dbo.GetUNGPSCores(@oldGrievanceMechanismRightsNormsScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismFilingGrievanceScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievanceScoreId', dbo.GetUNGPSCores(@oldGrievanceMechanismFilingGrievanceScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismReoccurringGrievancesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievancesScoreId', dbo.GetUNGPSCores(@oldGrievanceMechanismReoccurringGrievancesScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismFormatAndProcesseScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesseScoreId', dbo.GetUNGPSCores(@oldGrievanceMechanismFormatAndProcesseScoreId), NULL, 'Delete')
  END
  IF (@oldSalientHumanRightsPotentialViolationTotalScore IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('SalientHumanRightsPotentialViolationTotalScore', @oldSalientHumanRightsPotentialViolationTotalScore, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyTotalScore IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyTotalScore', @oldHumanRightsPolicyTotalScore, NULL, 'Delete')
  END
  IF (@oldTotalScoreForHumanRightsDueDiligence IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForHumanRightsDueDiligence', @oldTotalScoreForHumanRightsDueDiligence, NULL, 'Delete')
  END
  IF (@oldTotalScoreForRemediationOfAdverseHumanRightsImpacts IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForRemediationOfAdverseHumanRightsImpacts', @oldTotalScoreForRemediationOfAdverseHumanRightsImpacts, NULL, 'Delete')
  END
  IF (@oldTheExtentOfHarmesScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreComment', @oldTheExtentOfHarmesScoreComment, NULL, 'Delete')
  END
  IF (@oldTheNumberOfPeopleAffectedScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreComment', @oldTheNumberOfPeopleAffectedScoreComment, NULL, 'Delete')
  END
  IF (@oldOverSeveralYearsScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreComment', @oldOverSeveralYearsScoreComment, NULL, 'Delete')
  END
  IF (@oldSeveralLocationsScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreComment', @oldSeveralLocationsScoreComment, NULL, 'Delete')
  END
  IF (@oldIsViolationScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreComment', @oldIsViolationScoreComment, NULL, 'Delete')
  END
  IF (@oldGesConfirmedViolationScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreComment', @oldGesConfirmedViolationScoreComment, NULL, 'Delete')
  END
  IF (@oldGesCommentSalientHumanRight IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentSalientHumanRight', @oldGesCommentSalientHumanRight, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyPubliclyDisclosed IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosed', @oldHumanRightsPolicyPubliclyDisclosed, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyCommunicated IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicated', @oldHumanRightsPolicyCommunicated, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyStipulates IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulates', @oldHumanRightsPolicyStipulates, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyApproved IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApproved', @oldHumanRightsPolicyApproved, NULL, 'Delete')
  END
  IF (@oldGovernanceCommitment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitment', @oldGovernanceCommitment, NULL, 'Delete')
  END
  IF (@oldGovernanceExamples IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamples', @oldGovernanceExamples, NULL, 'Delete')
  END
  IF (@oldGovernanceClearDivision IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivision', @oldGovernanceClearDivision, NULL, 'Delete')
  END
  IF (@oldBusinessPartners IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartners', @oldBusinessPartners, NULL, 'Delete')
  END
  IF (@oldIdentificationAndCommitment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitment', @oldIdentificationAndCommitment, NULL, 'Delete')
  END
  IF (@oldStakeholderEngagement IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagement', @oldStakeholderEngagement, NULL, 'Delete')
  END
  IF (@oldHumanRightsTraining IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTraining', @oldHumanRightsTraining, NULL, 'Delete')
  END
  IF (@oldRemedyProcessInPlace IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlace', @oldRemedyProcessInPlace, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismHasOperationalLevel IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevel', @oldGrievanceMechanismHasOperationalLevel, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismExistenceOfOperationalLevel IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevel', @oldGrievanceMechanismExistenceOfOperationalLevel, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismClearProcess IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcess', @oldGrievanceMechanismClearProcess, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismRightsNorms IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNorms', @oldGrievanceMechanismRightsNorms, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismFilingGrievance IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievance', @oldGrievanceMechanismFilingGrievance, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismReoccurringGrievances IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievances', @oldGrievanceMechanismReoccurringGrievances, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismFormatAndProcesse IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesse', @oldGrievanceMechanismFormatAndProcesse, NULL, 'Delete')
  END
  IF (@oldGesCommentCompanyPreparedness IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentCompanyPreparedness', @oldGesCommentCompanyPreparedness, NULL, 'Delete')
  END
  IF (@oldTotalScoreForCompanyPreparedness IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForCompanyPreparedness', @oldTotalScoreForCompanyPreparedness, NULL, 'Delete')
  END
  IF (@oldIsPublished IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, OldValue, NewValue, AuditDataState)
      VALUES ('IsPublished', @oldIsPublished, NULL, 'Delete')
  END

  IF (@hasChangedValue = 'true')
  BEGIN
    EXEC UpdateGesUNGPAssessmentForm_Audit @oldGesUNGPAssessmentFormId,
                                           @oldI_GesCaseReports_Id,
                                           @audit_columns_list,
                                           'Delete',
                                           @oldModifiedBy
  END