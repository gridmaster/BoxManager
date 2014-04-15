using System.Collections.Generic;
using BoxIntegrator.Models;
using Newtonsoft.Json;

namespace BoxIntegrator.Response
{
    public class BaseResponseData
    {
        public string error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string errorDescription { get; set; }

        public string code { get; set; }
        public string state { get; set; }

        public string access_token { get; set; }
        public int expires_in { get; set; }
        public List<RestrictedTo> restricted_to { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }

    }
}
