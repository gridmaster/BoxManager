using Newtonsoft.Json;

namespace BoxManager.Models
{
    public class FileInfo
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "sequence_id")]
        public string SequenceId { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
        [JsonProperty(PropertyName = "path_id")]
        public string PathId { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty(PropertyName = "modified_at")]
        public string ModifiedAt { get; set; }
        [JsonProperty(PropertyName = "shared_link")]
        public ShareInfo ShareDetails { get; set; }
        [JsonProperty(PropertyName = "etag")]
        public string ETag { get; set; }
    }
}