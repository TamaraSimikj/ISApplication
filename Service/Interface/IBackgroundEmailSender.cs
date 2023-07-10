using System.Threading.Tasks;

namespace ClassLibrary1.Interface
{
    public interface IBackgroundEmailSender
    {
        public Task DoWork();
    }
}