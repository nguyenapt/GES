

/****** Object:  Table [dbo].[I_ControversialActivitesPresets]    Script Date: 2016-03-25 3:53:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[I_ControversialActivitesPresets](
	[I_ControversialActivitesPresets_Id] [dbo].[IId] IDENTITY(1,1) NOT NULL,
	[Name] [dbo].[Name] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_I_ControversialActivitesPresets_Created]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_I_ControversialActivitesPresets] PRIMARY KEY CLUSTERED 
(
	[I_ControversialActivitesPresets_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[I_ControversialActivitesPresetsItems]    Script Date: 2016-03-25 3:53:47 PM ******/
CREATE TABLE [dbo].[I_ControversialActivitesPresetsItems](
	[I_ControversialActivitesPresetsItems_Id] [dbo].[IId] IDENTITY(1,1) NOT NULL,
	[I_ControversialActivitesPresets_Id] [dbo].[IId] NOT NULL,
	[I_ControversialActivites_Id] [dbo].[IId] NOT NULL,
	[MinimumInvolvment] [smallint] NOT NULL CONSTRAINT [DF_I_ControversialActivitesPresetsItems_MinimumInvolvment]  DEFAULT ((0)),
	[Created] [datetime] NOT NULL CONSTRAINT [DF_I_ControversialActivitesPresetsItems_Created]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_I_ControversialActivitesPresetsItems] PRIMARY KEY CLUSTERED 
(
	[I_ControversialActivitesPresetsItems_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[I_ControversialActivitesPresetsItems]  WITH CHECK ADD  CONSTRAINT [FK_I_ControversialActivitesPresetsItems_I_ControversialActivites] FOREIGN KEY([I_ControversialActivites_Id])
REFERENCES [dbo].[I_ControversialActivites] ([I_ControversialActivites_Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[I_ControversialActivitesPresetsItems] CHECK CONSTRAINT [FK_I_ControversialActivitesPresetsItems_I_ControversialActivites]
GO

ALTER TABLE [dbo].[I_ControversialActivitesPresetsItems]  WITH CHECK ADD  CONSTRAINT [FK_I_ControversialActivitesPresetsItems_I_ControversialActivitesPresets] FOREIGN KEY([I_ControversialActivitesPresets_Id])
REFERENCES [dbo].[I_ControversialActivitesPresets] ([I_ControversialActivitesPresets_Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[I_ControversialActivitesPresetsItems] CHECK CONSTRAINT [FK_I_ControversialActivitesPresetsItems_I_ControversialActivitesPresets]
GO



