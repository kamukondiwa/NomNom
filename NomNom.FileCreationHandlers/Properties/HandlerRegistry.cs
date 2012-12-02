namespace NomNom.FileCreationHandlers.Properties {
    #region Using Directives

    using StructureMap.Configuration.DSL;

    #endregion

    public class HandlerRegistry : Registry {
        public HandlerRegistry() {
            this.Scan(x => x.WithDefaultConventions());
        }
    }
}