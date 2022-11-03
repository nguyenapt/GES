using System.Collections.Generic;
using System.Threading.Tasks;
using GES.Common.Services.Interface;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;

namespace GES.Common.Services
{
    public class MailService : IMailService
    {
        private readonly string _mailPrefix;
        private readonly string _mailNoReply;
        private readonly string _mailNoReplyName;        
        private readonly string _contactEmail;        
        private readonly string _companyName;        
        private readonly MandrillApi _mandrillApi;

        public MailService(string apiKey, string mailPrefix, string mailNoReply, string mailNoReplyName, string contactEmail, string companyName)
        {
            _mandrillApi = new MandrillApi(apiKey);
            _mailPrefix = mailPrefix;
            _mailNoReply = mailNoReply;
            _mailNoReplyName = mailNoReplyName;
            _contactEmail = contactEmail;
            _companyName = companyName;
        }

        public async Task SendResetPasswordMail(string callbackUrl, string username, string email)
        {
            const string subject = "Reset your password";
            var emailMessage = GenerateEmailMessage(email, subject);
            emailMessage.AddGlobalVariable("CONTACT_EMAIL", _contactEmail);
            emailMessage.AddGlobalVariable("COMPANY_NAME", _companyName);
            emailMessage.AddRecipientVariable(email, "USERNAME", username);
            emailMessage.AddRecipientVariable(email, "PASSWORDRESETURL", callbackUrl);

            var request = new SendMessageTemplateRequest(emailMessage, "password-reset");
            await _mandrillApi.SendMessageTemplate(request);
        }

        private EmailMessage GenerateEmailMessage(string email, string subject)
        {
            return new EmailMessage
                   {
                       To = new[]
                            {
                                new EmailAddress
                                {
                                    Email = email
                                }
                            },
                       FromEmail = _mailNoReply,
                       FromName = _mailNoReplyName,
                       Subject = string.Format("{0} {1}", _mailPrefix, subject)
                   };
        }
    }
}
