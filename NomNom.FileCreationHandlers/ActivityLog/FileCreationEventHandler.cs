namespace NomNom.FileCreationHandlers.ActivityLog {
    #region Using Directives

    using System.IO;

    using Domain;
    using Domain.Models;

    #endregion

    public class FileCreationEventHandler : IFileCreationEventHandler {
        private readonly ILog log;

        public FileCreationEventHandler(ILog log) {
            this.log = log;
        }

        public void Handle(Monitor monitor, Stream fileCreated, string filePath) {
            const string messageFormat = "Monitor <{0}> has detected the creation of file <{1}>";
            this.log.Information(string.Format(messageFormat, monitor.Name, Path.GetFileName(filePath)));
        }
    }
}