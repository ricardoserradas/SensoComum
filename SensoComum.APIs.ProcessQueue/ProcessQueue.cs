using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SensoComum.APIs.ProcessQueue
{
    public static class ProcessQueue
    {
        [FunctionName("ProcessQueue")]
        public static void Run([QueueTrigger("values", Connection = "AzureWebJobsStorage")]string myQueueItem, TraceWriter log)
        {
            int.TryParse(myQueueItem, out int value);

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=scapisstoragedev;AccountKey=RmYV4v06YsQJkUbs9Kp5L5WlwSKQo4LUI0XIV8q21nl3pfjHkoFuSYFz2zRV8CdrzhfqsJTtHlidr7wBDfeOyg==;EndpointSuffix=core.windows.net";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("currentValue");

            table.CreateIfNotExists();

            TableQuery query = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "sum"));

            var retrievedValue = table.ExecuteQuery(query);
        }
    }
}
