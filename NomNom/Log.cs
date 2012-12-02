namespace NomNom {
    #region Using Directives

    using System.Diagnostics;

    using Domain;

    #endregion

    public class Log : ILog {
        public void Error(string message) {
            EventLog.WriteEntry("NomNom", message, EventLogEntryType.Error);
        }

        public void Information(string message) {
            EventLog.WriteEntry("NomNom", message, EventLogEntryType.Information);
        }

        public void Warn(string message) {
            EventLog.WriteEntry("NomNom", message, EventLogEntryType.Warning);
        }
    }
}