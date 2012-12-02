A simple windows service that allows for monitoring and consumption of file creation events. The service can be configured to monitor multiple file system locations and provides a simple api for adding new event handlers.

Step 1: Create a monitor.

<ArrayOfMonitor xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Monitor>
    <Name>
      Monitor 1
    </Name>
    <Source>\\NetworkLocation\Source</Source>
    <Destination>D:\local\Destination</Destination>
  </Monitor>
</ArrayOfMonitor>

Step 2: Add a handler

implement the IFileCreationEventHandler interface.

    public interface IFileCreationEventHandler {
        void Handle(Monitor monitor, Stream fileCreated, string fileName);
    }

Example : 

    public class FileCreationEventHandler : IFileCreationEventHandler {
        private readonly ILog log;

        public FileCreationEventHandler(ILog log) {
            this.log = log;
        }

        public void Handle(Monitor monitor, Stream fileCreated, string fileName) {
            const string messageFormat = "Monitor <{0}> has detected the creation of file <{1}>";
            this.log.Information(string.Format(messageFormat, monitor.Name, Path.GetFileName(fileName)));
        }
    }