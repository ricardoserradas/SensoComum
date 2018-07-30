using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SensoComum.APIs.ProcessValues
{
    public static class ProcessQueue
    {
        [FunctionName("ProcessQueue")]
        public static async Task Run([QueueTrigger("values", Connection = "AzureWebJobsStorage")]string myQueueItem, TraceWriter log)
        {
            int.TryParse(myQueueItem, out int value);

            CloudTable table = await RetrieveCloudTableInstance();

            CommonSenseResult commonSenseValue = await RetrieveCommonSenseValue(table);

            commonSenseValue.Update(value);

            await UpdateCommonSense(table, commonSenseValue);
        }

        private static async Task<CommonSenseResult> RetrieveCommonSenseValue(CloudTable table)
        {
            TableOperation retrieveValue = TableOperation.Retrieve<CommonSenseResult>("JeepCompass", "JeepCompass");

            TableResult retrieveResult = await table.ExecuteAsync(retrieveValue);

            CommonSenseResult commonSenseValue;

            if (retrieveResult.Result == null)
            {
                commonSenseValue = await InitializeCommonSense(table);
            }
            else
            {
                commonSenseValue = (CommonSenseResult)retrieveResult.Result;
            }

            return commonSenseValue;
        }

        private static async Task<CloudTable> RetrieveCloudTableInstance()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=scapisstoragedev;AccountKey=RmYV4v06YsQJkUbs9Kp5L5WlwSKQo4LUI0XIV8q21nl3pfjHkoFuSYFz2zRV8CdrzhfqsJTtHlidr7wBDfeOyg==;EndpointSuffix=core.windows.net";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("currentValue");

            bool tableExists = await table.CreateIfNotExistsAsync();

            if (!tableExists)
            {
                await table.CreateIfNotExistsAsync();

                await InitializeCommonSense(table);
            }

            return table;
        }

        private static async Task UpdateCommonSense(CloudTable table, CommonSenseResult commonSenseValue)
        {
            TableOperation tableOperation = TableOperation.InsertOrReplace(commonSenseValue);

            await table.ExecuteAsync(tableOperation);
        }

        private static async Task<CommonSenseResult> InitializeCommonSense(CloudTable table)
        {
            CommonSenseResult senseResult = new CommonSenseResult("JeepCompass");

            TableOperation tableOperation = TableOperation.Insert(senseResult);

            await table.ExecuteAsync(tableOperation);

            return senseResult;
        }
    }
}
