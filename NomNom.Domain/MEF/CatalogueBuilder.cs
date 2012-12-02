namespace NomNom.Domain.MEF
{
    #region Using Directives

    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Reflection;

    #endregion

    public class CatalogueBuilder
    {
        private readonly IList<ComposablePartCatalog> catalogs = new List<ComposablePartCatalog>();

        public ComposablePartCatalog Build()
        {
            return new AggregateCatalog(this.catalogs);
        }

        public CatalogueBuilder ForAssembly(Assembly assembly)
        {
            this.catalogs.Add(new AssemblyCatalog(assembly));
            return this;
        }
    }
}