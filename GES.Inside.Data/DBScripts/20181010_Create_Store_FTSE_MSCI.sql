
CREATE PROCEDURE [dbo].[I_SelectFtse_New]
AS
SELECT E.I_Ftse_Id as Id,E.ParentI_Ftse_Id as parentId, E.Code, Space(T.Depth*2) + E.Name AS Name, T.Lineage + Ltrim(str(T.I_Ftse_Tree_Id)) + '/' as Lineage
FROM I_Ftse E 
INNER JOIN I_Ftse_Tree T ON E.I_Ftse_Id=T.I_Ftse_Id
ORDER BY T.Lineage + Ltrim(Str(T.I_Ftse_Tree_Id,6,0))
GO

CREATE PROCEDURE [dbo].[I_SelectMsci_New]
AS
SELECT E.I_Msci_Id as Id,E.ParentI_Msci_Id as parentId,(SELECT [Name] FROM I_Msci O WHERE O.I_Msci_Id = E.ParentI_Msci_Id) ParentName, E.Code, Space(T.Depth*2) + E.Name AS Name, T.Lineage + Ltrim(str(T.I_Msci_Tree_Id)) + '/' as Lineage
FROM I_Msci E 
INNER JOIN I_Msci_Tree T ON E.I_Msci_Id=T.I_Msci_Id
ORDER BY T.Lineage + Ltrim(Str(T.I_Msci_Tree_Id,6,0))