using System;

namespace BoxIntegrator.Request
{
    public class FileUpdloadRequestData : BaseRequestData
    {
        public long parentId { get; set; }
        public string fileName { get; set; }
        public DateTime content_created_at { get; set; }
        public DateTime content_modified_at { get; set; }
    }
}
