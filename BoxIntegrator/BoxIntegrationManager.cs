using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using BoxIntegrator.Core;
using BoxIntegrator.Interfaces;
using BoxIntegrator.Models;
using BoxIntegrator.Request;
using BoxIntegrator.Response;
using Newtonsoft.Json;

namespace BoxIntegrator
{
    public class BoxIntegrationManager : BaseBoxManager, IBoxIntegrationManager
    {
// ReSharper disable SuggestUseVarKeywordEvident

        #region Private Properties

        private string token { get; set; }
        
        #endregion Private Properties

        #region Constructors

        public BoxIntegrationManager(string clientId, string clientSecret)
            : base(clientId, clientSecret)
        {
        }

        #endregion Constructors

        #region Implement IBoxIntegrationManager

        #region Token and Access Methods
        public void SetRefreshToken(string refreshToken)
        {
            base.refreshToken = refreshToken;
        }

        // This grants the Token
        // Example: grant_type=authorization_code&code=YOUR_AUTH_CODE&client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET
        // Method: POST
        // Required: grant_type, client_id, 
        // Optional: code, refresh_token
        public string PostAccessToken(string uri, string code)
        {
            string responseString = string.Empty;

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["grant_type"] = CoreConstants.authorizationCode;
                values["code"] = code;
                values["client_id"] = base.clientId;
                values["client_secret"] = base.clientSecret;

                var response = client.UploadValues(uri, values);

                responseString = Encoding.Default.GetString(response);
            }
            return responseString;
        }

        // Revoke access
        // Example: 'client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET&token=YOUR_TOKEN' 
        // Method: POST
        // Required: client_id, client_secret, token
        // public const string UriRevoke = "https://www.box.com/api/oauth2/revoke"; 
        public void RevokeAccess()
        {
            throw new NotImplementedException("Revoke Access is not implemented");
            // TODO: implement
        }
        #endregion Token and Access Methods

