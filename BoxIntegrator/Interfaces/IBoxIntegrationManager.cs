using BoxIntegrator.Models;
using BoxIntegrator.Response;

namespace BoxIntegrator.Interfaces
{
    public interface IBoxIntegrationManager
    {
        Folder ListAllFiles(string folderId);
        FolderResponseData GetFolder(string folderId);
        FileResponseData GetFile(string fileId);
        Files UpdateFile(string fileId, string body);
    }
}
