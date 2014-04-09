using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BoxManager.BoxModels
{
    // Summary:
    //     Defines base properties of Box.NET folder
    [DebuggerDisplay("ID = {ID}, Name = {Name}, FolderType = {FolderTypeID}, IsShared = {IsShared}")]
    public class FolderBase
    {
        // Summary:
        //     Initializes folder object
        public FolderBase()
        {
        }

        // Summary:
        //     Type of the folder. Could be null
        public long? FolderTypeID { get; set; }
        //
        // Summary:
        //     ID of the folder
        public long ID { get; set; }
        //
        // Summary:
        //     Indicates if folder is shared
        public bool? IsShared { get; set; }
        //
        // Summary:
        //     Name of the folder
        public string Name { get; set; }
        //
        // Summary:
        //     The ID of the user who owns the folder
        public long? OwnerID { get; set; }
        //
        // Summary:
        //     ID of the parent folder. Could be null
        public long? ParentFolderID { get; set; }
        //
        // Summary:
        //     If the file is shared and password protected, this is the password associated
        //     with that file.  Could be null
        public string Password { get; set; }
        //
        // Summary:
        //     The path of the folder from the root. Could be null
        public string Path { get; set; }
        //
        // Summary:
        //     If the file is shared, this URL can be used to display a shared page.  Could
        //     be null
        public string PublicName { get; set; }
    }
}
