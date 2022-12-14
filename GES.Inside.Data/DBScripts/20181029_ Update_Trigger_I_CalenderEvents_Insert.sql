ALTER TRIGGER [dbo].[I_CalenderEvents_Insert] 
ON [dbo].[I_CalenderEvents] 
FOR INSERT 
AS 
 
 INSERT INTO dbo.I_CalenderEvents_Audit (
			[I_CalenderEvents_Id]
		   ,[I_Companies_Id]
           ,[EventDate]
           ,[Description]
           ,[GesEvent]
           ,[MinOfDeadline]
           ,[ProxyDeadline]
           ,[RecordDate]
           ,[SpecialInstructions]
           ,[VotingInstructionNotes]
		   ,[EventTitle]
           ,[EventLocation]
           ,[EventEndDate]
           ,[StartTime]
           ,[EndTime]
           ,[AllDayEvent]
           ,[ResolutionNotes]
           ,[CollaborativeAction]
           ,[MeetingReport]
           ,[Created]
           )
 SELECT [I_CalenderEvents_Id]
		   ,[I_Companies_Id]
           ,[EventDate]
           ,[Description]
           ,[GesEvent]
           ,[MinOfDeadline]
           ,[ProxyDeadline]
           ,[RecordDate]
           ,[SpecialInstructions]
           ,[VotingInstructionNotes]
		   ,[EventTitle]
           ,[EventLocation]
           ,[EventEndDate]
           ,[StartTime]
           ,[EndTime]
           ,[AllDayEvent],'New','Insert',SUSER_SNAME(),getdate()  FROM INSERTED
