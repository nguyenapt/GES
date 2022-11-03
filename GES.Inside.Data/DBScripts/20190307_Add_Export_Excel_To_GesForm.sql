
IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesForm' 
		  AND COLUMN_NAME = 'SortOrder')
ALTER TABLE ges.GesForm ADD [SortOrder] int null;

GO

UPDATE ges.GesForm
SET SortOrder = Id * 5

GO

INSERT INTO	 ges.GesForm
        ( Name ,
          IsInClientSite ,
          Created ,
          FormKey ,
          SortOrder
        )
VALUES  ( N'Export > Screening Report (Excel)' , -- Name - nvarchar(max)
          0 , -- IsInClientSite - bit
          GETDATE() , -- Created - datetime
          N'ExportScreeningReport' , -- FormKey - nvarchar(255)
          41  -- SortOrder - int
        )


INSERT INTO	 ges.GesForm
        ( Name ,
          IsInClientSite ,
          Created ,
          FormKey ,
          SortOrder
        )
VALUES  ( N'Export > EF Status Report' , -- Name - nvarchar(max)
          0 , -- IsInClientSite - bit
          GETDATE() , -- Created - datetime
          N'ExportEFStatusReport' , -- FormKey - nvarchar(255)
          42  -- SortOrder - int
        )
