using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShuffleAlert
{
    class ConsoleForm
    {
        private ControlContainer Container { get; set; } = new ControlContainer();
        private NotifyIcon NotifyIcon { get; set; }

        public ConsoleForm()
        {
            NotifyIcon = new NotifyIcon(Container);

            Task.Run(async () => await new ShuffleCheck().Run()).Wait();
        }
    }
}