using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SensoComum.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SensoComum.Shared.Tables
{
    public class ValueTable
    {
        CloudTable _table;

        public ValueTable(string connectionString, string table)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            this._table = tableClient.GetTableReference(table);
        }

        public async Task<ValueTable> InitializeIfNotExists()
        {
            bool tableExists = await this._table.CreateIfNotExistsAsync();

            if (tableExists)
            {
                InitializeCommonSense(this._table);
            }

            return this;
        }

        public async Task<CommonSenseResult> RetrieveValue(string partitionKey, string rowKey)
        {
            TableOperation retrieveValue = TableOperation.Retrieve<CommonSenseResult>(partitionKey, rowKey);

            TableResult retrieveResult = await this._table.ExecuteAsync(retrieveValue);

            CommonSenseResult commonSenseValue;

            commonSenseValue = (CommonSenseResult)retrieveResult.Result;

            return commonSenseValue;
        }

        private static async void InitializeCommonSense(CloudTable table)
        {
            CommonSenseResult senseResult = new CommonSenseResult("JeepCompass");

            TableOperation tableOperation = TableOperation.Insert(senseResult);

            await table.ExecuteAsync(tableOperation);
        }
    }
}
