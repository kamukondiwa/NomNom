namespace NomNom {
    #region Using Directives

    using System.ServiceProcess;

    #endregion

    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main() {
            var ServicesToRun = new ServiceBase[] { new NomNomService() };
            ServiceBase.Run(ServicesToRun);
        }
    }
}