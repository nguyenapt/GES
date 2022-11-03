
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS ( SELECT  1
                FROM    sys.objects
                WHERE   object_id = OBJECT_ID(N'dbo.I_EngagementTypes_GesDocument')
                        AND type = N'U' )
    BEGIN  

        CREATE TABLE [dbo].[I_EngagementTypes_GesDocument]
            (
              [Id] [UNIQUEIDENTIFIER] NOT NULL ,
              [EngagementTypeId] [dbo].[IId] NULL ,
              [DocumentId] [UNIQUEIDENTIFIER] NULL ,
              [Created] [DATETIME] NULL ,
              CONSTRAINT [PK_I_EngagementTypes_GesDocument] PRIMARY KEY CLUSTERED
                ( [Id] ASC )
                WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                       IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
                       ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
            )
        ON  [PRIMARY]



        ALTER TABLE [dbo].[I_EngagementTypes_GesDocument] ADD  CONSTRAINT [DF_I_EngagementTypes_GesDocument_Id]  DEFAULT (NEWID()) FOR [Id]


        ALTER TABLE [dbo].[I_EngagementTypes_GesDocument] ADD  CONSTRAINT [DF_I_EngagementTypes_GesDocument_Created]  DEFAULT (GETUTCDATE()) FOR [Created]


        ALTER TABLE [dbo].[I_EngagementTypes_GesDocument]  WITH CHECK ADD  CONSTRAINT [FK_I_EngagementTypes_GesDocument_GesDocument] FOREIGN KEY([DocumentId])
        REFERENCES [dbo].[GesDocument] ([DocumentId])
        ON DELETE CASCADE


        ALTER TABLE [dbo].[I_EngagementTypes_GesDocument] CHECK CONSTRAINT [FK_I_EngagementTypes_GesDocument_GesDocument]


        ALTER TABLE [dbo].[I_EngagementTypes_GesDocument]  WITH CHECK ADD  CONSTRAINT [FK_I_EngagementTypes_GesDocument_I_EngagementTypes_GesDocument] FOREIGN KEY([EngagementTypeId])
        REFERENCES [dbo].[I_EngagementTypes] ([I_EngagementTypes_Id])
        ON DELETE CASCADE


        ALTER TABLE [dbo].[I_EngagementTypes_GesDocument] CHECK CONSTRAINT [FK_I_EngagementTypes_GesDocument_I_EngagementTypes_GesDocument]
    END

	GO
IF NOT EXISTS (SELECT * FROM GesDocumentService WHERE Name = 'EngTyp')
	INSERT	INTO dbo.GesDocumentService
        ( Name, SortOrder )
	VALUES  ( N'EngTyp', 5 )