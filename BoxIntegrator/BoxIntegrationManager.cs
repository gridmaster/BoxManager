﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using BoxIntegrator.Interfaces;
using BoxIntegrator.Models;
using Newtonsoft.Json;
using FileInfo = BoxIntegrator.Models.FileInfo;

namespace BoxIntegrator
{
    public class BoxIntegrationManager : BaseBoxManager, IBoxIntegrationManager
    {
        #region Private Properties
        private string token { get; set; }
        // this is a new GUID selected during development
        // private const string securety_token = "d66d5824-ae41-4249-902e-c32ad5c5b244";
        private const string UriTokenString = "https://www.box.com/api/oauth2/token";
        private const string UriAPI = "https://www.box.com/api/oauth2/";
        private const string UriFolders = "https://api.box.com/2.0/folders/{0}";
        private const string UriFiles = "https://api.box.com/2.0/files/{0}";

        #endregion Private Properties

        #region Constructors
        public BoxIntegrationManager(string clientId, string clientSecret, string refreshToken) : base(clientId, clientSecret, refreshToken)
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
                    result = client.DownloadString(string.Format(UriFolders, 0));
                    folders = JsonConvert.DeserializeObject<Folder>(result);
                }

                folders.Folders = new List<Folder>();
                folders.Files = new List<Files>();
                folders = BuildTree(folders, result, token);
            }
            catch (Exception ex)
            {
                throw new Exception(
                        string.Format("Error occured in Box Intigration code listing all files. Error returned: {0} :: {1}", ex.Message, ex.InnerException));
            }
            return folders;
        }

        public FileInfo GetFile(string fileId)
        {
            var acctFile = new FileInfo();

            GetNewToken();

            try
            {
                string url = String.Format(UriFiles, fileId);

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization: Bearer " + token);
                WebResponse response = request.GetResponse();
                
                var reader = new StreamReader(response.GetResponseStream());

                string responseString = reader.ReadToEnd();

                if (responseString.Length > 3) //Json returns "{}" for blank data
                {
                    var jsonSettings = new JsonSerializerSettings();
                    jsonSettings.NullValueHandling = NullValueHandling.Ignore;
                    jsonSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

                    acctFile = JsonConvert.DeserializeObject<FileInfo>(responseString, jsonSettings);
                }

                response.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(
                        string.Format("Error occured in Box Intigration code getting a file. Error returned: {0} :: {1}", ex.Message, ex.InnerException));
            }

            return acctFile;
        }

        public Files UpdateFile(string fileId, string body)
        {
            string uri = String.Format(UriFiles, fileId);
            Files fileData = new Files();

            GetNewToken();

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("Authorization: Bearer " + token);
                    client.Headers.Add("Content-Type", "application/json");
                    string response = client.UploadString(uri, "PUT", body);

                    fileData = JsonConvert.DeserializeObject<Files>(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Error occured in Box Intigration code updating a file. Error returned: {0} :: {1}", ex.Message, ex.InnerException));
            }

            return fileData;
        }
        #endregion Implement IBoxIntegrationManager

        #region Private Methods
        private void GetNewToken()
        {
            try
            {
                string newToken = PostRefreshToken(UriTokenString);
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

        private Folder BuildTree(Folder folders, string result, string access_token)
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
                            client.Headers.Add("Authorization: Bearer " + access_token);
                            string fileresult = client.DownloadString(String.Format(UriFiles, item.id));
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
                            client.Headers.Add("Authorization: Bearer " + access_token);
                            string folderresult =
                                client.DownloadString(String.Format(UriFolders, item.id));
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
        #endregion Private Methods

    }
}
