
namespace BoxIntegrator.Core
{
    public class CoreConstants
    {
        // API Uri Constants
        public const string BaseUri = "https://api.box.com/2.0";
        public const string UriUpload = "https://upload.box.com/api/2.0";

        // The first part of OAuth2 to authenticate the user
        // Example: "https://www.box.com/api/oauth2/authorize?response_type=code&client_id=zb185q3gef41kbee9qi3y0kej2tdeq2f&state=security_token%3Dd66d5824-ae41-4249-902e-c32ad5c5b244";
        // Required: response_type, client_id, client_secret
        // Optional: redirect_uri, state, scope, folder_id, redirect_uri, username, box_device_id, box_device_name
        public const string UriAuthorize = "https://www.box.com/api/oauth2/authorize";

        // This grants the Token
        // Example: grant_type=authorization_code&code=YOUR_AUTH_CODE&client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET
        // Method: POST
        // Required: grant_type, client_id, 
        // Optional: code, refresh_token
        public const string UriTokenString = "https://www.box.com/api/oauth2/token"; // POST

        // Revoke access
        // Example: 'client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET&token=YOUR_TOKEN' 
        // Method: POST
        // Required: client_id, client_secret, token
        public const string UriRevoke = "https://www.box.com/api/oauth2/revoke"; // POST

        public const string UriAPI = "https://www.box.com/api/oauth2/";

        /*******************************************************************************/
        // fields parameter:
        // use fields URL parameter to name what to be returned
        // https://api.box.com/2.0/files/FILE_ID?fields=modified_at,path_collection,name
        /*******************************************************************************/

        // Create a folder 
        // Example: POST /folders
        //      or: https://api.box.com/2.0/folders -H "Authorization: Bearer ACCESS_TOKEN" -d '{"name":"New Folder", "parent": {"id": "0"}}'
        // Required: name, parent, id
        public const string UriBaseFolders = BaseUri + "/folders"; // POST

        // Get the info about a folder
        // GET /folders/{folder id}
        // Update info about a folder
        // PUT /folders/{folder id}
        // Example: https://api.box.com/2.0/folders/FOLDER_ID -H "Authorization: Bearer ACCESS_TOKEN" -d '{"name":"New Folder Name!"}'
        // Request Body attributes: name, description, parent, id, shared_link, access, unshared_at, permissions
        //      permissions.can_download, permissions.can_preview, folder_upload_email, access, owned_by, id, sync_state, tags
        // Delete a folder
        // DELETE /folders/{folder id}
        // Example: https://api.box.com/2.0/folders/FOLDER_ID?recursive=true
        public const string UriFolders = BaseUri + "/folders/{0}"; // GET, PUT, DELETE
        
        // Create a shared link using UriFolders
        // Example: PUT /folders/{folder id}
        //      or: https://api.box.com/2.0/folders/FOLDER_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //              -d '{"shared_link": {"access": "open"}}' 
        // Request body attributes
        // shared_link, shared_link.access, shared_link.unshared_at, shared_link.permissions
        //  shared_link.permissions.can_download, shared_link.permissions.can_preview


        // Retrieve a folders items
        // Exmaple: GET folders/{id}/items
        //      or: https://api.box.com/2.0/folders/FOLDER_ID/items?limit=2&offset=0 
        // Optional: fields, limit, offset
        public const string UriFoldersItems = UriFolders + "/items"; // GET
        
        // Copy a folder
        // Example: POST /folders/{folder id}/copy
        //      or: https://api.box.com/2.0/folders/FOLDER_ID/copy -H "Authorization: Bearer ACCESS_TOKEN" -d '{"parent": {"id" : DESTINATION_FOLDER_ID}}
        // Request Body attributes
        // Required: parent, id
        // Optional: name
        public const string UriFoldersCopy = UriFolders + "/copy";

        // view a folders colaborators
        // Example: GET /folders/{id}/colaborations
        public const string UriFoldersCollaborations = UriFolders + "/collaborations";

        // Get items in the trash
        // Example: GET /folders/trash/items
        //      or: https://api.box.com/2.0/folders/trash/items?limit=2&offset=0
        // URL Parameters: fields, limit, offset
        public const string GetFoldersFromTrash = UriBaseFolders + "/trash/items";

