using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace BoxIntegrator
{
    public class BaseBoxManager
    {
        protected string clientId { get; set; }
        protected string clientSecret { get; set; }
        protected string refreshToken { get; set; }

        public BaseBoxManager(string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(clientId) )
                throw new ArgumentException("Constructor must contain clientId");
            if (string.IsNullOrEmpty(clientSecret))
                throw new ArgumentException("Constructor must contain clientSecret");
            //if (string.IsNullOrEmpty(refreshToken))
            //    throw new ArgumentException("Constructor must contain refreshToken");

            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.refreshToken = "";
        }

        public string PostRefreshToken(string uri)
        {
            string responseString = string.Empty;

            try
            {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["grant_type"] = "refresh_token";
                    values["refresh_token"] = refreshToken;
                    values["client_id"] = clientId;
                    values["client_secret"] = clientSecret;

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
    }
}
