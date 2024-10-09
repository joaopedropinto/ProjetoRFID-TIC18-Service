using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace RFID_WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {
                try
                {
                    string parameter = string.Concat(args);
                    switch (parameter)
                    {
                        case "--install":
                            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                            break;
                        case "--uninstall":
                            ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                            break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new RfidWindowsService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

        
    }


}
