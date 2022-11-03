using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace GES.Clients.Web.Infrastructure
{
    public static class LoggerHelper
    {
        public static ILogger CreateJobLogger()
        {
            var logPath = HttpContext.Current != null && HttpContext.Current.Server != null 
                    ? HttpContext.Current.Server.MapPath("logs/jobs") 
                    : Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"logs\jobs");

            var log = Path.Combine(logPath, @"job-log-{Date}.txt");

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            return new LoggerConfiguration()
                            .WriteTo.RollingFile(log, shared: true)
                            .CreateLogger();
        }
    }
}