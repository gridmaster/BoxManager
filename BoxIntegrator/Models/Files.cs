using System;

namespace BoxIntegrator.Models
{
    public class Files
    {
        public string type { get; set; }
        public string id { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string sha1 { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public long size { get; set; }
        public PathCollection path_collection { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? modified_at { get; set; }
        public DateTime? trashed_at { get; set; }
        public DateTime? purged_at { get; set; }
        public DateTime? content_created_at { get; set; }
        public DateTime? content_modified_at { get; set; }
        public Entry created_by { get; set; }
        public Entry modified_by { get; set; }
        public Entry owned_by { get; set; }
        public ShareInfo shared_link { get; set; }
        public Entry parent { get; set; }
        public string item_status { get; set; }
    }
}



/*
{
   "type":"file",
   "id":"16169522245",
   "sequence_id":"2",
   "etag":"2",
   "sha1":"bb4fb7e7fc9962b5045fad4a849a40e5023b1a16",
   "name":"new name.jpg",
   "description":"",
   "size":68582,
   "path_collection":{
      "total_count":1,
      "entries":[
         {
            "type":"folder",
            "id":"0",
            "sequence_id":null,
            "etag":null,
            "name":"All Files"
         }
      ]
   },
   "created_at":"2014-04-12T15:39:56-07:00",
   "modified_at":"2014-04-13T13:24:25-07:00",
   "trashed_at":null,
   "purged_at":null,
   "content_created_at":"2013-08-28T16:59:41-07:00",
   "content_modified_at":"2013-08-28T16:59:41-07:00",
   "created_by":{
      "type":"user",
      "id":"213505951",
      "name":"Jim Ballard",
      "login":"AntlerHorse@gmail.com"
   },
   "modified_by":{
      "type":"user",
      "id":"213505951",
      "name":"Jim Ballard",
      "login":"AntlerHorse@gmail.com"
   },
   "owned_by":{
      "type":"user",
      "id":"213505951",
      "name":"Jim Ballard",
      "login":"AntlerHorse@gmail.com"
   },
   "shared_link":null,
   "parent":{
      "type":"folder",
      "id":"0",
      "sequence_id":null,
      "etag":null,
      "name":"All Files"
   },
   "item_status":"active"
}
*/