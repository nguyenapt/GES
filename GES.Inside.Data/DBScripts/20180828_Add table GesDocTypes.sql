
CREATE TABLE [GesDocTypes]
(
 [GesDocTypesId] UNIQUEIDENTIFIER NOT NULL ,
 [Name]						NVARCHAR(300) NOT NULL ,
 [Type]           NVARCHAR(30) NOT NULL ,
 [Description]    NVARCHAR(500) NOT NULL ,
 [Created]        DATETIME NULL ,

 CONSTRAINT [PK_GesDocTypes] PRIMARY KEY CLUSTERED ([GesDocTypesId] ASC)
);
GO

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesDocTypes' 
		  AND COLUMN_NAME = 'Type') 
ALTER TABLE [dbo].[GesDocTypes] ADD [Type] NVARCHAR(30) NOT NULL


IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_EngagementTypes_GesDocument' 
		  AND COLUMN_NAME = 'DocumentTypeId') 
ALTER TABLE [dbo].[I_EngagementTypes_GesDocument] ADD [DocumentTypeId] uniqueidentifier NULL

IF NOT EXISTS (
	SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS 
    WHERE CONSTRAINT_NAME ='FK_I_EngagementTypes_GesDocument_GesDocTypes')
BEGIN
	ALTER TABLE I_EngagementTypes_GesDocument     
		ADD CONSTRAINT FK_I_EngagementTypes_GesDocument_GesDocTypes FOREIGN KEY ([DocumentTypeId])     
		REFERENCES GesDocTypes([GesDocTypesId]) 
END

INSERT INTO [dbo].[GesDocTypes]([GesDocTypesId],[Name],[Description],[Type],[Created]) VALUES(NEWID(),'Annual reports','Annual reports', 'GESREPORTDOCTYPE', GETDATE())
INSERT INTO [dbo].[GesDocTypes]([GesDocTypesId],[Name],[Description],[Type],[Created]) VALUES(NEWID(),'Quarterly reports','Quarterly reports','GESREPORTDOCTYPE', GETDATE())
GO
