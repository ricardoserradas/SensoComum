using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SensoComum.Shared.Models;
using SensoComum.Shared.Tables;

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
            CommonSenseResult currentResult = await new ValueTable(
                this.configuration["AppSettings:ConnectionStrings:StorageTableConnection"]
                , this.configuration["AppSettings:TableName"]
                ).InitializeIfNotExists()
                .Result.RetrieveValue("JeepCompass", "JeepCompass");

            return currentResult.Sum.ToString();
        }
    }
}
