using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using WebApiTestApp.Models;

namespace WebApiTestApp.Services
{
    public class DocumentNotificationManager
    {
        CloudQueueClient queueClient;
        CloudQueue queue;

        string queueName = "document-uploaded";

        public DocumentNotificationManager()
        {
            var saName = ConfigurationManager.AppSettings["AzureStorageAccountName"];
            var saKey = ConfigurationManager.AppSettings["AzureStorageAccountAccessKey1"];
            var credentials = new StorageCredentials(saName, saKey);
            var storageAccount = new CloudStorageAccount(credentials, true);

            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
            queue.ClearAsync();

            queue.Metadata.Add(new KeyValuePair<string, string>("PollingGap", "10"));
            queue.SetMetadataAsync().Wait();
        }

        public async Task AddAsync(string fileName, string uri)
        {
            var data = $"FileName: {fileName}; AzureFileUri: {uri}; DateTimeAdded: {DateTime.UtcNow}";
            var message = new CloudQueueMessage(data);

            await queue.AddMessageAsync(message);
        }

        public async Task<ShowModel> GetAsync()
        {
            // Q: get by name, id?
            // Q: table - aggregation functions
            // Q: private ququq?

            var msgReceived = await queue.GetMessageAsync().ConfigureAwait(false);

            if (msgReceived != null)
            {
                var data = msgReceived.AsString.Split(';');
                var pollingGap = int.Parse(queue.Metadata["PollingGap"]);
                await queue.DeleteMessageAsync(msgReceived);

                return new ShowModel
                {
                    FileName = data[0].Trim().Split(':')[1],
                    Uri = data[1].Trim().Substring(14),
                    DateTimeAdded = DateTime.Parse(data[2].Trim().Substring(15)),
                    PollingGap = pollingGap
                };
            }
            return new ShowModel();
        }
    }
}