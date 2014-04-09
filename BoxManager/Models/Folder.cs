using System;
using System.Collections.Generic;

namespace BoxManager.Models
{
    public class Folder
    {
        public string type { get; set; }
        public string id { get; set; }
        public string sequence_id { get; set; }
        public string etag { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public PathCollection path_collection { get; set; }
        public Entry created_by { get; set; }
        public Entry modified_by { get; set; }
        public DateTime? trashed_at { get; set; }
        public DateTime? purged_at { get; set; }
        public DateTime? content_created_at { get; set; }
        public DateTime? content_modified_at { get; set; }
        public Entry owned_by { get; set; }
        public string sared_link { get; set; }
        public string folder_upload_email { get; set; }
        public Entry parent { get; set; }
        public string item_status { get; set; }
        public ItemCollection item_collection { get; set; }
        public IList<Folder> Folders { get; set; }
        public IList<Files> Files { get; set; }
    }
}

/*
{
	"type":"folder",
	"id":"1818218311",
	"sequence_id":"0",
	"etag":"0",
	"name":"pix",
	"created_at":"2014-04-06T09:56:18-07:00",
	"modified_at":"2014-04-07T05:17:35-07:00",
	"description":"",
	"size":98101,
	"path_collection":
	{
		"total_count":1,
		"entries":[
		{
			"type":"folder",
			"id":"0",
			"sequence_id":null,
			"etag":null,
			"name":"All Files"
		}]
	},
	"created_by":
	{
		"type":"user",
		"id":"213505951",
		"name":"Jim Ballard",
		"login":"AntlerHorse@gmail.com"
	},
	"modified_by":
	{
		"type":"user",
		"id":"213505951",
		"name":"Jim Ballard",
		"login":"AntlerHorse@gmail.com"
	},
	"trashed_at":null,
	"purged_at":null,
	"content_created_at":"2014-04-06T09:56:18-07:00",
	"content_modified_at":"2014-04-07T05:17:35-07:00",
	"owned_by":
	{
		"type":"user",
		"id":"213505951",
		"name":"Jim Ballard",
		"login":"AntlerHorse@gmail.com"
	},
	"shared_link":null,
	"folder_upload_email":null,
	"parent":
	{
		"type":"folder",
		"id":"0",
		"sequence_id":null,
		"etag":null,
		"name":"All Files"
	},
	"item_status":"active",
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
}
*/