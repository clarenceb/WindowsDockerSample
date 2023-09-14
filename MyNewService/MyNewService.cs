using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MyNewService
{
    public partial class MyNewService : ServiceBase
    {
        private readonly FileSystemWatcher watcher = new FileSystemWatcher(@"C:\uploads")
        {
            NotifyFilter = NotifyFilters.Attributes
                     | NotifyFilters.CreationTime
                     | NotifyFilters.DirectoryName
                     | NotifyFilters.FileName
                     | NotifyFilters.LastAccess
                     | NotifyFilters.LastWrite
                     | NotifyFilters.Security
                     | NotifyFilters.Size
        };

        private int eventId = 1;

        public MyNewService(string[] args)
        {
            InitializeComponent();

            if (!Environment.UserInteractive)
            {
                eventLogger = new System.Diagnostics.EventLog();
                if (!System.Diagnostics.EventLog.SourceExists("DemoLogSource"))
                {
                    System.Diagnostics.EventLog.CreateEventSource(
                        "DemoLogSource", "DemoServiceLog");
                }
                eventLogger.Source = "DemoLogSource";
                eventLogger.Log = "DemoServiceLog";
            }

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "*.json";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
        }

        protected override void OnStart(string[] args)
        {
            WriteLog("OnStart - starting service");

            // Set up a timer that triggers every 30s.
            Timer timer = new Timer();
            timer.Interval = 30000; // 30 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            WriteLog("OnStop - stopping service");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            WriteLog($"Monitoring the System - {eventId++}");
        }

        private void WriteLog(string message)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine($"{nameof(MyNewService)} - {message}");
            }
            else
            {
                eventLogger.WriteEntry($"{nameof(MyNewService)} - {message}", EventLogEntryType.Information);
            }
        }

        // This method helps in validating service as console app when run directly.
        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            WriteLog($"Changed: {e.FullPath}");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            WriteLog(value);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e) =>
            WriteLog($"Deleted: {e.FullPath}");

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            StringBuilder builder = new StringBuilder($"Renamed:");
            builder.AppendLine($"    Old: {e.OldFullPath}");
            builder.AppendLine($"    New: {e.FullPath}");
            WriteLog(builder.ToString());
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                StringBuilder builder = new StringBuilder($"Message: {ex.Message}");
                builder.AppendLine("Stacktrace:");
                builder.AppendLine(ex.StackTrace);
                WriteLog(builder.ToString());
                PrintException(ex.InnerException);
            }
        }
    }
}
