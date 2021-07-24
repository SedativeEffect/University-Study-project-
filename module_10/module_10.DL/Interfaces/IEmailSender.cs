using System.Threading.Tasks;

namespace module_10.DL.Interfaces
{
    public interface IEmailSender
    {
        Task NotifyByEmail(string email, string subject, string message);

    }
}
