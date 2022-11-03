IF NOT EXISTS (SELECT * FROM GesDocumentService WHERE Name = 'Oekom')
INSERT	INTO dbo.GesDocumentService
        ( Name, SortOrder )
VALUES  ( N'Oekom', 3 )