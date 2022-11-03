using System;
using System.Web.Script.Serialization;

namespace GES.Inside.Data
{
    public class LinkHelper
    {
        private readonly bool _isNew;
        private readonly string _baseUrl;

        public LinkHelper(bool isNew, string baseUrl)
        {
            _isNew = isNew;
            _baseUrl = baseUrl;
        }
        public string GenBaseLink()
        {
            var baseUrl = _isNew ? _baseUrl : _baseUrl + "en-US/client/";
            return baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/";
        }

        public string GenCompanyLink(long companyId)
        {
            return _isNew
                ? String.Format("{0}Company/Profile/{1}", GenBaseLink(), companyId) :
                String.Format("{0}engagement_forum/company.aspx?I_Companies_Id={1}", GenBaseLink(), companyId);
        }

        public string GenIssueLink(long caseId, long? engagementTypeId, long companyId)
        {
            if (engagementTypeId == null)
                return GenBaseLink();

            return _isNew
                ? String.Format("{0}Company/CaseReport/{1}", GenBaseLink(), caseId) :
                String.Format("{0}engagement_forum/process.aspx?I_GesCaseReports_Id={1}&I_Companies_Id={2}&I_EngagementTypes_Id={3}", GenBaseLink(), caseId, companyId, engagementTypeId);
        }

        public string GenStandardLink(long id, long? templateId, long companyId)
        {
            if (templateId == null)
                return GenBaseLink();

            return _isNew
                ? String.Format("{0}Standard/Details/{1}", GenBaseLink(), id) :
                String.Format("{0}ges/case_profile.aspx?G_ReportingTemplates_Id={1}&I_Companies_Id={2}&I_GesCaseReports_Id={3}", GenBaseLink(), templateId, companyId, id);
        }
    }
}