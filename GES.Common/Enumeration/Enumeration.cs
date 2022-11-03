using System.ComponentModel;

namespace GES.Common.Enumeration
{
    public enum ActionEnum
    {
        Read = 1,
        Create = 2,
        Update = 3,
        Delete = 4
    }

    public enum ClaimEnum
    {
        [Description("Endorsement")]
        Endorserment,

        [Description("Use inside site")]
        AccessInside 
    }

    public enum ReportEnum
    {
        Anual = 1,
        Quarterly = 2,
        Position = 3
    }
    
    public enum SynchronizationFileType
    {
        OekomDataFile = 4,
    }

    public enum DocumentServices
    {
        ClientsReportsAnnual =1, 
        ClientsReportsQuarterly = 2,
        ClientsReportsPositionPapers = 3,
        Oekom = 4,
        Sdg = 5,
        EngTyp = 6
    }    
    public enum GManagedDocumentService
    {
        Company = 6,
        Controversial = 8, 
        CaseProfile = 30,
        ExternalDocument = 29
    }


    public enum EngagementTypeEnum
    {
        Conventions = 2,
        WestenSahara = 4,
        Burnma = 5,
        EmerginMarket = 9,
        ExtractiveSector = 13,
        PalmOil = 14,
        LowPerformers = 16,
        CarbonRisk = 17,
        BusinessConductExtendedTaxation = 18,
        Water = 19,
        Taxation = 20,
        Governance = 26
    }

    public enum EngagementTypeCategoryEnum
    {
        BusinessConduct = 1,
        StewardshipAndRisk = 2,
        RiskTransparency = 3,
        PalmOilBenchmarkStudy = 5,
        Bespoke = 6,
        Tailored = 7,
        Governance = 8
    }

    public enum SortOrderByKeyword
    {
        FirstOrder = 1,
        SecondOrder = 2,
        ThirdOrder = 3,
        FourthOrder = 4
    }

    public enum RecommendationType
    {
        Resolved = 3,
        Evaluate = 6,
        Engage = 7,
        Disengage = 8,
        Archived = 9,
        ResolvedIndicationOfViolation = 10
    }

    public enum ConclusionType
    {
        IndicationOfViolation = 1,
        ConfirmedViolation = 2,
        Resolved = 3,
        Archived = 5,
        Alert = 11
    }

    public enum EngagementStatisticType
    {
        Contacts,
        Meetings,
        Dialogues,
        ConferenceCalls,
        Emails
    }

    public enum ContactType
    {
        Email = 1,
        PostalMail = 2,
        Telephone = 3,
        Meeeting = 4,
        Fax = 5,
        ConferenceCall = 6,
        ArchivedDialogue = 7
    }

    public enum GesCaseReportStatus
    {
        IndicationOfViolation = 1,
        ConfirmedViolation = 2,
        Resolved = 3,
        ArchivedOfConclution = 5,
        Evaluate = 6,
        Engage = 7,
        Disengage = 8,
        ArchivedOfRecommendation = 9,
        ResolvedIndicationOfViolation = 10,
        Alert = 11
    }

    public enum ProcessStatus
    {
        CountryRisk = 1,
        Alert = 2,
        Dialogue = 3,
        IncidentAnalysis = 4,
        RiskRatinSummary = 5,
        CaseProfile = 7,
        Exit = 12,
        Closed = 13,
        ExecutiveSummary = 14,
        Controversial = 15,
        IncidentHistory = 16,
        RiskBenchmark = 17,
        Management = 18,
        EngagementProfile = 19,
        SwotGap = 20
    }

    public enum ReportingTemplates
    {
        StandardConfirmedViolations = 23,
        StandardIndicationViolations = 24
    }

    public enum GesService
    {
        GesAlertServices = 16,
        GesEngagementForum = 19,
        GesGlobalEthicalStandard = 20,
        GesRiskRating = 21,
        GesControversial = 22,
        BusinessConduct = 30,
        Burnma = 31,
        EmerginMarkets = 32,
        PalmOil = 35,
        CarbonRisk = 44,
        Water = 45,
        Taxation = 46,
        Governance = 53, //Governance AGM related
        GovernanceOngoing = 63
    }

    public enum SignUpValue
    {
        None = 0,
        Active = 1,
        Passive = 2
    }

    public enum SecurityGroups
    {
        Administrator = 1,
        PowerUser = 2,
        User = 3,
        Guest = 4,
        Analyst = 5
    }

    public enum ContactDirection
    {
        Incoming = 1,
        Outgoing = 2,
        Meeting = 4
    }

    public enum TimeLineCommonType
    {
        Recommendation = 0,
        Conclusion = 1,
        MileStone = 2
    }
    
    public enum UngpScoreType
    {
        SXOH, //Scale: The extent of harm
        SNPA, // Scale: The number of people affected
        SOSY, // Systematic: Over several years
        SSLO, // Systematic: Several locations
        OVSO, // Ongoing: Is the (alleged) violation still occurring?
        CVOI, // Confirmed: Is the case a GES’ confirmed violation of international norms?
        HNMR, // Human rights policy: disclosed human rights policy additional 0.5 point
        HBPR, // Human rights due diligence: Business partners additional 0.5 point
        HBSE, // Human rights due diligence: Stakeholder engagement additional 0.5 point
        HBTC, // Human rights trainning: additional 0.5 point
        HSPC, //
        HPSE, // 
        HPAP, //
        HGWC, //
        HGPE, // 
        HGCD,
        HICD,
        RRCP,
        RGOM,
        RGEO,
        RGDC,
        RGGA,
        RGFG,
        RGRG,
        RGFP
        
    }

    public enum ClientType
    {
        BusinessConduct = 0,
        GlobalEthicalStandardOnly = 1,
        GlobalEthicalStandardAndOtherEngagmement = 2,
        HasAnyEngagementTypeService = 3,
        Other = 4
    }

    public enum NaType
    {
        Alert = 1,
        AlertIncidentWatch = 2,
        Extended = 3,
        ExtendedIncidentWatch = 3
    }

    public enum SearchCompanyResult
    {
        Matched = 1,
        NotFound = 2,
        OutsidePortfolio = 3
    }

    public enum RelatedDocumentMng
    {
        Company = 1,
        CompanyDialog = 2,
        SourceDialog = 3
    }
    public enum ScreeningNormTheme
    {
        GlobalEthicalStandard = 1,
        BusinessConduct = 2,
        CorporateGovernance = 3,
        StewardshipAndRisk = 4
    }

    public enum DocumentService
    {
        ExternalDocument = 29,
        CaseProfile = 30
    }
}
