using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Configuration;

namespace SensoComum.APIs.ProcessValues
{
    public static class ProcessQueue
    {
        private static IConfiguration _configuration;

        [FunctionName("ProcessQueue")]
        public static async Task Run([QueueTrigger("values", Connection = "AzureWebJobsStorage")]string myQueueItem, TraceWriter log, ExecutionContext executionContext)
        {
            SetupConfigurationManager(executionContext);

            int.TryParse(myQueueItem, out int value);

            if(value == 0)
            {
                log.Info("Non-string or zero as a value on queue. Dequeuing...");
            }
            else
            {
                await SumUp(value);
            }
        }

        private static async Task SumUp(int value)
        {
            CloudTable table = await RetrieveCloudTableInstance();

            CommonSenseResult commonSenseValue = await RetrieveCommonSenseValue(table);

            commonSenseValue.Update(value);

            await UpdateCommonSense(table, commonSenseValue);
        }

        private static void SetupConfigurationManager(ExecutionContext executionContext)
        {
            _configuration = new ConfigurationBuilder()
                            .SetBasePath(executionContext.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();
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
            string connectionString = _configuration["AzureWebJobsStorage"];

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("currentValue");

            bool tableExists = await table.CreateIfNotExistsAsync();

            if (tableExists)
            {
                await InitializeCommonSense(table);
            }

            return table;
        }

        private static async Task UpdateCommonSense(CloudTable table, CommonSenseResult commonSenseValue)
        {
            TableOperation tableOperation = TableOperation.Replace(commonSenseValue);

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
