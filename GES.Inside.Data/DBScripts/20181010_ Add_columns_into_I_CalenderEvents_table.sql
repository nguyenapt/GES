IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents' 
		  AND COLUMN_NAME = 'EventTitle')
ALTER TABLE I_CalenderEvents ADD [EventTitle] nvarchar(500) null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents' 
		  AND COLUMN_NAME = 'EventLocation')
ALTER TABLE I_CalenderEvents ADD [EventLocation] nvarchar(500) null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents' 
		  AND COLUMN_NAME = 'EventEndDate')
ALTER TABLE I_CalenderEvents ADD [EventEndDate] Datetime null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents' 
		  AND COLUMN_NAME = 'StartTime')
ALTER TABLE I_CalenderEvents ADD [StartTime] Time null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents' 
		  AND COLUMN_NAME = 'EndTime')
ALTER TABLE I_CalenderEvents ADD [EndTime] Time null;


IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents' 
		  AND COLUMN_NAME = 'AllDayEvent')
ALTER TABLE I_CalenderEvents ADD [AllDayEvent] bit null;


CREATE TABLE [dbo].[GesEventCalendarUserAccept](
	[GesEventCalendarUserAcceptId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](500) NULL,
	[FullName] [nvarchar](500) NULL,
	[SendDate] [datetime] NULL,
	[I_CalenderEvents_Id] bigint NOT NULL,
	[IsSentUpdate] bit null,
	[UpdateSentDate] [datetime] NULL
 CONSTRAINT [PK_GesEventCalendarUserAccept] PRIMARY KEY CLUSTERED ([GesEventCalendarUserAcceptId] ASC)
 CONSTRAINT [FK_I_CalenderEvents_GesEventCalendarUserAccept] FOREIGN KEY ([I_CalenderEvents_Id])
 REFERENCES [I_CalenderEvents]([I_CalenderEvents_Id]));