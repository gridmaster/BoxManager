using System.ServiceModel;
using System.ServiceModel.Web;

namespace BoxManager
{
    [ServiceContract]
    public interface IBoxService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/GetToken/?")] // code={code}&state={state}&error={error}")]
        string GetToken(); // string code, string state);

    }
}
