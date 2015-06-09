using System.Windows.Threading;

namespace CCSWE
{
    public static class DispatcherExtensions
    {
        public static void DoEvents(this Dispatcher dispatcher)
        {
            dispatcher.Invoke(delegate {}, DispatcherPriority.Background);
        }
    }
}
