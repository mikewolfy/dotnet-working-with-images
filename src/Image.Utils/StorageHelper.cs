using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Image.Utils
{
    public class StorageHelper
    {
        CloudStorageAccount storageAccount;
        CloudBlobClient client;
        CloudBlobContainer container;

        public StorageHelper(string connectionString)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
            client = storageAccount.CreateCloudBlobClient();
            container = client.GetContainerReference("images");
        }

        public async Task<bool> SaveToBlobStorage(string blobName, byte[] data)
        {
            var blob = container.GetBlockBlobReference(blobName);
            await blob.UploadFromByteArrayAsync(data, 0, data.Length);

            return true;
        }
    }
}
