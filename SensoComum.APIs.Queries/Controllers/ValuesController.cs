using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SensoComum.APIs.Queries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IConfiguration configuration;

        public ValuesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.configuration["AppSettings:ConnectionStrings:StorageTableConnection"]);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(this.configuration["AppSettings:TableName"]);

            bool tableExists = await table.CreateIfNotExistsAsync();

            if (tableExists)
            {
                await InitializeCommonSense(table);
            }

            CommonSenseResult currentResult = await RetrieveCommonSenseValue(table);

            return currentResult.Sum.ToString();
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

        private static async Task<CommonSenseResult> InitializeCommonSense(CloudTable table)
        {
            CommonSenseResult senseResult = new CommonSenseResult("JeepCompass");

            TableOperation tableOperation = TableOperation.Insert(senseResult);

            await table.ExecuteAsync(tableOperation);

            return senseResult;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
