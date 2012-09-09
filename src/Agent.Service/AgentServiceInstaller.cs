using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace SignalKo.SystemMonitor.Agent.Service
{
    [RunInstaller(true)]
    public class AgentServiceInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller serviceProcessInstaller;

        public AgentServiceInstaller()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.serviceInstaller = new ServiceInstaller();
            this.serviceProcessInstaller = new ServiceProcessInstaller();

            this.serviceInstaller.Description = "System Monitor Agent";
            this.serviceInstaller.DisplayName = "SignalKo.SystemMonitor.Agent.Service";
            this.serviceInstaller.ServiceName = "SignalKo.SystemMonitor.Agent.Service";

            this.serviceProcessInstaller.Account = ServiceAccount.User;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;

            this.Installers.AddRange(new Installer[] { this.serviceProcessInstaller, this.serviceInstaller });
        }         
    }
}