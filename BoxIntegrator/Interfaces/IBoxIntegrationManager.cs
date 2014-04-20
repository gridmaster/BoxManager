using BoxIntegrator.Models;
using BoxIntegrator.Response;

namespace BoxIntegrator.Interfaces
{
    public interface IBoxIntegrationManager
    {
        void SetRefreshToken(string refreshToken);
        string PostAccessToken(string uri, string code);

        FolderResponseData CreateFolder(string name, string parentId);
        FolderResponseData GetFolder(string folderId);
        FolderResponseData UpdateFolder(string folderId, string body);
        string DeleteFolder(string folderId);
        FolderResponseData CopyFolder(string folderId, string parentId);
        FolderResponseData CreateFolderShare(string folderId, string folderShareType);
        FolderResponseData RestoreTrashedFolder(string name, string folderId);
        FolderCollaborationsResponseData GetFolderCollaborations(string folderId);
        string PermanentlyDeleteTrashedFolder(string folderId);
        GetTrashedItemsResponseData GetItemsFromTrash();
        
        FileResponseData GetFile(string fileId);
        Files UpdateFile(string fileId, string body);
        byte[] DownloadFile(string fileId);
        CommentResponseData AddCommentToItem(string fileId, string itemType, string message);

        Folder ListAllFiles(string folderId);
    }
}
