

DROP TABLE [GesUNGPAssessmentForm];
GO


DROP TABLE [GesUNGPAssessmentScores];
GO


--************************************** [GesUNGPAssessmentScores]

CREATE TABLE [GesUNGPAssessmentScores]
(
 [GesUNGPAssessmentScoresId] UNIQUEIDENTIFIER NOT NULL ,
 [Name]						NVARCHAR(500) NOT NULL ,
 [ScoreType]                NVARCHAR(30) NOT NULL ,
 [Score]                    float NOT NULL ,
 [Order]                    INT NULL ,
 [Description]              NVARCHAR(500) NOT NULL ,
 [Created]                  DATETIME NULL ,

 CONSTRAINT [PK_table_4] PRIMARY KEY CLUSTERED ([GesUNGPAssessmentScoresId] ASC)
);
GO



--************************************** [GesUNGPAssessmentForm]

CREATE TABLE [GesUNGPAssessmentForm]
(
 [GesUNGPAssessmentFormId]                       UNIQUEIDENTIFIER NOT NULL ,
 [I_GesCaseReports_Id]                           BIGINT NOT NULL ,
 [TheExtentOfHarmesScoreId]                       UNIQUEIDENTIFIER NULL ,
 [TheExtentOfHarmesScoreComment]                  NVARCHAR(500) NULL ,
 [TheNumberOfPeopleAffectedScoreId]               UNIQUEIDENTIFIER NULL ,
 [TheNumberOfPeopleAffectedScoreComment]          NVARCHAR(500) NULL ,
 [OverSeveralYearsScoreId]                        UNIQUEIDENTIFIER NULL ,
 [OverSeveralYearsScoreComment]                   NVARCHAR(500) NULL ,
 [SeveralLocationsScoreId]                        UNIQUEIDENTIFIER NULL ,
 [SeveralLocationsScoreComment]                   NVARCHAR(500) NULL ,
 [IsViolationScoreId]                             UNIQUEIDENTIFIER NULL ,
 [IsViolationScoreComment]                        NVARCHAR(500) NULL ,
 [GesConfirmedViolationScoreId]                   UNIQUEIDENTIFIER NULL ,
 [GesConfirmedViolationScoreComment]              NVARCHAR(500) NULL ,
 [SalientHumanRightsPotentialViolationTotalScore] float NULL ,
 [GesCommentSalientHumanRight]                   NVARCHAR(500) NULL ,
 [HumanRightsPolicyPubliclyDisclosedAddScoreId]  UNIQUEIDENTIFIER NULL ,
 [HumanRightsPolicyPubliclyDisclosed]            NVARCHAR(500) NULL ,
 [HumanRightsPolicyCommunicatedScoreId]          UNIQUEIDENTIFIER NULL ,
 [HumanRightsPolicyCommunicated]                 NVARCHAR(500) NULL ,
 [HumanRightsPolicyStipulatesScoreId]            UNIQUEIDENTIFIER NULL ,
 [HumanRightsPolicyStipulates]                   NVARCHAR(500) NULL ,
 [HumanRightsPolicyApprovedScoreId]              UNIQUEIDENTIFIER NULL ,
 [HumanRightsPolicyApproved]                     NVARCHAR(500) NULL ,
 [HumanRightsPolicyTotalScore]					 float NULL ,
 [GovernanceCommitmentScoreId]                   UNIQUEIDENTIFIER NULL ,
 [GovernanceCommitment]                          NVARCHAR(500) NULL ,
 [GovernanceExamplesScoreId]                     UNIQUEIDENTIFIER NULL ,
 [GovernanceExamples]                            NVARCHAR(500) NULL ,
 [GovernanceClearDivisionScoreId]                UNIQUEIDENTIFIER NULL ,
 [GovernanceClearDivision]                      NVARCHAR(500) NULL ,
 [BusinessPartners]                              NVARCHAR(500) NULL ,
 [BusinessPartnersAddScoreId]                    UNIQUEIDENTIFIER NULL ,
 [IdentificationAndCommitmentScoreId]            UNIQUEIDENTIFIER NULL ,
 [IdentificationAndCommitment]                   NVARCHAR(500) NULL ,
 [StakeholderEngagement]                         NVARCHAR(500) NULL ,
 [StakeholderEngagementAddScoreId]               UNIQUEIDENTIFIER NULL ,
 [HumanRightsTraining]                           NVARCHAR(500) NULL ,
 [HumanRightsTrainingScoreId]                    UNIQUEIDENTIFIER NULL ,
 [TotalScoreForHumanRightsDueDiligence]			 float NULL ,
 [RemedyProcessInPlaceScoreId]                   UNIQUEIDENTIFIER NULL ,
 [RemedyProcessInPlace]                          NVARCHAR(500) NULL ,
 [GrievanceMechanismHasOperationalLevelScoreId] UNIQUEIDENTIFIER NULL ,
 [GrievanceMechanismHasOperationalLevel]         NVARCHAR(500) NULL ,
 [GrievanceMechanismExistenceOfOperationalLevelScoreId]   UNIQUEIDENTIFIER NULL ,
 [GrievanceMechanismExistenceOfOperationalLevel] NVARCHAR(500) NULL ,
 [GrievanceMechanismClearProcessScoreId]         UNIQUEIDENTIFIER NULL ,
 [GrievanceMechanismClearProcess]				 NVARCHAR(500) NULL ,
 [GrievanceMechanismRightsNormsScoreId]          UNIQUEIDENTIFIER NULL ,
 [GrievanceMechanismRightsNorms]                 NVARCHAR(500) NULL ,
 [GrievanceMechanismFilingGrievanceScoreId]      UNIQUEIDENTIFIER NULL ,
 [GrievanceMechanismFilingGrievance]             NVARCHAR(500) NULL ,
 [GrievanceMechanismReoccurringGrievancesScoreId] UNIQUEIDENTIFIER NULL ,
 [GrievanceMechanismReoccurringGrievances]       NVARCHAR(500) NULL ,
 [GrievanceMechanismFormatAndProcesseScoreId]    UNIQUEIDENTIFIER NULL ,
 [GrievanceMechanismFormatAndProcesse]           NVARCHAR(500) NULL , 
 [TotalScoreForRemediationOfAdverseHumanRightsImpacts] float NULL ,
 [GesCommentCompanyPreparedness]                 NVARCHAR(500) NULL ,
 [TotalScoreForCompanyPreparedness]				 float NULL , 
 [SourcesName]									 NVARCHAR(500) NULL ,
 [SourcesLink]									 NVARCHAR(500) NULL ,
 [SourceDate]                                    DATETIME NULL ,
 [Modified]                                      DATETIME NULL ,
 [Created]                                       DATETIME NULL ,
 [ModifiedBy]                                    NVARCHAR(128) NULL , 

 CONSTRAINT [PK_GesUNGPAssessmentForm] PRIMARY KEY CLUSTERED ([GesUNGPAssessmentFormId] ASC),
 CONSTRAINT [FK_I_GesCaseReports_GesUNGPAssessmentForm] FOREIGN KEY ([I_GesCaseReports_Id])
  REFERENCES [I_GesCaseReports]([I_GesCaseReports_Id]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_1] FOREIGN KEY ([TheExtentOfHarmesScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_2] FOREIGN KEY ([TheNumberOfPeopleAffectedScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_3] FOREIGN KEY ([OverSeveralYearsScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_4] FOREIGN KEY ([SeveralLocationsScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_5] FOREIGN KEY ([IsViolationScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_6] FOREIGN KEY ([GesConfirmedViolationScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_7] FOREIGN KEY ([BusinessPartnersAddScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_8] FOREIGN KEY ([StakeholderEngagementAddScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_9] FOREIGN KEY ([HumanRightsTrainingScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_GesUNGPAssessmentScores_10] FOREIGN KEY ([HumanRightsPolicyPubliclyDisclosedAddScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_11] FOREIGN KEY ([HumanRightsPolicyCommunicatedScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_12] FOREIGN KEY ([HumanRightsPolicyStipulatesScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_13] FOREIGN KEY ([HumanRightsPolicyApprovedScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_14] FOREIGN KEY ([GovernanceCommitmentScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_15] FOREIGN KEY ([GovernanceExamplesScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_16] FOREIGN KEY ([GovernanceClearDivisionScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_17] FOREIGN KEY ([IdentificationAndCommitmentScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_18] FOREIGN KEY ([RemedyProcessInPlaceScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_19] FOREIGN KEY ([GrievanceMechanismHasOperationalLevelScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_20] FOREIGN KEY ([GrievanceMechanismExistenceOfOperationalLevelScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_21] FOREIGN KEY ([GrievanceMechanismClearProcessScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_22] FOREIGN KEY ([GrievanceMechanismRightsNormsScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_23] FOREIGN KEY ([GrievanceMechanismFilingGrievanceScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_24] FOREIGN KEY ([GrievanceMechanismReoccurringGrievancesScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
   CONSTRAINT [FK_GesUNGPAssessmentScores_25] FOREIGN KEY ([GrievanceMechanismFormatAndProcesseScoreId])
  REFERENCES [GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId]),
 CONSTRAINT [FK_ges.Users_GesUNGPAssessmentForm] FOREIGN KEY ([ModifiedBy])
  REFERENCES [ges].[Users]([Id]),
);

ALTER TABLE [dbo].[GesUNGPAssessmentForm] ADD  CONSTRAINT [DF_GesUNGPAssessmentForm_Created]  DEFAULT (getdate()) FOR [Created]
GO


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0 = no harm','SXOH',0,1,'no harm',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = minor','SXOH',1,2,'Minor: when the adverse human rights impact is less serious and during a relatively short period of time, e.g. minor health and safety incidents would be when no workers are seriously sickened, injured or killed.',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'2= major','SXOH',2,3,'Major: seriously harm peoples’ health, life situation and/or possibility to earn a living.',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0 = no one','SNPA',0,1,'no one',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = few','SNPA',1,2,'Few - less than 5 people (employees/contract workers/third parties)',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'2 = many','SNPA',2,3,'Many - more than 5 people seriously sickened, injured or killed (employees/contract workers/third parties)',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0 = no years','SOSY',0,1,'no years',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1-5 years','SOSY',1,2,'1-5 years',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'2 = more than 5 years','SOSY',2,3,'more than 5 years',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1-2 locations','SSLO',1,1,'1-2 locations',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'2 = 3 or more locations','SSLO',2,2,'3 or more locations',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0 = no','OVSO',0,1,'No – when there are no recent reports –(within a year); there is information that the company has addressed the problem; audit showed in supply chain that most of the issues has been addressed; the company reinstated the workers; paid compensation; provided proper living etc.',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = yes','OVSO',1,2,'No – when there are recent reports –(within a year); there is information that the company has addressed the problem; audit showed in supply chain that most of the issues has been addressed; the company reinstated the workers; paid compensation; provided proper living etc.',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0 = no','CVOI',0,1,'no',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = yes','CVOI',1,2,'yes',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HNMR',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5 = if any international norm is referred to','HNMR',0.5,2,'0.5 point should be given if any international norm is referred to',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HNMR',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = International norms most relevant to the company and the incident are referred to.','HNMR',1,4,'1 point should be given if international norms most relevant to the company and the incident are referred to.',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HSPC',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HSPC',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HSPC',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1 point should be given per fulfilled indicator. ','HSPC',1,4,'1 point should be given per fulfilled indicator. ',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HPSE',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HPSE', 0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HPSE',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1 point should be given per fulfilled indicator. ','HPSE',1,4,'1 point should be given per fulfilled indicator. ',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HPAP',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HPAP', 0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HPAP',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1 point should be given per fulfilled indicator. ','HPAP',1,4,'1 point should be given per fulfilled indicator. ',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HGWC',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HGWC', 0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HGWC',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1 point should be given per fulfilled indicator. ','HGWC',1,4,'1 point should be given per fulfilled indicator. ',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HGPE',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HGPE', 0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HGPE',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1 point should be given per fulfilled indicator. ','HGPE',1,4,'1 point should be given per fulfilled indicator. ',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HGCD',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HGCD', 0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HGCD',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1 point should be given per fulfilled indicator. ','HGCD',1,4,'1 point should be given per fulfilled indicator. ',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HBPR',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HBPR',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HBPR',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','HBPR',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HICD',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HICD', 0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HICD',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1 = 1 point should be given per fulfilled indicator. ','HICD',1,4,'1 point should be given per fulfilled indicator. ',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HBSE',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HBSE',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HBSE',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','HBSE',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','HBTC',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','HBTC',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','HBTC',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','HBTC',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())


INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','RRCP',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','RRCP',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','RRCP',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','RRCP',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','RGOM',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','RGOM',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','RGOM',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','RGOM',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','RGEO',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','RGEO',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','RGEO',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','RGEO',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','RGDC',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','RGDC',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','RGDC',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','RGDC',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','RGGA',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','RGGA',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','RGGA',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','RGGA',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','RGFG',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','RGFG',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','RGFG',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','RGFG',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','RGRG',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','RGRG',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','RGRG',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','RGRG',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())

INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.25','RGFP',0.25,1,'0.25',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.5','RGFP',0.5,2,'0.5',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'0.75','RGFP',0.75,3,'0.75',GETDATE())
INSERT INTO [dbo].[GesUNGPAssessmentScores]([GesUNGPAssessmentScoresId],[Name],[ScoreType],[Score],[Order],[Description],[Created])VALUES(NEWID(),'1','RGFP',1,4,'1 = 1 point should be given per fulfilled indicator',GETDATE())



GO


