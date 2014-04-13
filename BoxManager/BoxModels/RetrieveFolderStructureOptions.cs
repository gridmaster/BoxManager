using System;

namespace BoxManager.BoxModels
{
    // Summary:
    //     Specifies options for folder structure retrieval operation
    [Flags]
    public enum RetrieveFolderStructureOptions
    {
        // Summary:
        //     Indicates that retrieve process should use default options
        None = 0,
        //
        // Summary:
        //     Indicates that only folders must be included in the result tree, no files
        NoFiles = 1,
        //
        // Summary:
        //     Indicates that XML folder structure tree should not be compressed
        NoZip = 2,
        //
        // Summary:
        //     Indicates that only one level of folder structure tree should be retrieved,
        //     so you will get only files and folders stored in folder which FolderID you
        //     have provided
        OneLevel = 4,
        //
        // Summary:
        //     Indicates that XML folder structure tree shouldn't contain all the details
        //     (thumbnails, shared status, tags, and other attributes are left out).  Recomended
        //     to use in mobile applications
        Simple = 8,
    }
}