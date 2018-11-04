using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace WebApiTestApp.Services
{
    public class DocumentStore
    {
        CloudBlobClient blobCLient;
        CloudBlobContainer container;

        string baseUri = "https://mentoringlearnstorage.blob.core.windows.net/";

        public DocumentStore()
        {
            var saName = ConfigurationManager.AppSettings["AzureStorageAccountName"];
            var saKey = ConfigurationManager.AppSettings["AzureStorageAccountAccessKey1"];
            var credentials = new StorageCredentials(saName, saKey);
            blobCLient = new CloudBlobClient(new Uri(baseUri), credentials);
            container = blobCLient.GetContainerReference("documents");
            container.CreateIfNotExists();
        }


        public async Task SaveDocumentAsync(Stream stream, string fileName)
        {
            var blob = container.GetBlockBlobReference(fileName);
            await blob.UploadFromStreamAsync(stream);
        }

        public string UriFor(string fileName)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };
            var blob = container.GetBlockBlobReference(fileName);
            var sas = blob.GetSharedAccessSignature(sasPolicy);

            return $"{baseUri}documents/{fileName}{sas}";
        }
    }
}