        // Get a trashed folder
        // Example: GET /folders/{folder id}/trash
        // Permanently delete a trashed folder
        // Example: DELETE /folders/{folder id}/trash
        public const string UriGetTrashedFolder = UriFolders + "/trash";

        // Restore a trashed folder
        // Example: POST /folders/{folder id}
        //      or: https://api.box.com/2.0/folders/FOLDER_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -d '{"name":"non-conflicting-name"}'
        // Request Body Attributes:
        //      name, parent, parent.id

        // Get information about a file
        // Example: GET /files/{file id}
        // Change a files information
        // Example: PUT /files/{file id}
        //      or: https://api.box.com/2.0/files/FILE_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -d '{"name":"new name.jpg"}
        // Headers: If-Match
        // Request Body Attributes:
        // name, description, parent, parent.id, shared_link, shared_link.access, shared_link.unshared_at,
        //       shared_link.permissions, shared_link.permissions.download, shared_link.permissions.preview, tags
        
        // Delete a file
        // Headers: If-Match the etag of the file
        // Returns and empty 204 for successful delete, or 412 Precondition Failed if If-Match fales
        // Example: DELETE /files/{id}
        //      or: ttps://api.box.com/2.0/files/FILE_ID -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -H "If-Match: a_unique_value"
        public const string UriFiles = BaseUri + "/files/{0}"; // GET, PUT, DELETE
        
        // Download a file
        // URL Parameters: version
        // Example: GET /files/{file id}/content
        // Response is 302 if found, 202 if accepted but not available and a Ready-After header
        public const string UriFilesContent = UriFiles + "/content"; // POST

        // Upload a file
        // Example: POST https://upload.box.com/api/2.0/files/content
        //      or: https://upload.box.com/api/2.0/files/content -H "Authorization: Bearer ACCESS_TOKEN" 
        //                  -F filename=@FILE_NAME -F parent_id=PARENT_FOLDER_ID
        // Headers: Content-MD5 (The SHA1 hash of the file)
        // Form Elements:
        // Required: filename, parent_id
        // Optional: content_created_at, content_modified_at
        public const string UriUploadFile = UriUpload + "/files/content"; // POST

        // Upload a new version of a file
        // Example: POST https://upload.box.com/api/2.0/files/{file id}/content
        //      or: https://upload.box.com/api/2.0/files/FILE_ID/content 
        //                  -H "Authorization: Bearer ACCESS_TOKEN" -H "If-Match: ETAG_OF_ORIGINAL" 
        //                  -F filename=@FILE_NAME
        // Headers: If-Match the etag of the file
        // Form Elements:
        // Required: name
        // Optional: content_modified_at
        // Returns: updated file object
        public const string UriUploadNewVersionFile = UriUpload + "/files/{file id}/content";

        // View versions of a file
        // Example: GET /files/{file id}/versions
        //      or: https://api.box.com/2.0/files/FILE_ID/versions -H "Authorization: Bearer ACCESS_TOKEN"
        // Returns: An array of version objects 
        public const string UriFilesVersions = UriFiles + "/versions";
        
        // Promote an older version of a file
        // Example: POST /files/{file id}/versions/current
        //      or: https://api.box.com/2.0/files/FILE_ID/versions/current -H "Authorization: Bearer ACCESS_TOKEN" \
        //              -d '{"type": "file_version", "id" : "FILE_VERSION_ID"}
        // Request Body Attributes
        // Required: type (must be file_version for this request), id
        // Returns: The newly promoted file_version object i
        public const string UriPromoteFile = UriFiles + "/versions/current";


        public const string UriSharedItems = UriFiles + "/shared_items"; // GET
        public const string UriComments = BaseUri + "/comments";
        public const string UriCollaborations = BaseUri + "/collaborations";
        public const string UriSearch = BaseUri + "/search";
        public const string UriEvents = BaseUri + "/events";
        public const string UriUsers = BaseUri + "/users";
        public const string UriTokens = BaseUri + "/tokens";

        // API magic string constants
        public const string sharedLink = "{{\"shared_link\": {{\"access\": \"{0}\"}}}}";
        public const string authorizationBearer = "Authorization: Bearer ";
        public const string applicationType = "application/json";
    }
}
