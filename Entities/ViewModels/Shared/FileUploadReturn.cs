using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class FileUploadReturn
    {
        public string name { get; set; }
        public long size { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteUrl { get; set; }
        public string delete_url { get; set; }
        public string deleteType { get; set; }
        public string delete_type { get; set; }

        public string error { get; set; }

    }
}
