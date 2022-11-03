
CREATE TABLE [I_GesCaseReportsG_ManagedDocuments]
(
 [Id]                    bigint IDENTITY(1,1) NOT NULL ,
 [I_GesCaseReports_Id]   bigint NOT NULL ,
 [G_ManagedDocuments_Id] bigint NOT NULL ,

 CONSTRAINT [PK_I_I_GesCaseReports_GesDocument] PRIMARY KEY CLUSTERED ([Id] ASC),
 CONSTRAINT [FK_29] FOREIGN KEY ([I_GesCaseReports_Id])  REFERENCES [I_GesCaseReports]([I_GesCaseReports_Id]),
 CONSTRAINT [FK_33] FOREIGN KEY ([G_ManagedDocuments_Id])  REFERENCES [G_ManagedDocuments]([G_ManagedDocuments_Id])
);
GO


CREATE NONCLUSTERED INDEX [fkIdx_29] ON [I_GesCaseReportsG_ManagedDocuments] 
 (
  [I_GesCaseReports_Id] ASC
 )

GO

CREATE NONCLUSTERED INDEX [fkIdx_32] ON [I_GesCaseReportsG_ManagedDocuments] 
 (
  [G_ManagedDocuments_Id] ASC
 )

GO

INSERT INTO [dbo].[G_ManagedDocumentServices]([Name]) VALUES('CaseProfile')

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > CompanyDocuments', 
          0,
          GETDATE(),
		  'ConfigCompanyDocument'
          )
          
          

INSERT INTO [dbo].[I_GesCaseProfileEntitiesGroup] ([I_GesCaseProfileEntitiesGroup_Id] ,[Name],[GroupType], [Order], [VisibleType]) 
VALUES (NEWID(),'Additional documents','ADDITIONAL-DOCUMENT',8, 3)

INSERT INTO [dbo].[I_GesCaseProfileEntities] ([GesCaseProfileEntity_Id],[Name] ,[Type],[Order],[Description],[I_GesCaseProfileEntitiesGroup_Id], [VisibleType]) 
VALUES (NEWID(),'Additional documents','ADDITIONAL-DOCUMENT',16,'',(select [I_GesCaseProfileEntitiesGroup_Id] from [I_GesCaseProfileEntitiesGroup] where [GroupType] = 'ADDITIONAL-DOCUMENT'), 3);
          
