-- ================================================
-- Template generated from Template Explorer using:
-- Create Trigger (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- See additional Create Trigger templates for more
-- examples of different Trigger statements.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[I_Companies_Update] 
   ON  [dbo].[I_Companies]
   FOR UPDATE
AS 
DECLARE @oldCompanyName as NVarchar(1024);
DECLARE @newCompanyName as NVarchar(1024);
DECLARE @audit_columns_list GesCaseReports_Audit_Columns_list;
DECLARE @IGesCaseReportsId IId;
DECLARE @CompaniesId IId;  


BEGIN
	select @CompaniesId = [I_Companies_Id], @newCompanyName = [MsciName] from INSERTED;
	select @oldCompanyName = [MsciName] from DELETED;		

		if (@newCompanyName <>  @oldCompanyName) 
		begin 
		Declare curP cursor For
		select [I_GesCaseReports_Id]  from I_GesCaseReports
		where I_GesCompanies_Id = (select I_GesCompanies_Id from I_GesCompanies where I_Companies_Id =  @CompaniesId)
		
		OPEN curP
		FETCH NEXT FROM curP INTO @IGesCaseReportsId
		
		WHILE @@FETCH_STATUS = 0 BEGIN
			INSERT INTO @audit_columns_list (TableName, ColumnName,OldValue, NewValue,AuditDataState)
			VALUES ('I_Companies','NameCoalesce', @oldCompanyName, @newCompanyName, 'Update')
			print @IGesCaseReportsId
			Exec GesUpdateGesCaseReportsAudit 'I_Companies', @IGesCaseReportsId, @audit_columns_list, 'Update'

			DELETE FROM @audit_columns_list;
			FETCH NEXT FROM curP INTO @IGesCaseReportsId
		END

		CLOSE curP    
		DEALLOCATE curP
	end 

END
GO
