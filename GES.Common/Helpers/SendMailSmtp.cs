using System;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace GES.Common.Helpers
{
    public class SendMailSmtp
    {
        public static SmtpClient GetSmtpClient(string host, int port, System.Net.NetworkCredential networkCredential, bool enableSsl)
        {
            var smtpclient = new SmtpClient
            {
                Host = host,
                Port = port,
                Credentials = networkCredential,
                EnableSsl = enableSsl
            };

            return smtpclient;
        }
        
        public static void SendEmail(MailMessage msg, SmtpClient smtpClient, string eventMessageContent, ContentType mailContentType)
        {
            var avCal = AlternateView.CreateAlternateViewFromString(eventMessageContent, mailContentType);
            avCal.TransferEncoding = TransferEncoding.EightBit;
            msg.AlternateViews.Add(avCal);
            
            smtpClient.Send(msg);
        }        
    }
}