
ALTER TRIGGER [dbo].[GesUNGPAssessmentForm_Delete]
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
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreId','The extent of harm''s score value', dbo.GetUNGPSCores(@oldTheExtentOfHarmesScoreId), NULL, 'Delete')
  END
  IF (@oldTheNumberOfPeopleAffectedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreId','The number of people affected''s score value', dbo.GetUNGPSCores(@oldTheNumberOfPeopleAffectedScoreId), NULL, 'Delete')
  END
  IF (@oldOverSeveralYearsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreId','Over several years''s score value', dbo.GetUNGPSCores(@oldOverSeveralYearsScoreId), NULL, 'Delete')
  END
  IF (@oldSeveralLocationsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreId','Several locations''s score value', dbo.GetUNGPSCores(@oldSeveralLocationsScoreId), NULL, 'Delete')
  END
  IF (@oldIsViolationScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreId','The (alleged) violation still occurring''s score value', dbo.GetUNGPSCores(@oldIsViolationScoreId), NULL, 'Delete')
  END
  IF (@oldGesConfirmedViolationScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreId','The case a GES’ confirmed violation of international norms''s score value', dbo.GetUNGPSCores(@oldGesConfirmedViolationScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyPubliclyDisclosedAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosedAddScoreId','Human rights policy - A publicly disclosed human rights policy''s score value', dbo.GetUNGPSCores(@oldHumanRightsPolicyPubliclyDisclosedAddScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyCommunicatedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicatedScoreId','Human rights policy - The company states''s score value', dbo.GetUNGPSCores(@oldHumanRightsPolicyCommunicatedScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyStipulatesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulatesScoreId','Human rights policy - The policy stipulates''s score value', dbo.GetUNGPSCores(@oldHumanRightsPolicyStipulatesScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyApprovedScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApprovedScoreId','Human rights policy - The policy is approved ''s score value', dbo.GetUNGPSCores(@oldHumanRightsPolicyApprovedScoreId), NULL, 'Delete')
  END
  IF (@oldGovernanceCommitmentScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitmentScoreId','Human rights due diligence - Governance - A written commitment''s score value', dbo.GetUNGPSCores(@oldGovernanceCommitmentScoreId), NULL, 'Delete')
  END
  IF (@oldGovernanceExamplesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamplesScoreId','Human rights due diligence - Governance - The company provides examples''s score value', dbo.GetUNGPSCores(@oldGovernanceExamplesScoreId), NULL, 'Delete')
  END
  IF (@oldGovernanceClearDivisionScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivisionScoreId','Human rights due diligence - Governance - The company provides a clear division of responsibility''s score value', dbo.GetUNGPSCores(@oldGovernanceClearDivisionScoreId), NULL, 'Delete')
  END
  IF (@oldBusinessPartnersAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartnersAddScoreId','Human rights due diligence - Business partners -  The company takes human rights considerations into account''s score value', dbo.GetUNGPSCores(@oldBusinessPartnersAddScoreId), NULL, 'Delete')
  END
  IF (@oldIdentificationAndCommitmentScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitmentScoreId','Human rights due diligence - Identification and commitment''s score value', dbo.GetUNGPSCores(@oldIdentificationAndCommitmentScoreId), NULL, 'Delete')
  END
  IF (@oldStakeholderEngagementAddScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagementAddScoreId','Human rights due diligence - Stakeholder engagement''s score value', dbo.GetUNGPSCores(@oldStakeholderEngagementAddScoreId), NULL, 'Delete')
  END
  IF (@oldHumanRightsTrainingScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTrainingScoreId','Human rights due diligence - Human rights training''s score value', dbo.GetUNGPSCores(@oldHumanRightsTrainingScoreId), NULL, 'Delete')
  END
  IF (@oldRemedyProcessInPlaceScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlaceScoreId','Remediation of adverse human rights impacts - Remedy process in place''s score value', dbo.GetUNGPSCores(@oldRemedyProcessInPlaceScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismHasOperationalLevelScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevelScoreId','Remediation of adverse human rights impacts - Grievance mechanism''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismHasOperationalLevelScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismExistenceOfOperationalLevelScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevelScoreId','Remediation of adverse human rights impacts - The existence of operational-level grievance mechanism''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismExistenceOfOperationalLevelScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismClearProcessScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcessScoreId','Remediation of adverse human rights impacts - The company discloses a clear process''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismClearProcessScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismRightsNormsScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNormsScoreId','Remediation of adverse human rights impacts - The company addresses a grievance''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismRightsNormsScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismFilingGrievanceScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievanceScoreId','Remediation of adverse human rights impacts - The people filing grievance''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismFilingGrievanceScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismReoccurringGrievancesScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievancesScoreId','Remediation of adverse human rights impacts - Reoccurring grievances on similar matters''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismReoccurringGrievancesScoreId), NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismFormatAndProcesseScoreId IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesseScoreId','Remediation of adverse human rights impacts - The format and processes related to the grievance mechanism''s score value', dbo.GetUNGPSCores(@oldGrievanceMechanismFormatAndProcesseScoreId), NULL, 'Delete')
  END
  IF (@oldSalientHumanRightsPotentialViolationTotalScore IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('SalientHumanRightsPotentialViolationTotalScore','Salient human rights potential violation total''s score value', @oldSalientHumanRightsPotentialViolationTotalScore, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyTotalScore IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyTotalScore','Human rights policy total''s score value', @oldHumanRightsPolicyTotalScore, NULL, 'Delete')
  END
  IF (@oldTotalScoreForHumanRightsDueDiligence IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForHumanRightsDueDiligence','Human rights due diligence total''s score value',  @oldTotalScoreForHumanRightsDueDiligence, NULL, 'Delete')
  END
  IF (@oldTotalScoreForRemediationOfAdverseHumanRightsImpacts IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForRemediationOfAdverseHumanRightsImpacts','Remediation of adverse human rights impacts total''s score value', @oldTotalScoreForRemediationOfAdverseHumanRightsImpacts, NULL, 'Delete')
  END
  IF (@oldTheExtentOfHarmesScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('TheExtentOfHarmesScoreComment', 'The extent of harm''s comments', @oldTheExtentOfHarmesScoreComment, NULL, 'Delete')
  END
  IF (@oldTheNumberOfPeopleAffectedScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('TheNumberOfPeopleAffectedScoreComment','The number of people affected''s comments', @oldTheNumberOfPeopleAffectedScoreComment, NULL, 'Delete')
  END
  IF (@oldOverSeveralYearsScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('OverSeveralYearsScoreComment','Over several years''s comments', @oldOverSeveralYearsScoreComment, NULL, 'Delete')
  END
  IF (@oldSeveralLocationsScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('SeveralLocationsScoreComment','Several locations''s comments', @oldSeveralLocationsScoreComment, NULL, 'Delete')
  END
  IF (@oldIsViolationScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('IsViolationScoreComment','The (alleged) violation still occurring''s comments', @oldIsViolationScoreComment, NULL, 'Delete')
  END
  IF (@oldGesConfirmedViolationScoreComment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GesConfirmedViolationScoreComment', 'The case a GES’ confirmed violation of international norms''s comments', @oldGesConfirmedViolationScoreComment, NULL, 'Delete')
  END
  IF (@oldGesCommentSalientHumanRight IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentSalientHumanRight','Level of Human rights salience - GES general comment''s comments', @oldGesCommentSalientHumanRight, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyPubliclyDisclosed IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyPubliclyDisclosed','Human rights policy - A publicly disclosed human rights policy''s comments', @oldHumanRightsPolicyPubliclyDisclosed, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyCommunicated IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyCommunicated','Human rights policy - The company states''s comments', @oldHumanRightsPolicyCommunicated, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyStipulates IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyStipulates','Human rights policy - The policy stipulates''s comments', @oldHumanRightsPolicyStipulates, NULL, 'Delete')
  END
  IF (@oldHumanRightsPolicyApproved IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsPolicyApproved','Human rights policy - The policy is approved''s comments', @oldHumanRightsPolicyApproved, NULL, 'Delete')
  END
  IF (@oldGovernanceCommitment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceCommitment', 'Human rights due diligence - Governance - A written commitment''s comments', @oldGovernanceCommitment, NULL, 'Delete')
  END
  IF (@oldGovernanceExamples IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceExamples','Human rights due diligence - Governance - The company provides examples''s comments', @oldGovernanceExamples, NULL, 'Delete')
  END
  IF (@oldGovernanceClearDivision IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GovernanceClearDivision','Human rights due diligence - Governance - The company provides a clear division of responsibility''s comments', @oldGovernanceClearDivision, NULL, 'Delete')
  END
  IF (@oldBusinessPartners IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('BusinessPartners','Human rights due diligence - Business partners -  The company takes human rights considerations into account''s comments', @oldBusinessPartners, NULL, 'Delete')
  END
  IF (@oldIdentificationAndCommitment IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('IdentificationAndCommitment', 'Human rights due diligence - Identification and commitment''s comments', @oldIdentificationAndCommitment, NULL, 'Delete')
  END
  IF (@oldStakeholderEngagement IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('StakeholderEngagement','Human rights due diligence - Stakeholder engagement''s comments', @oldStakeholderEngagement, NULL, 'Delete')
  END
  IF (@oldHumanRightsTraining IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('HumanRightsTraining','Human rights due diligence - Human rights training''s comments', @oldHumanRightsTraining, NULL, 'Delete')
  END
  IF (@oldRemedyProcessInPlace IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('RemedyProcessInPlace','Remediation of adverse human rights impacts - Remedy process in place''s comments', @oldRemedyProcessInPlace, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismHasOperationalLevel IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismHasOperationalLevel', 'Remediation of adverse human rights impacts - Grievance mechanism''s comments', @oldGrievanceMechanismHasOperationalLevel, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismExistenceOfOperationalLevel IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismExistenceOfOperationalLevel','Remediation of adverse human rights impacts - The existence of operational-level grievance mechanism''s comments',  @oldGrievanceMechanismExistenceOfOperationalLevel, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismClearProcess IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismClearProcess', 'Remediation of adverse human rights impacts - The company discloses a clear process''s comments',@oldGrievanceMechanismClearProcess, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismRightsNorms IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismRightsNorms', 'Remediation of adverse human rights impacts - The company addresses a grievance''s comments',@oldGrievanceMechanismRightsNorms, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismFilingGrievance IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFilingGrievance', 'Remediation of adverse human rights impacts - The people filing grievance''s comments',@oldGrievanceMechanismFilingGrievance, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismReoccurringGrievances IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismReoccurringGrievances','Remediation of adverse human rights impacts - Reoccurring grievances on similar matters''s comments', @oldGrievanceMechanismReoccurringGrievances, NULL, 'Delete')
  END
  IF (@oldGrievanceMechanismFormatAndProcesse IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GrievanceMechanismFormatAndProcesse','Remediation of adverse human rights impacts - The format and processes related to the grievance mechanism''s comments', @oldGrievanceMechanismFormatAndProcesse, NULL, 'Delete')
  END
  IF (@oldGesCommentCompanyPreparedness IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('GesCommentCompanyPreparedness','Salient human rights potential violation total''s comments',   @oldGesCommentCompanyPreparedness, NULL, 'Delete')
  END
  IF (@oldTotalScoreForCompanyPreparedness IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('TotalScoreForCompanyPreparedness','Company Preparedness total''s score value', @oldTotalScoreForCompanyPreparedness, NULL, 'Delete')
  END
  IF (@oldIsPublished IS NOT NULL)
  BEGIN
    SET @hasChangedValue = 'true'
    INSERT INTO @audit_columns_list (ColumnName, ColumnNameDescription,OldValue, NewValue, AuditDataState)
      VALUES ('IsPublished','Show in client', @oldIsPublished, NULL, 'Delete')
  END

  IF (@hasChangedValue = 'true')
  BEGIN
    EXEC UpdateGesUNGPAssessmentForm_Audit @oldGesUNGPAssessmentFormId,
                                           @oldI_GesCaseReports_Id,
                                           @audit_columns_list,
                                           'Delete',
                                           @oldModifiedBy
  END