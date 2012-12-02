namespace NomNom.Contracts {
    #region Using Directives

    using System;
    using System.IO;
    using System.Linq.Expressions;

    using Domain.Models;

    #endregion

    public interface IStartMonitor {
        void Start(Monitor monitor, Expression<Action<Monitor, FileStream>> fileCreationHandler);
    }
}