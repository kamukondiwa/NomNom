namespace NomNom.Domain.MEF
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    #endregion

    public class PluginInitialiser
    {
        public static void Initialise()
        {
            var catalog = new CatalogueBuilder()
                .ForAssembly(typeof(IHavePlugins).Assembly)
                .Build();

            var compositionContainer = new CompositionContainer(catalog);
            compositionContainer.ComposeParts();

            var test = compositionContainer.GetExports<IFileCreationEventHandler>();

        }
    }
}