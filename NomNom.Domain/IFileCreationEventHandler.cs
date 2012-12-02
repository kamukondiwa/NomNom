namespace NomNom.Domain {
    #region Using Directives

    using System.IO;

    using Models;

    #endregion

    public interface IFileCreationEventHandler {
        void Handle(Monitor monitor, Stream fileCreated, string fileName);
    }
}