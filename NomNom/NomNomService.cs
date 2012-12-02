namespace NomNom {
    #region Using Directives

    using System.ServiceProcess;

    using Contracts;

    using StructureMap;

    #endregion

    public partial class NomNomService : ServiceBase {
        private readonly IReadMonitorSettings readMonitorSettings;

        public NomNomService() {
            this.InitializeComponent();
            StructureMapInitialiser.Initialiser();
            this.readMonitorSettings = ObjectFactory.GetInstance<IReadMonitorSettings>();
            this.monitorController = ObjectFactory.GetInstance<IMonitorController>();
        }

        protected override void OnStart(string[] args) {
            var monitorSettings = this.readMonitorSettings.Read();
            this.monitorController.BeginMonitoring(monitorSettings);
        }

        protected override void OnStop() {
        }
    }
}