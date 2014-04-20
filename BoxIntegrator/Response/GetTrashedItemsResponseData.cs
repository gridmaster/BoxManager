﻿using System.Collections.Generic;
using BoxIntegrator.Models;

namespace BoxIntegrator.Response
{
    public class GetTrashedItemsResponseData : BaseResponseData
    {
        public int total_count { get; set; }
        public IList<Entry> entries { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
    }
}
/*
    "total_count": 49542,
    "entries": [
        {
            "type": "file",
            "id": "2701979016",
            "sequence_id": "1",
            "etag": "1",
            "sha1": "9d976863fc849f6061ecf9736710bd9c2bce488c",
            "name": "file Tue Jul 24 145436 2012KWPX5S.csv"
        },
        {
            "type": "file",
            "id": "2698211586",
            "sequence_id": "1",
            "etag": "1",
            "sha1": "09b0e2e9760caf7448c702db34ea001f356f1197",
            "name": "file Tue Jul 24 010055 20129Z6GS3.csv"
        }
    ],
    "offset": 0,
    "limit": 2
}
*/