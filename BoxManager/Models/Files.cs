using System;
using System.Collections.Generic;

namespace BoxManager.Models
{
    public class Files
    {
        public string type { get; set; }
        public string id { get; set; }
        public string etag { get; set; }
        public DateTime? modified_at { get; set; } // "2012-10-04T12:34:05-07:00"
        public PathCollection path_collection { get; set; }
        public string name { get; set; }
    }
}

//{
//    "type": "file",
//    "id": "3447956798",
//    "etag": "1",
//    "modified_at": "2012-10-04T12:34:05-07:00",
//    "path_collection": {
//        "total_count": 2,
//        "entries": [
//            {
//                "type": "folder",
//                "id": "0",
//                "sequence_id": null,
//                "etag": null,
//                "name": "All Files"
//           },
//           {
//               "type": "folder",
//               "id": "423404344",
//               "sequence_id": "15",
//               "etag": "15",
//               "name": "General Info"
//           }
//       ]
//   },
//   "name": "Org Chart.PDF"
//}