using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Configuration;
using SensoComum.Shared.Models;
using SensoComum.Shared.Tables;

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
            var valueTable = new ValueTable(
                _configuration["AzureWebJobsStorage"]
                , "currentValue");

            await valueTable.InitializeIfNotExists();

            CommonSenseResult commonSenseValue = await valueTable.RetrieveValue("JeepCompass", "JeepCompass");

            commonSenseValue.Update(value);

            valueTable.Update(commonSenseValue);
        }

        private static void SetupConfigurationManager(ExecutionContext executionContext)
        {
            _configuration = new ConfigurationBuilder()
                            .SetBasePath(executionContext.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();
        }
    }
}
