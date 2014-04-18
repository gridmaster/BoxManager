using System;
using System.Collections.Generic;
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

        public BoxIntegrationManager(string clientId, string clientSecret, string refreshToken)
            : base(clientId, clientSecret, refreshToken)
        {
        }

        #endregion Constructors

        #region Implement IBoxIntegrationManager

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
                    result = client.DownloadString(string.Format(CoreConstants.UriFolders, 0));
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

                string url = String.Format(CoreConstants.UriFolders, folderId);

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

                string url = String.Format(CoreConstants.UriFiles, fileId);

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
            string uri = String.Format(CoreConstants.UriFiles, fileId);
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
            string uri = string.Format(CoreConstants.UriFiles, fileId);
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

        #endregion Implement IBoxIntegrationManager

        #region Private Methods

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

        private string Post<T>(string uri, T postData) where T : BaseRequestData
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
                            string fileresult = client.DownloadString(String.Format(CoreConstants.UriFiles, item.id));
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
                                client.DownloadString(String.Format(CoreConstants.UriFolders, item.id));
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
