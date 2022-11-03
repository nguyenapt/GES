IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents_Audit' 
		  AND COLUMN_NAME = 'EventTitle')
ALTER TABLE I_CalenderEvents_Audit ADD [EventTitle] nvarchar(500) null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents_Audit' 
		  AND COLUMN_NAME = 'EventLocation')
ALTER TABLE I_CalenderEvents_Audit ADD [EventLocation] nvarchar(500) null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents_Audit' 
		  AND COLUMN_NAME = 'EventEndDate')
ALTER TABLE I_CalenderEvents_Audit ADD [EventEndDate] Datetime null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents_Audit' 
		  AND COLUMN_NAME = 'StartTime')
ALTER TABLE I_CalenderEvents_Audit ADD [StartTime] Time null;

IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents_Audit' 
		  AND COLUMN_NAME = 'EndTime')
ALTER TABLE I_CalenderEvents_Audit ADD [EndTime] Time null;


IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'I_CalenderEvents_Audit' 
		  AND COLUMN_NAME = 'AllDayEvent')
ALTER TABLE I_CalenderEvents_Audit ADD [AllDayEvent] bit null;