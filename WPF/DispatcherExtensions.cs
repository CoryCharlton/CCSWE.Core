using System.Windows.Threading;

namespace CCSWE
{
    public static class DispatcherExtensions
    {
        /// <summary>
        /// Invokes an empty delegate on the <see cref="Dispatcher"/> to pump the message system like Application.DoEvents() from System.Windows.Forms would
        /// </summary>
        /// <param name="dispatcher">The <see cref="Dispatcher"/> to invoke the empty delegate on</param>
        public static void DoEvents(this Dispatcher dispatcher)
        {
            dispatcher.Invoke(delegate {}, DispatcherPriority.Background);
        }
    }
}
