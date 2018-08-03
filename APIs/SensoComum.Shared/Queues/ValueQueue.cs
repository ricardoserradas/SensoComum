/*
 * References:
 * https://github.com/Azure-Samples/storage-queue-dotnet-pop-receipt/blob/master/dotnet/storage-queue-dotnet-popreceipt/Program.cs
 * https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues#create-a-queue
 * https://stackoverflow.com/questions/30575689/how-do-we-use-cloudconfigurationmanager-with-asp-net-5-json-configs#30580006
 * 
 */

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensoComum.Shared.Queues
{
    public class ValueQueue
    {
        CloudQueue _queue;

        public ValueQueue(string connectionString, string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            this._queue = queueClient.GetQueueReference(queueName);

            this._queue.CreateIfNotExistsAsync();
        }

        public void AddMessage(string value)
        {
            CloudQueueMessage message = new CloudQueueMessage(value);

            this._queue.AddMessageAsync(message);
        }
    }
}
