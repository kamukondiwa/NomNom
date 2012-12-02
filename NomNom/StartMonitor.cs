namespace NomNom {
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading;

    using Contracts;

    using Domain;

    using Monitor = Domain.Models.Monitor;

    #endregion

    public class StartMonitor : IStartMonitor {
        private readonly ILog log;

        private readonly int maxRetry;

        private readonly Dictionary<string, int> retryCount;

        private readonly int retryInterval;

        private Expression<Action<Monitor, FileStream>> innerFileCreationEventHandler;

        private Monitor innerMonitor;

        public StartMonitor(ILog log) {
            this.log = log;
            this.maxRetry = Int32.Parse(ConfigurationManager.AppSettings["maxRetry"]);
            this.retryInterval = Int32.Parse(ConfigurationManager.AppSettings["retryIntervalMilliseconds"]);
            this.retryCount = new Dictionary<string, int>();
        }

        public void Start(Monitor monitor, Expression<Action<Monitor, FileStream>> fileCreationHandler) {
            this.innerMonitor = monitor;
            this.innerFileCreationEventHandler = fileCreationHandler;
            this.SpawnMonitorThread(monitor);
        }

        private void AttatchFileCreationEventListener(object obj) {
            var monitor = obj as Monitor;
            if (monitor != null) {
                this.ListenForFileCreationEvents(monitor);
            }
        }

        private void ListenForFileCreationEvents(Monitor monitor) {
            var fileSystemWatcher = new FileSystemWatcher(monitor.Source) { EnableRaisingEvents = true };
            fileSystemWatcher.Created += this.HandleFileCreationEvents;
        }

        private void SpawnMonitorThread(Monitor monitor) {
            var monitorThreadStart = new ParameterizedThreadStart(this.AttatchFileCreationEventListener);
            var monitorThread = new Thread(monitorThreadStart);
            monitorThread.Start(monitor);
        }

        private void TryHandlingFileCreationEvent(object obj) {
            var e = obj as FileSystemEventArgs;
            try {
                using (var fileStream = File.Open(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    var action = this.innerFileCreationEventHandler.Compile();
                    action.Invoke(this.innerMonitor, fileStream);
                }
            }
            catch (IOException exception) {
                if (exception.Message.StartsWith("The process cannot access the file")) {
                    if (!this.retryCount.ContainsKey(e.FullPath)) {
                        this.retryCount[e.FullPath] = 0;
                    }

                    if (this.retryCount[e.FullPath] < this.maxRetry) {
                        this.log.Warn(
                            string.Format(
                                "failed to process {0} i'm going to try again {1} times with a {2} milliseconds interval.",
                                e.FullPath,
                                this.maxRetry - this.retryCount[e.FullPath],
                                this.retryInterval));
                        new Timer(this.TryHandlingFileCreationEvent, e, this.retryInterval, -1);
                        this.retryCount[e.FullPath]++;
                    }
                    else {
                        this.log.Error(
                            string.Format(
                                "Could not process {0} after {1} tries. I'm going to stop trying.",
                                e.FullPath,
                                this.maxRetry));
                        this.retryCount.Remove(e.FullPath);
                    }
                }
            }
        }

        private void HandleFileCreationEvents(object sender, FileSystemEventArgs e) {
            if (e.ChangeType == WatcherChangeTypes.Created) {
                this.TryHandlingFileCreationEvent(e);
            }
        }
    }
}