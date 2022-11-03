--Only run when have request update old data--
GO
DISABLE TRIGGER dbo.I_Milestones_Update ON dbo.I_Milestones;
DISABLE TRIGGER dbo.I_GesCompanyDialogues_Update ON dbo.I_GesCompanyDialogues;
GO
UPDATE I_Milestones set MilestoneModified = CAST(FORMAT(MilestoneModified,'yyyy-MM-dd 0:0') AS datetime)
GO
UPDATE I_GesCompanyDialogues set ContactDate = CAST(FORMAT(ContactDate,'yyyy-MM-dd 0:0') AS datetime)
GO
UPDATE I_GesSourceDialogues set ContactDate = CAST(FORMAT(ContactDate,'yyyy-MM-dd 0:0') AS datetime)
GO
ENABLE TRIGGER dbo.I_Milestones_Update ON dbo.I_Milestones; 
ENABLE TRIGGER dbo.I_GesCompanyDialogues_Update ON dbo.I_GesCompanyDialogues; 
GO




