
using System;
using System.Collections.Generic;
using BoxIntegrator.Models;

namespace BoxIntegrator.Request
{
    public class FolderUpdateData : BaseRequestData
    {
        public string type { get; set; }
        public string id { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public PathCollection path_collection { get; set; }
        public Entry created_by { get; set; }
        public Entry modified_by { get; set; }
        public DateTime? trashed_at { get; set; }
        public DateTime? purged_at { get; set; }
        public DateTime? content_created_at { get; set; }
        public DateTime? content_modified_at { get; set; }
        public Entry owned_by { get; set; }
        public string sared_link { get; set; }
        public string folder_upload_email { get; set; }
        public Entry parent { get; set; }
        public string item_status { get; set; }
        public ItemCollection item_collection { get; set; }
        public IList<Folder> Folders { get; set; }
        public IList<Files> Files { get; set; }
    }
}
