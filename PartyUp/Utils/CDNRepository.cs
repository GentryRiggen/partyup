using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyUp.Utils
{
    public class CDNRepository
    {
        public static CloudBlobContainer GetContainer(string containerName)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Utilities.GetSetting("StorageConnectionString"));
            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            return blobClient.GetContainerReference(containerName);
        }
    }
}