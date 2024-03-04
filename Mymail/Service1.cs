using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Mymail
{
    public partial class Service1 : ServiceBase
    {
        commoncls obj = new commoncls();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            obj.Logging("Service Started", "T_");
            commoncls.ServiceStatus = true;
            obj.checkmail();
        }

        protected override void OnStop()
        {
            obj.Logging("Service Stoped", "T_");
            commoncls.ServiceStatus = false;
        }
    }
}
