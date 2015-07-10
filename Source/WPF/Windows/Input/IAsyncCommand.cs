using System.Threading.Tasks;
using System.Windows.Input;

namespace CCSWE.Windows.Input
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