        #region Folder Methods
        // Create a folder 
        // Example: POST /folders
        //      or: https://api.box.com/2.0/folders -H "Authorization: Bearer ACCESS_TOKEN" -d '{"name":"New Folder", "parent": {"id": "0"}}'
        // Required: name, parent, id
        // public const string UriBaseFolders = BaseUri + "/folders"; 
        // Returns: A full folder object
        public FolderResponseData CreateFolder(string name, string parentId)
        {
            var folderResponseData = new FolderResponseData();
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                CreateFolderRequestData folderRequest = new CreateFolderRequestData
                {
                    name = name,
                    parent = new Item
                    {
                        id = parentId
                    },
                    token = token
                };

                jsonData = Post(CoreConstants.UriBaseFolders, folderRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code creating a folder. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            folderResponseData = DeserializeJson<FolderResponseData>(jsonData);

            return folderResponseData;
        }

        // Get a folder
        // GET /folders/{folder id}
        // Returns a folder object
        public FolderResponseData GetFolder(string folderId)
        {
            var folderResponseData = new FolderResponseData();
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                FolderRequestData folderRequest = new FolderRequestData
                {
                    Id = Convert.ToInt64(folderId),
                    token = token
                };

                string url = String.Format(CoreConstants.UriGetFolders, folderId);

                jsonData = Get(url, folderRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code getting a file. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            folderResponseData = DeserializeJson<FolderResponseData>(jsonData);

            return folderResponseData;
        }

        // Update info about a folder
        // PUT /folders/{folder id}
        // Example: https://api.box.com/2.0/folders/FOLDER_ID -H "Authorization: Bearer ACCESS_TOKEN" -d '{"name":"New Folder Name!"}'
        // Request Body attributes: name, description, parent, id, shared_link, access, unshared_at, permissions
        //      permissions.can_download, permissions.can_preview, folder_upload_email, access, owned_by, id, sync_state, tags
        // public const string UriUpdateFolders = UriGetFolders;
        public FolderResponseData UpdateFolder(string folderId, string body)
        {
            string jsonData = string.Empty;
            FolderResponseData folderResponseData = new FolderResponseData();

            GetNewToken();

            Folder folder = DeserializeJson<Folder>(body);

            try
            {
                FolderUpdateData folderRequestData = DeserializeJson<FolderUpdateData>(body);

                folderRequestData.token = token;

                string url = String.Format(CoreConstants.UriUpdateFolders, folderId);

                jsonData = Put(url, folderRequestData);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code updating a file. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            return folderResponseData;
        }

        // Delete a folder
        // DELETE /folders/{folder id}
        // Example: https://api.box.com/2.0/folders/FOLDER_ID?recursive=true
        // Returns: An empty 204 response will be returned upon successful deletion. 
        //  An error is thrown if the folder is not empty and the ‘recursive’ parameter is not included.
        public string DeleteFolder(string folderId)
        {
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                FolderRequestData folderRequest = new FolderRequestData
                {
                    Id = Convert.ToInt64(folderId),
                    token = token
                };

                string url = String.Format(CoreConstants.UriDeleteFolders, folderId);

                jsonData = Delete(url, folderRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code deleting a folder. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            return "success";
        }

        // Copy a folder
        // Example: POST /folders/{folder id}/copy
        //      or: https://api.box.com/2.0/folders/FOLDER_ID/copy -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -d '{"parent": {"id" : DESTINATION_FOLDER_ID}}
        // Request Body attributes
        // Required: parent, id
        // Optional: name
        // public const string UriCopyFolder = UriGetFolders + "/copy";
        // Returns: A full folder object is returned
        public FolderResponseData CopyFolder(string folderId, string parentId)
        {
            var folderResponseData = new FolderResponseData();
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                CopyFolderRequestData folderRequest = new CopyFolderRequestData
                {
                    parent = new Item
                    {
                        id = parentId
                    },
                    token = token
                };
                string url = String.Format(CoreConstants.UriCopyFolder, folderId);

                jsonData = Post(url, folderRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code creating a folder. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            folderResponseData = DeserializeJson<FolderResponseData>(jsonData);

            return folderResponseData;
        }

        // Create a shared Folder link
        // Example: PUT /folders/{folder id}
        //      or: https://api.box.com/2.0/folders/FOLDER_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //              -d '{"shared_link": {"access": "open"}}' 
        // Request body attributes
        // shared_link, shared_link.access, shared_link.unshared_at, shared_link.permissions
        //  shared_link.permissions.can_download, shared_link.permissions.can_preview
        // Returns: A full folder object containing the updated shared link
        // TODO: add optional parameter[] to allow more fields to be updated
        public FolderResponseData CreateFolderShare(string folderId, string folderShareType)
        {
            string jsonData = string.Empty;

            FolderResponseData folderResponseData = new FolderResponseData();

            GetNewToken();

            try
            {
                FolderShareRequestData folderRequest = new FolderShareRequestData
                {
                    Body = string.Format(CoreConstants.sharedLink, folderShareType)
                };
                string uri = string.Format(CoreConstants.UriGetFolders, folderId);

                jsonData = Put(uri, folderRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code creating a folder share. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            folderResponseData = DeserializeJson<FolderResponseData>(jsonData);

            return folderResponseData;
        }

        // view a folders colaborators
        // Example: GET /folders/{id}/colaborations
        // public const string UriFoldersCollaborations = UriGetFolders + "/collaborations";
        // Returns: A collection of collaboration objects
        public FolderCollaborationsResponseData GetFolderCollaborations(string folderId)
        {
            FolderCollaborationsResponseData folderCollaborationsResponseData = new FolderCollaborationsResponseData();
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                FolderCollaborationsRequestData foldeCollaborationsRequest = new FolderCollaborationsRequestData
                {
                    Id = Convert.ToInt64(folderId),
                    token = token
                };

                string url = String.Format(CoreConstants.UriFoldersCollaborations, folderId);

                jsonData = Get(url, foldeCollaborationsRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code getting a folders collaborations. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            folderCollaborationsResponseData = DeserializeJson<FolderCollaborationsResponseData>(jsonData);

            return folderCollaborationsResponseData;
        }

        // Get files and/or folders that have been moved to the trash
        // Filters: Any attribute in the full files or folders objects can be passed in with the fields 
        //      parameter to get specific attributes, and only those specific attributes back; otherwise, 
        //      the mini format is returned for each item by default. Multiple attributes can be passed in 
        //      separated by commas e.g. fields=name,created_at. Paginated results can be retrieved using the limit and offset parameters.
        // Example: GET /folders/trash/items
        //      or: https://api.box.com/2.0/folders/trash/items?limit=2&offset=0
        // URL Parameters: fields, limit, offset
        // public const string UriGetFoldersFromTrash = UriBaseFolders + "/trash/items";
        // Returns: A collection of items contained in the trash
        // TODO: implement parameters[] for fields and limit=2&offset=0
        public GetTrashedItemsResponseData GetItemsFromTrash()
        {
            var getTrashedItemsResponseData = new GetTrashedItemsResponseData();
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                GetTrashedItemsRequestData getTrashedItemsRequestData = new GetTrashedItemsRequestData
                {
                    limit = 2,
                    offset = 0,
                    token = token
                };

                jsonData = Get(CoreConstants.UriGetFoldersFromTrash, getTrashedItemsRequestData);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code getting a file. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            getTrashedItemsResponseData = DeserializeJson<GetTrashedItemsResponseData>(jsonData);

            return getTrashedItemsResponseData;
        }

        // Get a trashed folder
        // Example: GET /folders/{folder id}/trash
        // Returns: The full item will be returned, including information about when the it was moved to the trash
        // public const string UriGetTrashedFolder = UriGetFolders + "/trash";
        public FolderResponseData GetTrashedFolder(string folderId)
        {
            var folderResponseData = new FolderResponseData();
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                FolderRequestData folderRequest = new FolderRequestData
                {
                    token = token
                };

                string url = String.Format(CoreConstants.UriGetTrashedFolder, folderId);

                jsonData = Get(url, folderRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code getting a trashed folder. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            folderResponseData = DeserializeJson<FolderResponseData>(jsonData);

            return folderResponseData;
        }
        
        // Permanently delete a trashed folder
        // Example: DELETE /folders/{folder id}/trash
        // public const string UriDeleteTrashedFolder = UriGetFolders + "/trash";
        // Returns: An empty 204 No Content response
        public string PermanentlyDeleteTrashedFolder(string folderId)
        {
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                FolderRequestData folderRequest = new FolderRequestData
                {
                    Id = Convert.ToInt64(folderId),
                    token = token
                };

                string url = String.Format(CoreConstants.UriDeleteTrashedFolder, folderId);

                jsonData = Delete(url, folderRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code deleting a trashed folder. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            return jsonData;
        }
        
        // Restore a trashed folder
        // Example: POST /folders/{folder id}
        //      or: https://api.box.com/2.0/folders/FOLDER_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -d '{"name":"non-conflicting-name"}'
        // Request Body Attributes:
        //      name, parent, parent.id
        // public const string UriRestoreTrashedFolder = UriBaseFolders + "/{id}";
        // Returns: The full item will be returned with a 201 Created status
        public FolderResponseData RestoreTrashedFolder(string name, string folderId)
        {
            var folderResponseData = new FolderResponseData();
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                RestoreFolderRequestData folderRequest = new RestoreFolderRequestData
                {
                    name = name,
                    token = token
                };

                string url = String.Format(CoreConstants.UriRestoreTrashedFolder, folderId);

                jsonData = Post(url, folderRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code creating a folder. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            folderResponseData = DeserializeJson<FolderResponseData>(jsonData);

            return folderResponseData;
        }

        #endregion Folder Methods

        #region File Methods

        // Get information about a file
        // Example: GET /files/{file id}
        // public const string UriGetFile = BaseUri + "/files/{0}";
        // Returns: A full file object
        public FileResponseData GetFile(string fileId)
        {
            var fileResponseData = new FileResponseData();
            string jsonData = string.Empty;

            GetNewToken();
            
            try
            {
                FileRequestData fileRequest = new FileRequestData
                    {
                        Id = Convert.ToInt64(fileId),
                        token = token
                    };

                string url = String.Format(CoreConstants.UriGetFile, fileId);

                jsonData = Get(url, fileRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code getting a file. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            fileResponseData = DeserializeJson<FileResponseData>(jsonData);

            return fileResponseData;
        }

        // Update a files information
        // Example: PUT /files/{file id}
        //      or: https://api.box.com/2.0/files/FILE_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -d '{"name":"new name.jpg"}
        // Headers: If-Match
        // Request Body Attributes:
        // name, description, parent, parent.id, shared_link, shared_link.access, shared_link.unshared_at,
        //       shared_link.permissions, shared_link.permissions.download, shared_link.permissions.preview, tags
        // public const string UriUpdateFile = UriGetFile;
        // Returns: A full file object is returned 
        public Files UpdateFile(string fileId, string body)
        {
            string jsonData = string.Empty;
            FileResponseData fileData = new FileResponseData();

            GetNewToken();

            try
            {
                FolderUpdateData folderRequestData = DeserializeJson<FolderUpdateData>(body);

                folderRequestData.token = token;

                string url = String.Format(CoreConstants.UriUpdateFile, fileId);

                jsonData = Put(url, folderRequestData);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code updating a file. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            fileData = JsonConvert.DeserializeObject<FileResponseData>(jsonData);

            return fileData;
        }

        // Download a file
        // URL Parameters: version
        // Example: GET /files/{file id}/content
        // Response is 302 if found, 202 if accepted but not available and a Ready-After header
        // public const string UriFilesContent = UriGetFile + "/content"; // POST
        // Returns: If the file is available to be downloaded, the response will be a 302 Found to a URL at dl.boxcloud.com. 
        //  The dl.boxcloud.com URL is not persistent. Clients will need to follow the redirect in order to actually download 
        //  the file. The raw data of the file is returned unless the file ID is invalid or the user does not have access to it.
        //  If the file is not ready to be downloaded (i.e. in the case where the file was uploaded immediately before the 
        //  download request), a response with an HTTP status of 202 Accepted will be returned with a Retry-After header indicating 
        //  the time in seconds after which the file will be available for the client to download.
        // Response: Raw text of a text file 
        public byte[] DownloadFile(string fileId)
        {
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                FileRequestData fileRequest = new FileRequestData
                {
                    Id = Convert.ToInt64(fileId),
                    token = token
                };

                string url = String.Format(CoreConstants.UriFilesContent, fileId);

                jsonData = Get(url, fileRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code getting a file. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            byte[] requestData = Encoding.UTF8.GetBytes(jsonData);

            return requestData;
        }

        // Upload a file
        // Example: POST https://upload.box.com/api/2.0/files/content
        //      or: https://upload.box.com/api/2.0/files/content -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -F filename=@FILE_NAME -F parent_id=PARENT_FOLDER_ID
        // Headers: Content-MD5 (The SHA1 hash of the file)
        // Form Elements:
        // Required: filename, parent_id
        // Optional: content_created_at, content_modified_at
        // Returns: A full file object is returned inside of a collection
        // public const string UriUploadFile = UriUpload + "/files/content"; // POST
        public FileUploadResponseData UploadFile(string fileName, string parentId)
        {
            FileUploadResponseData fileUploadResponseData = new FileUploadResponseData();

            return fileUploadResponseData;
        }


        // Delete a file
        // Headers: If-Match the etag of the file
        // Returns and empty 204 for successful delete, or 412 Precondition Failed if If-Match fales
        // Example: DELETE /files/{id}
        //      or: ttps://api.box.com/2.0/files/FILE_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -H "If-Match: a_unique_value"
        // public const string UriDeleteFile = UriGetFile;
        // Returns: a 204 if successful. 
        public string DeleteFile(string fileId)
        {
            string jsonData = string.Empty;

            GetNewToken();

            try
            {
                FileRequestData fileRequest = new FileRequestData
                {
                    Id = Convert.ToInt64(fileId),
                    token = token
                };

                string url = String.Format(CoreConstants.UriDeleteFile, fileId);

                jsonData = Delete(url, fileRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code deleting a file. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            return "success";
        }



        // Create a shared link using UriFiles
        // Example: PUT /files/{files id}
        //      or: https://api.box.com/2.0/files/FILE_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //              -d '{"shared_link": {"access": "open"}}' 
        // Request body attributes:
        // shared_link, shared_link.access, shared_link.unshared_at, shared_link.permissions
        // shared_link.permissions.can_download, shared_link.permissions.can_preview
        // Returns a full file object
        public FileResponseData CreateFileShare(string fileId, string fileShareType)
        {
            string body = string.Format(CoreConstants.sharedLink, fileShareType);
            string uri = string.Format(CoreConstants.UriGetFile, fileId);
            string jsonData = string.Empty;
            FileResponseData fileResponseData = new FileResponseData();

            GetNewToken();

            try
            {
                FileShareRequestData fileRequest = new FileShareRequestData
                    {
                        Body = string.Format(CoreConstants.sharedLink, fileShareType)
                    };

                jsonData = Put(uri, fileRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code creating file share. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            fileResponseData = DeserializeJson<FileResponseData>(jsonData);

            return fileResponseData;
        }
        
        // Add a comment to an item
        // Example: POST /comments
        //      or: https://api.box.com/2.0/comments -H "Authorization: Bearer ACCESS_TOKEN"
        //              -d '{"item": {"type": "file", "id": "FILE_ID"}, "message": "YOUR_MESSAGE"}'
        // Request Body Attributes
        // Required: item, item.type, item.id, message
        // Returns: The new comment object
        public CommentResponseData AddCommentToItem(string fileId, string itemType, string message)
        {
            GetNewToken();

            CommentRequestData commentRequest = new CommentRequestData
                {
                    item = new Item
                        {
                            id = fileId,
                            type = itemType
                        },
                    message = message,
                    token = token
                };
            string jsonData = string.Empty;

            try
            {
                string url = CoreConstants.UriAddCommentToItem;

                jsonData = Post(url, commentRequest);

            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code adding comment to item. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            CommentResponseData commentResponse = DeserializeJson<CommentResponseData>(jsonData);

            return commentResponse;
        }
        #endregion File Methods

        #region Expanded Methods
        public Folder ListAllFiles(string folderId)
        {
            string result = String.Empty;
            Folder folders = new Folder();

            GetNewToken();

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("Authorization: Bearer " + token);
                    result = client.DownloadString(string.Format(CoreConstants.UriGetFolders, 0));
                    folders = JsonConvert.DeserializeObject<Folder>(result);
                }

                folders.Folders = new List<Folder>();
                folders.Files = new List<Files>();
                folders = BuildTree(folders, result, token);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        "Error occured in Box Intigration code listing all files. Error returned: {0} :: {1}",
                        ex.Message, ex.InnerException));
            }
            return folders;
        }
        #endregion Expanded Methods

        #endregion Implement IBoxIntegrationManager

        #region Private Methods

        private string Post<T>(string uri, T postData) where T : BaseRequestData
        {
            string jsonData = string.Empty;
            try
            {
                jsonData = JsonConvert.SerializeObject(postData);
                byte[] requestData = Encoding.UTF8.GetBytes(jsonData);

                WebRequest request = WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = CoreConstants.contentType;
                request.Headers.Add(CoreConstants.authorizationBearer + postData.token);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(requestData, 0, requestData.Length);
                dataStream.Dispose();

                string jsonResponse = string.Empty;
                using (WebResponse response = request.GetResponse())
                {
                    if (((HttpWebResponse)response).StatusDescription == "Created")
                    {
                        dataStream = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            jsonResponse = reader.ReadToEnd();
                        }
                    }
                }

                //logger.DebugFormat(
                //    "postAsync{0}URI: {1}{0}Post Data: {2}{0}Content Type: {3}{0}Response: {4}",
                //    Environment.NewLine,
                //    uri,
                //    jsonData,
                //    CoreConstants.CONTENT_TYPE,
                //    jsonResponse
                //    );

                BasicResponseData responseCode = JsonConvert.DeserializeObject<BasicResponseData>(jsonResponse);
                if (responseCode.error > 0)
                {
                    throw new Exception(string.Format("Error CodeString {0} Returned from Web Service call.",
                                                      responseCode.error));
                }

                return jsonResponse;
            }
            catch (Exception ex)
            {
                string error = string.Format(
                    "Exception in postAsync{0}URI: {1}{0}Post Data: {2}{0}Content Type: {3}{0}",
                    Environment.NewLine,
                    uri,
                    jsonData,
                    CoreConstants.contentType);

                throw new Exception(error, ex);
            }
        }

        private string Put<T>(string uri, T putData) where T : BaseRequestData
        {
            string jsonData = string.Empty;
            string body = JsonConvert.SerializeObject(putData);

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add(CoreConstants.authorizationBearer + token);
                    client.Headers.Add("Content-Type", CoreConstants.applicationType);
                    jsonData = client.UploadString(uri, "PUT", body);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code on PUT. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));

            }
            return jsonData;
        }

        private string Get<T>(string uri, T getData) where T : BaseRequestData
        {
            string jsonData = string.Empty;
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add(CoreConstants.authorizationBearer + getData.token);
                    jsonData = client.DownloadString(uri);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code on GET. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));

            }
            return jsonData;
        }

        private string Delete<T>(string uri, T deleteData) where T : BaseRequestData
        {
            string jsonData = string.Empty;
            try
            {
                jsonData = JsonConvert.SerializeObject(deleteData);
                byte[] requestData = Encoding.UTF8.GetBytes(jsonData);

                WebRequest request = WebRequest.Create(uri);
                request.Method = "DELETE";

                request.Headers.Add(CoreConstants.authorizationBearer + deleteData.token);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(requestData, 0, requestData.Length);
                dataStream.Dispose();

                string jsonResponse = string.Empty;
                using (WebResponse response = request.GetResponse())
                {
                    if (((HttpWebResponse)response).StatusDescription == "No Content")
                    {
                        dataStream = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            jsonResponse = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "success";
        }

        private string Post_it<T>(string uri, T postData) where T : BaseRequestData
        {
            string jsonData = string.Empty;
            try
            {
                using( WebClient client = new WebClient())
                {
                    client.Headers.Add(CoreConstants.authorizationBearer + postData.token);
                    client.Headers.Add("Content-Type", CoreConstants.applicationType);
                   // jsonData = client.UploadData(uri, "POST", "");
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = CoreConstants.applicationType;
                request.Headers.Add(CoreConstants.authorizationBearer + postData.token);
                using (WebResponse response = request.GetResponse())
                {
                    var reader = new StreamReader(response.GetResponseStream());

                    jsonData = reader.ReadToEnd();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                        string.Format("Error occured in Box Intigration code on POST. Error returned: {0} :: {1}", ex.Message, ex.InnerException));
            }

            return jsonData;
        }

        private string OldPost<T>(string uri, T postData) where T : BaseRequestData
        {
            string jsonData = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = CoreConstants.applicationType;
                request.Headers.Add(CoreConstants.authorizationBearer + postData.token);
                using (WebResponse response = request.GetResponse())
                {
                    var reader = new StreamReader(response.GetResponseStream());

                    jsonData = reader.ReadToEnd();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                        string.Format("Error occured in Box Intigration code on POST. Error returned: {0} :: {1}", ex.Message, ex.InnerException));
            }

            return jsonData;
        }

        private void GetNewToken()
        {
            try
            {
                string newToken = PostRefreshToken(CoreConstants.UriTokenString);
                Token tokenObj = JsonConvert.DeserializeObject<Token>(newToken);

                refreshToken = tokenObj.refresh_token;
                token = tokenObj.access_token;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration getting a token. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }
        }

        private Folder BuildTree(Folder folders, string result, string accessToken)
        {
            for (int i = 0; i < folders.item_collection.entries.Count; i++)
            {
                var item = folders.item_collection.entries[i];

                if (item.type == "file")
                {
                    try
                    {
                        using (var client = new WebClient())
                        {
                            Files file = new Files();
                            client.Headers.Add(CoreConstants.authorizationBearer + accessToken);
                            string fileresult = client.DownloadString(String.Format(CoreConstants.UriGetFile, item.id));
                            file = JsonConvert.DeserializeObject<Files>(fileresult);
                            if (folders.Files == null)
                                folders.Files = new List<Files>();
                            folders.Files.Add(file);
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            string.Format("Error occured in Box Intigration getting files. Error returned: {0} :: {1}",
                                          ex.Message, ex.InnerException));
                    }
                }

                if (item.type == "folder")
                {
                    try
                    {
                        using (var client = new WebClient())
                        {
                            Folder fold = new Folder();
                            client.Headers.Add(CoreConstants.authorizationBearer + accessToken);
                            string folderresult =
                                client.DownloadString(String.Format(CoreConstants.UriGetFolders, item.id));
                            fold = JsonConvert.DeserializeObject<Folder>(folderresult);
                            if (folders.Folders == null)
                                folders.Folders = new List<Folder>();
                            folders.Folders.Add(fold);
                            // use recursion to build out the full structure
                            // BuildTree(fold, result, access_token);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            string.Format("Error occured in Box Intigration getting folders. Error returned: {0} :: {1}",
                                          ex.Message, ex.InnerException));
                    }
                }
            }

            return folders;
        }

        private T DeserializeJson<T>(string jsonData)
        {
            T deserializedJson = default(T);

            try
            {
                if (jsonData.Length > 3) //Json returns "{}" for blank data
                {
                    JsonSerializerSettings jsonSettings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                    deserializedJson = JsonConvert.DeserializeObject<T>(jsonData, jsonSettings);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        "Error occured in Box Intigration code deserializing json. Error returned: {0} :: {1}",
                        ex.Message, ex.InnerException));
            }

            return deserializedJson;
        }

        #endregion Private Methods
// ReSharper restore SuggestUseVarKeywordEvident
    }
}
