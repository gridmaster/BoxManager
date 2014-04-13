using System.Collections.Generic;

namespace BoxIntegrator.Models
{
    public class ItemCollection
    {
        public int total_count { get; set; }
        public IList<Entry> entries { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public IList<Order> order { get; set; } 
    }
}

/*
	"item_collection":
	{
		"total_count":2,
		"entries":[
		{
			"type":"file",
			"id":"15833091479",
			"sequence_id":"2",
			"etag":"2",
			"sha1":"1567c696f7a86ef636c0db41dc86ac56f815e121",
			"name":"clouds.jpg"
		},			{
			"type":"file",
			"id":"15995918533",
			"sequence_id":"1",
			"etag":"1",
			"sha1":"f1cb04dbbb4322d11a065e5acd8f132b88a90b92",
			"name":"doogins.jpg"
		}],
		"offset":0,
		"limit":100,
		"order":[
		{
			"by":"type",
			"direction":"ASC"
		},
		{
			"by":"name",
			"direction":"ASC"
		}]
	}
*/