using BoxIntegrator.Models;

namespace BoxIntegrator.Response
{
    public class FolderCollaborationsResponseData : BaseResponseData 
    {
        public int total_count { get; set; }
        public Entry entry { get; set; }
    }
}
