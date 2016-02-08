using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using NotifyServer;

namespace SIT_NotifyService
{
    public partial class SITNservice : ServiceBase
    {
        private NotifyServerCore server;

        public SITNservice()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            server = new NotifyServerCore();
            server.Start();

        }

        protected override void OnStop()
        {
            server.Stop();
        }

        protected override void OnPause()
        {
            server.Suspend();
        }

        protected override void OnContinue()
        {
            server.Resume();
        }
    }
}
