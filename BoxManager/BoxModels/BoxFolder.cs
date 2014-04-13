using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BoxManager.BoxModels
{
    // Summary:
    //     Represents the Box.NET folder entity
    [DebuggerDisplay("ID = {ID}, Name = {Name}, Folders = {Folders.Count}, Files = {Files.Count}, Tags = {Tags.Count}, IsShared = {IsShared}")]
    public sealed class BoxFolder : FolderBase
    {
        // Summary:
        //     Initializes folder object
        public BoxFolder()
        {
        }

        // Summary:
        //     Gets or sets the date when folder was created
        public DateTime Created { get; set; }
        //
        // Summary:
        //     Description of the folder
        public string Description { get; set; }
        //
        // Summary:
        //     File count
        public int FileCount { get; set; }
        //
        // Summary:
        //     Gets or sets list of child files
        public List<BoxFile> Files { get; set; }
        //
        // Summary:
        //     Gets or sets list subfolders
        public List<BoxFolder> Folders { get; set; }
        //
        // Summary:
        //     Folder permissions
        public UserPermissionFlags? PermissionFlags { get; set; }
        //
        // Summary:
        //     Role
        public string Role { get; set; }
        //
        // Summary:
        //     Link to shared folder
        public string SharedLink { get; set; }
        //
        // Summary:
        //     Size of the folder
        public long? Size { get; set; }
        //
        // Summary:
        //     List of tags associated with folder
        // public List<TagPrimitive> Tags { get; set; }
        //
        // Summary:
        //     Gets or sets the date when folder was updated last time
        public DateTime Updated { get; set; }
    }
}