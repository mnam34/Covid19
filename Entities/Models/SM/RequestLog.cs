using System;
using System.ComponentModel.DataAnnotations;
namespace Entities
{
    public class RequestLog
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreateDate { get; set; }
        public string IpAddress { get; set; }
        public string RemoteIpAddress { get; set; }
        public string FullComputerName { get; set; }
        public string ComputerName { get; set; }
        public string BrowserInfo { get; set; }
        public string RawUrl { get; set; }
        public string AbsoluteUri { get; set; }

        //public string Describe()
        //{
        //    return "Request Logging";
        //}
    }
}
