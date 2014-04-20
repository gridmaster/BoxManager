
using System;

namespace BoxIntegrator.Models
{
    public class Entry
    {
        public string type { get; set; }
        public string id { get; set; }
        public Entry created_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime modified_at { get; set; }
        public DateTime expires_at { get; set; }
        public string status { get; set;}
        public Entry accessible_by { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string sha1 { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string role { get; set; }
        public DateTime acknowledged_at { get; set; }
        public Item item { get; set; }
    }
}

/*
{
    "total_count": 1,
    "entries": [
        {
            "type": "collaboration",
            "id": "14176246",
            "created_by": {
                "type": "user",
                "id": "4276790",
                "name": "David Lee",
                "login": "david@box.com"
            },
            "created_at": "2011-11-29T12:56:35-08:00",
            "modified_at": "2012-09-11T15:12:32-07:00",
            "expires_at": null,
            "status": "accepted",
            "accessible_by": {
                "type": "user",
                "id": "755492",
                "name": "Simon Tan",
                "login": "simon@box.net"
            },
            "role": "editor",
            "acknowledged_at": "2011-11-29T12:59:40-08:00",
            "item": null
        }
    ]
}
*/