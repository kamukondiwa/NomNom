namespace NomNom.Contracts {
    #region Using Directives

    using System.Collections.Generic;

    using Domain.Models;

    #endregion

    public interface IMonitorController {
        void BeginMonitoring(IEnumerable<Monitor> monitors);
    }
}