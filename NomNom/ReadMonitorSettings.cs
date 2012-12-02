namespace NomNom {
    #region Using Directives

    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Xml.Serialization;

    using Contracts;

    using Domain.Models;

    #endregion

    public class ReadMonitorSettings : IReadMonitorSettings {
        public IEnumerable<Monitor> Read() {
            IEnumerable<Monitor> monitors;

            var monitorPath = ConfigurationManager.AppSettings["monitorFilePath"];

            var xmlReader = new StreamReader(monitorPath);

            var xmlSerializer = new XmlSerializer(typeof(List<Monitor>));

            using (xmlReader) {
                monitors = xmlSerializer.Deserialize(xmlReader) as IEnumerable<Monitor>;
            }

            return monitors;
        }
    }
}