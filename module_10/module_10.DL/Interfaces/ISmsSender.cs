using System.Threading.Tasks;

namespace module_10.DL.Interfaces
{
    public interface ISmsSender
    {
        Task NotifyBySms(string number, string message);

    }
}
