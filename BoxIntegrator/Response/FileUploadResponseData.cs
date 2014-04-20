using System.Collections.Generic;
using BoxIntegrator.Models;

namespace BoxIntegrator.Response
{
    public class FileUploadResponseData : BaseResponseData
    {
        public int total_count { get; set; }
        public IList<Files> entries { get; set; }
    }
}
