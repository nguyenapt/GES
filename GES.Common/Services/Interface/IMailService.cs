using System.Threading.Tasks;

namespace GES.Common.Services.Interface
{
    public interface IMailService
    {
        Task SendResetPasswordMail(string callbackUrl, string username, string email);
    }
}
