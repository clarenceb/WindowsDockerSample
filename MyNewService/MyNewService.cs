using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MyNewService
{
    public partial class MyNewService : ServiceBase
    {
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
        }

        protected override void OnStart(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine($"OnStart - starting serice {nameof(MyNewService)}");
            }
            else
            {
                eventLogger.WriteEntry($"OnStart - starting serice {nameof(MyNewService)}");
            }

            // Set up a timer that triggers every minute.
            Timer timer = new Timer();
            timer.Interval = 30000; // 30 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine($"OnStop - stopping serice {nameof(MyNewService)}");
            }
            else
            {
                eventLogger.WriteEntry($"OnStop - stopping serice {nameof(MyNewService)}");
            }
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            if (Environment.UserInteractive)
            {
                Console.WriteLine($"{nameof(MyNewService)} - Monitoring the System - {eventId++}");
            }
            else
            {
                eventLogger.WriteEntry($"{nameof(MyNewService)} - Monitoring the System", EventLogEntryType.Information, eventId++);
            }
        }

        // This method helps in validating service as console app when run directly.
        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
    }
}
