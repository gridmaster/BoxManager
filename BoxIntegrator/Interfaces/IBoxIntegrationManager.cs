using BoxIntegrator.Models;
using BoxIntegrator.Response;

namespace BoxIntegrator.Interfaces
{
    public interface IBoxIntegrationManager
    {
        void SetRefreshToken(string refreshToken);
        string PostAccessToken(string uri, string code);
        Folder ListAllFiles(string folderId);
        FolderResponseData GetFolder(string folderId);
        FolderResponseData RestoreTrashedFolder(string name, string folderId);
        FileResponseData GetFile(string fileId);
        Files UpdateFile(string fileId, string body);
        CommentResponseData AddCommentToItem(string fileId, string itemType, string message);
    }
}
