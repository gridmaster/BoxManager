using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BoxManager.BoxModels
{
    // Summary:
    //     Represents the Box.NET file entity
    [DebuggerDisplay("ID = {ID}, Name = {Name}, Size = {Size}, Tags = {Tags.Count} IsShared = {IsShared}")]
    public class File
    {
        public File()
        {
        }

        // Summary:
        //     Date when file was created
        public DateTime Created { get; set; }
        //
        // Summary:
        //     Gets or sets file description
        public string Description { get; set; }
        //
        // Summary:
        //     Gets or sets file ID
        public long ID { get; set; }
        //
        // Summary:
        //     Indicates if file is shared
        public bool? IsShared { get; set; }
        //
        // Summary:
        //     Gets or sets file name
        public string Name { get; set; }
        //
        // Summary:
        //     Gets or sets ID of the file's owner
        public long? OwnerID { get; set; }
        //
        // Summary:
        //     User permissions for file
        public UserPermissionFlags? PermissionFlags { get; set; }
        //
        // Summary:
        //     Gets or sets public name of the file (if file is shared)
        public string PublicName { get; set; }
        //
        // Summary:
        //     Gets or sets SHA1 hash
        public string SHA1Hash { get; set; }
        //
        // Summary:
        //     Link to shared file
        public string SharedLink { get; set; }
        //
        // Summary:
        //     File size
        public long Size { get; set; }
        //
        // Summary:
        //     List of tags associated with file
        // public List<TagPrimitive> Tags { get; set; }
        //
        // Summary:
        //     Date when file was updated
        public DateTime? Updated { get; set; }
    }
}