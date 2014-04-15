using Newtonsoft.Json;

namespace BoxIntegrator.Request
{
    public class restrictedObject
    {
        [JsonProperty("type")]
        public string objectType { get; set; }

        public string id { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string name { get; set; }
    }
}
