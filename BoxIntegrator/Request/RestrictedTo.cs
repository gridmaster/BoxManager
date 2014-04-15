using BoxIntegrator.Models;
using Newtonsoft.Json;

namespace BoxIntegrator.Request
{
    public class RestrictedTo
    {
        public string scope { get; set; }

        [JsonProperty("object")]
        public restrictedObject restrictedToObject { get; set; }
    }
}
