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

        public void SetRefreshToken(string refreshToken)
        {
            base.refreshToken = refreshToken;
        }

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

        // Delete a folder
        // DELETE /folders/{folder id}
        // Example: https://api.box.com/2.0/folders/FOLDER_ID?recursive=true
        // Returns: An empty 204 response will be returned upon successful deletion. 
        //  An error is thrown if the folder is not empty and the ‘recursive’ parameter is not included.
        public FolderResponseData DeleteFolder(string folderId)
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

                string url = String.Format(CoreConstants.UriDeleteFolders, folderId);

                jsonData = Delete(url, folderRequest);

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

        public Files UpdateFile(string fileId, string body)
        {
            string uri = String.Format(CoreConstants.UriGetFile, fileId);
            Files fileData = new Files();

            GetNewToken();

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add(CoreConstants.authorizationBearer + token);
                    client.Headers.Add("Content-Type", CoreConstants.applicationType);
                    string response = client.UploadString(uri, "PUT", body);

                    fileData = JsonConvert.DeserializeObject<Files>(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code updating a file. Error returned: {0} :: {1}",
                                  ex.Message, ex.InnerException));
            }

            return fileData;
        }

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

        #endregion Implement IBoxIntegrationManager

        #region Private Methods

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
                    if (((HttpWebResponse) response).StatusDescription == "No Content")
                    {
                        dataStream = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            jsonResponse = reader.ReadToEnd();
                        }
                    }
                }

                //HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jsonData;
        }


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
                    client.Headers.Add(CoreConstants.authorizationBearer + token);
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

    }
}
