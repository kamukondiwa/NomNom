namespace NomNom {
    #region Using Directives

    using System.ComponentModel;
    using System.Configuration.Install;

    #endregion

    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer {
        public ProjectInstaller() {
            this.InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e) {
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e) {
        }
    }
}