using System.ServiceProcess;
using System.ServiceModel;
using WcfServiceLibrary1;

namespace RFID_WindowsService
{
    public partial class RfidWindowsService : ServiceBase
    {
        internal static ServiceHost myServiceHost = null; 

        public RfidWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (myServiceHost != null)
            {
                myServiceHost.Close();
            }
            myServiceHost = new ServiceHost(typeof(Service1));
            myServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (myServiceHost != null)
            {
                myServiceHost.Close();
                myServiceHost = null;
            }
        }
    }
}
