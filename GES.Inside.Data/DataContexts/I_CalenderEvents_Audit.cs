//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GES.Inside.Data.DataContexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class I_CalenderEvents_Audit
    {
        public Nullable<long> I_CalenderEvents_Id { get; set; }
        public long I_Companies_Id { get; set; }
        public System.DateTime EventDate { get; set; }
        public string Description { get; set; }
        public bool GesEvent { get; set; }
        public Nullable<System.DateTime> MinOfDeadline { get; set; }
        public Nullable<System.DateTime> ProxyDeadline { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }
        public string SpecialInstructions { get; set; }
        public string VotingInstructionNotes { get; set; }
        public string ResolutionNotes { get; set; }
        public string CollaborativeAction { get; set; }
        public string MeetingReport { get; set; }
        public System.DateTime Created { get; set; }
        public string AuditDataState { get; set; }
        public string AuditDMLAction { get; set; }
        public string AuditUser { get; set; }
        public Nullable<System.DateTime> AuditDateTime { get; set; }
        public string EventTitle { get; set; }
        public string EventLocation { get; set; }
        public Nullable<System.DateTime> EventEndDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<bool> AllDayEvent { get; set; }
    }
}