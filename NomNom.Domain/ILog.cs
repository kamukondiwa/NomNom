namespace NomNom.Domain {
    public interface ILog {
        void Information(string message);

        void Error(string message);

        void Warn(string message);
    }
}