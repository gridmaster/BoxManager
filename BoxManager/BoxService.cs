using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using BoxManager.Models;
using Newtonsoft.Json;

namespace BoxManager
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BoxService : IBoxService
    {
        // this id is the app ID generated at Box
        private static string client_id = "u9lrankemqfefpsbhmmbfikkhmhg4iao";
        // this is a new GUID selected during development
        private static string securety_token = "d66d5824-ae41-4249-902e-c32ad5c5b244";
        // this is the client secret that is generated at Box.
        private static string client_secret = "P1HpIuastZ1vr9QCKgX5B5bAjEQvupqq";

        private static string uriTokenString = "https://www.box.com/api/oauth2/token";
        //private static string uriRefreshToken = "https://www.box.com/api/oauth2/token/?grant_type=refresh_token&refresh_token={valid refresh token}&client_id={your_client_id}&client_secret={your_client_secret}'

        public string GetToken()
        {
            string error = string.Empty;
            string errorDescription = string.Empty;
            string code = string.Empty;
            string state = string.Empty;

            if (WebOperationContext.Current != null)
            {
                error = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["error"];
                errorDescription = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["error_description"];
                code = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["code"];
                state = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["state"];
            }

            string result = string.Empty;
            string uri = string.Empty;

            if (string.IsNullOrEmpty(error))
            {

                result = PostAccessToken(uriTokenString, code);
                Token token = new Token();
                Folder folders = new Folder();

                try
                {
                    token = JsonConvert.DeserializeObject<Token>(result);

                    using (var client = new WebClient())
                    {
                        client.Headers.Add("Authorization: Bearer " + token.access_token);
                        result = client.DownloadString("https://api.box.com/2.0/folders/0"); // 1818218311");
                        folders = JsonConvert.DeserializeObject<Folder>(result);
                    }

                    folders.Folders = new List<Folder>();
                    folders.Files = new List<Files>();
                    DateTime start = DateTime.Now;
                    folders = BuildTree(folders, result, token.access_token);
                    System.TimeSpan end = DateTime.Now.Subtract(start);

                    Trace.WriteLine(string.Format("Minutes: {0}, Seconds: {1}, Milliseconds: {2}", end.Minutes, end.Seconds, end.Milliseconds));

                    result = JsonConvert.SerializeObject(folders);

                    Folder testresult = JsonConvert.DeserializeObject<Folder>(result);

                    // https://www.box.com/api/oauth2/token/?grant_type=refresh_token&refresh_token={valid refresh token}&client_id={your_client_id}&client_secret={your_client_secret}'
                    //result = PostRefreshToken(uriTokenString, token.refresh_token);
                    //token = JsonConvert.DeserializeObject<Token>(result);

                    //using (var client = new WebClient())
                    //{
                    //    client.Headers.Add("Authorization: Bearer " + token.access_token);
                    //    result = client.DownloadString("https://api.box.com/2.0/files/15833091479");
                    //    Files files = JsonConvert.DeserializeObject<Files>(result);
                    //}
                }
                catch (Exception ex)
                {
                    result = string.Format("Error occured: {0}", ex.Message);
                }
            }
            else
            {
                result = string.Format("\nerror: {0}\nerror_description: {1}", error, errorDescription);
            }

            Trace.WriteLine(result);
 
            return result;
        }

        private Folder BuildTree(Folder folders, string result, string access_token)
        {
            for (int i = 0; i < folders.item_collection.entries.Count; i++)
            {
                var item = folders.item_collection.entries[i];

                if (item.type == "file")
                {
                    using (var client = new WebClient())
                    {
                        Files file = new Files();
                        client.Headers.Add("Authorization: Bearer " + access_token);
                        result = client.DownloadString(string.Format("https://api.box.com/2.0/files/{0}", item.id));
                        file = JsonConvert.DeserializeObject<Files>(result);
                        if (folders.Files == null)
                            folders.Files = new List<Files>();
                        folders.Files.Add(file);
                        continue;
                    }
                }

                if (item.type == "folder")
                {
                    using (var client = new WebClient())
                    {
                        try
                        {
                            Folder fold = new Folder();
                            client.Headers.Add("Authorization: Bearer " + access_token);
                            result = client.DownloadString(string.Format("https://api.box.com/2.0/folders/{0}", item.id));
                            fold = JsonConvert.DeserializeObject<Folder>(result);
                            if (folders.Folders == null)
                                folders.Folders = new List<Folder>();
                            folders.Folders.Add(fold);
                            BuildTree(fold, result, access_token);
                        }
                        catch (Exception ex)
                        {
                            result = ex.Message;
                        }
                    }
                }
            }

            return folders;
        }


        private static string PostRefreshToken(string uri, string code)
        {
            string responseString = string.Empty;

            try
            {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["grant_type"] = "refresh_token";
                    values["refresh_token"] = code;
                    values["client_id"] = client_id;
                    values["client_secret"] = client_secret;

                    var response = client.UploadValues(uri, values);

                    responseString = Encoding.Default.GetString(response);
                }
            }
            catch (Exception ex)
            {
                responseString = ex.Message;
            }
            
            return responseString;
        }

        private static string PostAccessToken(string uri, string code)
        {
            string responseString = string.Empty;

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["grant_type"] = "authorization_code";
                values["code"] = code;
                values["client_id"] = client_id;
                values["client_secret"] = client_secret;

                var response = client.UploadValues(uri, values);

                responseString = Encoding.Default.GetString(response);
            }
            return responseString;
        }
    }
}