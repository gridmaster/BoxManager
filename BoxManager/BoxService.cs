using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.SessionState;
using BoxIntegrator;
using BoxManager.Models;
using Newtonsoft.Json;

namespace BoxManager
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BoxService : IBoxService
    {
        // this id is the app ID generated at Box
        private static string client_id = "zb185q3gef41kbee9qi3y0kej2tdeq2f"; // "u9lrankemqfefpsbhmmbfikkhmhg4iao";
        // this is a new GUID selected during development
        private static string securety_token = "d66d5824-ae41-4249-902e-c32ad5c5b244";
        // this is the client secret that is generated at Box.
        private static string client_secret = "hmHUk2lgClazVZLIytaASuJQ40RORZP9"; //"P1HpIuastZ1vr9QCKgX5B5bAjEQvupqq";

        private static string uriTokenString = "https://www.box.com/api/oauth2/token";
        //private static string uriRefreshToken = "https://www.box.com/api/oauth2/token/?grant_type=refresh_token&refresh_token={valid refresh token}&client_id={your_client_id}&client_secret={your_client_secret}'

        private static Dictionary<string, CookieContainer> containers = new Dictionary<string, CookieContainer>();

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
                string fileId = "16169522245"; // "16063364337"

                try
                {
                    token = JsonConvert.DeserializeObject<Token>(result);

                    BoxIntegrationManager BI = new BoxIntegrationManager(client_id, client_secret, token.refresh_token);

                    var crap = BI.CreateFileShare(fileId, "Open");

                    var theshit = BI.ListAllFiles("0");



                    var Fstuff = BI.GetFolder("0");

                    var stuff = BI.GetFile(fileId);

                    string body = "{\"name\":\"car.jpg\"}"; // "{\"description\": \"new file description\"}";
                    var jig = BI.UpdateFile(fileId, body);
                    body = "{\"shared_link\": {\"access\": \"open\"}}";
                    jig = BI.UpdateFile(fileId, body);

                    // var jig = PostFileShare(token.access_token, fileId);
                    //    https://app.box.com/s/brb0p33vfsf3e13dckuk

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



        public static string PostFileShare(string token, string fileId)
        {
            string uri = string.Format("https://api.box.com/2.0/files/{0}", fileId);
            string body = "{\"shared_link\": {\"access\": \"open\"}}";

            byte[] postArray = Encoding.ASCII.GetBytes(body);
            
            using (var client = new WebClient())
            {

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Authorization: Bearer " + token);

                var response = client.UploadData(uri, postArray);

                var responseString = Encoding.Default.GetString(response);
            }

            
            using (var client = new WebClient())
            {
                //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded"); // "application/json");
                client.Headers.Add("Authorization: Bearer " + token);

                //UploadData implicitly sets HTTP POST as the request method. 
                byte[] responseArray = client.UploadData(uri, postArray);

                // Decode and display the response.
                var tits =  Encoding.ASCII.GetString(responseArray);

                var values = new NameValueCollection();
                values["Authorization: Bearer"] = token;
                
                var response = client.UploadValues(uri, values);

                var responseString = Encoding.Default.GetString(response);
            }

            try
            {
                    var request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add("Authorization: Bearer " + token);

              
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
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