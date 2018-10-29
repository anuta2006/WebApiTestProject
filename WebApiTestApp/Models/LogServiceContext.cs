
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiTestApp.Models
{
    public class LogServiceContext : TableServiceContext
    {
        public LogServiceContext(string baseAddress, StorageCredentials credentials) : base(baseAddress, credentials) { }

        public  void Log(LogEntry logEntry)
        {
            AddObject("LogEntries", logEntry);
            SaveChanges();
        }

        public IQueryable<LogEntry> LogEntries
        {
            get
            {
                return CreateQuery<LogEntry>("LogEntries");
            }
        }
    }
}