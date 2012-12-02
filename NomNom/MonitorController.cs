namespace NomNom {
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Contracts;

    using Domain;

    using Monitor = Domain.Models.Monitor;

    #endregion

    public class MonitorController : IMonitorController {
        private readonly IEnumerable<IFileCreationEventHandler> fileCreationEventHandlers;

        private readonly ILog log;

        private readonly IStartMonitor startMonitor;

        public MonitorController(
            ILog log, IStartMonitor startMonitor, IEnumerable<IFileCreationEventHandler> fileCreationEventHandlers) {
            this.log = log;
            this.fileCreationEventHandlers = fileCreationEventHandlers;
            this.startMonitor = startMonitor;
        }

        public void BeginMonitoring(IEnumerable<Monitor> monitors) {
            foreach (var monitor in monitors) {
                this.log.Information(string.Format("Monitor <{0}> Starting", monitor.Name));

                this.startMonitor.Start(monitor, (x, y) => this.FileCreationHandler(monitor, y));

                this.log.Information(string.Format("Monitor <{0}> Started", monitor.Name));
            }
        }

        private void FileCreatedEventHandlerDelegate(object obj) {
            var parameterArray = obj as object[];
            if (parameterArray != null) {
                var fileCreationEventHandler = parameterArray[0] as IFileCreationEventHandler;
                var monitor = parameterArray[1] as Monitor;
                var fileCreated = parameterArray[2] as Stream;
                var fileName = parameterArray[3] as string;

                try {
                    using (fileCreated) {
                        if (fileCreationEventHandler != null) {
                            fileCreationEventHandler.Handle(monitor, fileCreated, fileName);
                        }
                    }
                }
                catch (Exception exception) {
                    this.log.Error(exception.Message + " ==> " + exception.StackTrace);
                }
            }
        }

        private void FileCreationHandler(Monitor monitor, FileStream fileCreated) {
            if (this.fileCreationEventHandlers != null && this.fileCreationEventHandlers.Count() > 0) {
                using (fileCreated) {
                    foreach (var fileCreationEventHandler in this.fileCreationEventHandlers) {
                        var filename = fileCreated.Name;
                        var memoryStream = new MemoryStream();
                        fileCreated.CopyTo(memoryStream);
                        this.SpawnFileCreationHandlerThread(fileCreationEventHandler, monitor, memoryStream, filename);
                    }
                }
            }
        }

        private void SpawnFileCreationHandlerThread(IFileCreationEventHandler fileCreationEventHandler, Monitor monitor, Stream fileCreated, string fileName) {
            var parameterizedThreadStart = new ParameterizedThreadStart(this.FileCreatedEventHandlerDelegate);
            var thread = new Thread(parameterizedThreadStart);
            thread.Start(new object[] { fileCreationEventHandler, monitor, fileCreated, fileName });
        }
    }
}