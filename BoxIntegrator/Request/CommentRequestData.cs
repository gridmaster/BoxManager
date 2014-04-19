
using BoxIntegrator.Models;

namespace BoxIntegrator.Request
{
    public class CommentRequestData : BaseRequestData
    {
        public Item item { get; set; }
        public string message { get; set; }
    }
}
