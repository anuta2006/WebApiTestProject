using System;

namespace WebApiTestApp.Models
{
    public class ShowModel
    {
        public string Uri { get; set; }
        public string FileName { get; set; }
        public DateTime DateTimeAdded { get; set; }
        public int PollingGap { get; set; }
    }
}