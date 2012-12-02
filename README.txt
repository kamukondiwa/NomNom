A simple windows service that allows for monitoring and consumption of file creation events. The service can be configured to monitor multiple file system locations and provides a simple api for adding new event handlers.

Step 1: Create a monitor.

<code>
  &lt;ArrayOfMonitor xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot; xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot;&gt;
    &lt;Monitor&gt;
    &lt;Name&gt;
      Monitor 1
    &lt;/Name&gt;
    &lt;Source&gt;\\NetworkLocation\Source&lt;/Source&gt;
    &lt;Destination&gt;D:\local\Destination&lt;/Destination&gt;
  &lt;/Monitor&gt;
</code>

Step 2: Add a handler

implement the IFileCreationEventHandler interface.

    public interface IFileCreationEventHandler {
        void Handle(Monitor monitor, Stream fileCreated, string filePath);
    }

Example : 

    public class FileCreationEventHandler : IFileCreationEventHandler {
        private readonly ILog log;

        public FileCreationEventHandler(ILog log) {
            this.log = log;
        }

        public void Handle(Monitor monitor, Stream fileCreated, string filePath) {
            const string messageFormat = "Monitor <{0}> has detected the creation of file <{1}>";
            this.log.Information(string.Format(messageFormat, monitor.Name, Path.GetFileName(filePath)));
        }
    }