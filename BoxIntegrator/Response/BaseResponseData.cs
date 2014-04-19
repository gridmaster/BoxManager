using System.Collections.Generic;
using BoxIntegrator.Models;
using Newtonsoft.Json;

namespace BoxIntegrator.Response
{
    public class BaseResponseData
    {
        public int error { get; set; }

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
/*
{
    "type":"error",
    "status":403,
    "code":"storage_limit_exceeded",
    "help_url":"",
    "message":"Account storage limit reached",
    "request_id":"7522414694f97d171b6aea"
}
*/