UPDATE [dbo].[G_Countries] SET IsoAlpha2 = 'GG' WHERE Name like 'Guernsey'
UPDATE [dbo].[G_Countries] SET IsoAlpha2 = 'RE' WHERE Name like 'Réunion'
UPDATE [dbo].[G_Countries] SET IsoAlpha2 = 'RS' WHERE Name like 'Serbia'
UPDATE [dbo].[G_Countries] SET IsoAlpha2 = 'ME' WHERE Name like 'Montenegro'
UPDATE [dbo].[G_Countries] SET IsoAlpha2 = 'CD', IsoAlpha3 = 'COD'  WHERE G_Countries_Id = 49
UPDATE [dbo].[G_Countries] SET IsoAlpha3 = 'COG'  WHERE G_Countries_Id = 58