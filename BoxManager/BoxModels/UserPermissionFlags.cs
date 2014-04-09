using System;

namespace BoxManager.BoxModels
{
    // Summary:
    //     Specifies type of user access for specific object
    [Flags]
    public enum UserPermissionFlags
    {
        // Summary:
        //     No permissions
        None = 1,
        //
        // Summary:
        //     Permission to download the object
        Download = 2,
        //
        // Summary:
        //     Permission to delete the object
        Delete = 4,
        //
        // Summary:
        //     Permission to rename the object
        Rename = 8,
        //
        // Summary:
        //     Permission to share the object
        Share = 16,
        //
        // Summary:
        //     Permission to upload the object
        Upload = 32,
        //
        // Summary:
        //     Permission to view the object
        View = 64,
    }
}