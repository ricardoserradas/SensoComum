using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using SensoComum.Shared.Queues;

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
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            if(this.configuration["AppSettings:ConnectionStrings:StorageQueueConnection"] == string.Empty)
            {
                throw new ArgumentNullException("You must first fill in your Storage Queue Connection string and Queue Name. See more: https://docs.microsoft.com/pt-br/azure/storage/common/storage-account-manage#access-keys");
            }

            new ValueQueue(
                this.configuration["AppSettings:ConnectionStrings:StorageQueueConnection"],
                this.configuration["AppSettings:QueueName"]
                )
                .AddMessage(value);
        }
    }
}
