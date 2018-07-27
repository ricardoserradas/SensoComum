using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace SensoComum.APIs.Commands.Controllers
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
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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
            // https://github.com/Azure-Samples/storage-queue-dotnet-pop-receipt/blob/master/dotnet/storage-queue-dotnet-popreceipt/Program.cs
            // https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues#create-a-queue
            // https://stackoverflow.com/questions/30575689/how-do-we-use-cloudconfigurationmanager-with-asp-net-5-json-configs#30580006

            CloudStorageAccount storageAccount = this.configuration.Get(typeof(string), ;
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
