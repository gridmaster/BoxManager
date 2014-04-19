
using System;

namespace BoxManager.Models
{
    public class Comment
    {
        public string type { get; set; }
        public string id { get; set; }
        public string is_reply_comment { get; set; }
        public string message { get; set; }
        public Entry created_by { get; set; }
        public DateTime created_at { get; set; }
        public Item item { get; set; }
        public DateTime modified_at { get; set; } 
    }
}

/*
{
    "type": "comment",
    "id": "191969",
    "is_reply_comment": false,
    "message": "These tigers are cool!",
    "created_by": {
        "type": "user",
        "id": "17738362",
        "name": "sean rose",
        "login": "sean@box.com"
    },
    "created_at": "2012-12-12T11:25:01-08:00",
    "item": {
        "id": "5000948880",
        "type": "file"
    },
    "modified_at": "2012-12-12T11:25:01-08:00"
}
*/