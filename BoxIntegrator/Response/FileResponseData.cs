
using System;
using BoxIntegrator.Models;

namespace BoxIntegrator.Response
{
    public class FileResponseData : BaseResponseData
    {
        public string type { get; set; }
        public string id { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string sha1 { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public long size { get; set; }
        public PathCollection path_collection { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? modified_at { get; set; }
        public DateTime? trashed_at { get; set; }
        public DateTime? purged_at { get; set; }
        public DateTime? content_created_at { get; set; }
        public DateTime? content_modified_at { get; set; }
        public Entry created_by { get; set; }
        public Entry modified_by { get; set; }
        public Entry owned_by { get; set; }
        public ShareInfo shared_link { get; set; }
        public Entry parent { get; set; }
        public string item_status { get; set; }
    }
}
