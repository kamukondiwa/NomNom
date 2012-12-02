namespace NomNom {
    #region Using Directives

    using StructureMap;

    #endregion

    public class StructureMapInitialiser {
        public static void Initialiser() {
            ObjectFactory.Initialize(
                i => i.Scan(
                    x => {
                        x.AssembliesFromApplicationBaseDirectory();
                        x.WithDefaultConventions();
                        x.LookForRegistries();
                    }));
        }
    }
}