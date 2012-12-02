namespace NomNom.Contracts {
    #region Using Directives

    using System.Collections.Generic;

    using Domain.Models;

    #endregion

    public interface IReadMonitorSettings {
        IEnumerable<Monitor> Read();
    }
}