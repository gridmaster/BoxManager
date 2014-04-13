using Newtonsoft.Json;

namespace BoxManager.Models
{
    public class ShareInfo
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "download_url")]
        public string DownloadUrl { get; set; }
        [JsonProperty(PropertyName = "download_count")]
        public int DownloadCount { get; set; }
        [JsonProperty(PropertyName = "preview_count")]
        public int PreviewCount { get; set; }
        [JsonProperty(PropertyName = "access")]
        public string AccessLevel { get; set; }
        [JsonProperty(PropertyName = "permissions")]
        public FilePermissions Permissions { get; set; }
    }
}