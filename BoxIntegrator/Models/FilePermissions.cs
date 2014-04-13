﻿using Newtonsoft.Json;

namespace BoxIntegrator.Models
{
    public class FilePermissions
    {
        [JsonProperty(PropertyName = "can_download")]
        public bool AllowDownload { get; set; }
        [JsonProperty(PropertyName = "can_preview")]
        public bool AllowPreview { get; set; }
    }
}