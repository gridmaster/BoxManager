using System.Collections.Generic;

namespace BoxIntegrator.Request
{
    class GetTrashedItemsRequestData : BaseRequestData
    {
        public IDictionary<string, string> fields { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
    }
}
