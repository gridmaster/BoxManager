﻿using BoxIntegrator.Models;

namespace BoxIntegrator.Interfaces
{
    public interface IBoxIntegrationManager
    {
        Folder ListAllFiles(string folderId);
        FileInfo GetFile(string fileId);
        Files UpdateFile(string fileId, string body);
    }
}