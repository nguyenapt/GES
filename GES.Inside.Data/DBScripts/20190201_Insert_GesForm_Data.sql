--DBCC CHECKIDENT ('ges.GesForm', RESEED, 1);

 IF NOT EXISTS(SELECT *
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'GesForm' 
		  AND COLUMN_NAME = 'FormKey')
ALTER TABLE ges.GesForm ADD [FormKey] nvarchar(255) null;

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Companies',
          0,
          GETDATE(),
		  'Company'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Cases > List', 
          0,
          GETDATE(),
		  'Case'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Cases > Endorsement', 
          0,
          GETDATE(),
		  'Endorsement'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Contacts', 
          0,
          GETDATE(),
		  'Contact'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Clients', 
          0,
          GETDATE(),
		  'Client'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Portfolios > Portfolio list', 
          0,
          GETDATE(),
		  'Portfolio'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Portfolios > ControActive presets', 
          0,
          GETDATE(),
		  'ControActivePreset'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Accounts', 
          0,
          GETDATE(),
		  'Account'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Utilities > Data Utilities', 
          0,
          GETDATE(),
		  'DataUtility'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Utilities > Format ISINs', 
          0,
          GETDATE(),
		  'FormatISIN'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Utilities > Tools', 
          0,
          GETDATE(),
		  'UtilityTool'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > SDGs', 
          0,
          GETDATE(),
		  'ConfigSDG'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > Services', 
          0,
          GETDATE(),
		  'ConfigService'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > Engagement types', 
          0,
          GETDATE(),
		  'ConfigEngagementType'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > Case profile templates', 
          0,
          GETDATE(),
		  'ConfigCaseProfileTemplate'
          )

INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > Sustainalytics Announcements', 
          0,
          GETDATE(),
		  'ConfigAnnouncement'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > Conventions', 
          0,
          GETDATE(),
		  'ConfigConvention'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > Guidelines', 
          0,
          GETDATE(),
		  'ConfigGuideline'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > Documents', 
          0,
          GETDATE(),
		  'ConfigDocument'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > AccountRoles', 
          0,
          GETDATE(),
		  'ConfigAccountRole'
          )
INSERT INTO ges.GesForm
        ( Name, IsInClientSite, Created, FormKey )
VALUES  ( N'Config > Glossary', 
          0,
          GETDATE(),
		  'ConfigGlossary'
          )
