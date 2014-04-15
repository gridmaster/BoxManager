using System.Collections.Generic;
using Newtonsoft.Json;

namespace BoxManager.Models
{
    public class Token
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public List<RestrictedTo> restricted_to { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
    }

    public class RestrictedTo
    {
        public string scope { get; set; }

        [JsonProperty("object")]
        public restrictedObject restrictedToObject { get; set; }
    }

    public class restrictedObject
    {

        [JsonProperty("type")]
        public string objectType { get; set; }

        public string id { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string name { get; set; }
    }
}


//{
//    "access_token": "GmVknkzQcvA3XLuutjVrNgAjqXxFE0HD",
//    "expires_in": 3600,
//    "restricted_to": [
//        {
//            "scope": "folder_readwrite",
//            "object": {
//                "type": "folder",
//                "id": "11697178",
//                "sequence_id": "0",
//                "etag": "0",
//                "name": "Increo Documents"
//            }
//        }
//    ],
//    "refresh_token": "F93tWpXOGsUSMhlUG6Nid2VoOtZgBemusfxLwkgTKk7YSdx7E2aePyg2dQjvIBAu",
//    "token_type": "bearer"
//}

//{"access_token":"ecxEgnyDwBmQd9BxkWGeToLyeD4BzcBi","expires_in":4274,"restricted_to":[],
// "refresh_token":"KjHEtIVEZ3YJ0zCd3qjCM5wNmsKFqfTGGokINXr2ZfBoaWI7j2SF1BA9Z13N0h1f","token_type":"bearer"}