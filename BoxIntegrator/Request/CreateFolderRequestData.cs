using BoxIntegrator.Models;

namespace BoxIntegrator.Request
{
    public class CreateFolderRequestData  : BaseRequestData
    {
        public string name { get; set; }
        public Item parent { get; set; }
    }

    //public class Parent
    //{
    //    public string id { get; set; }
    //}
}
