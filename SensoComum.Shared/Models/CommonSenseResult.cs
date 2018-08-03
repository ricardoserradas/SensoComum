using Microsoft.WindowsAzure.Storage.Table;

namespace SensoComum.Shared.Models
{
    public class CommonSenseResult : TableEntity
    {
        public CommonSenseResult(string name)
        {
            this.PartitionKey = name;
            this.RowKey = name;
        }

        public CommonSenseResult()
        {

        }

        public int Sum { get; set; }

        public void Update(int value)
        {
            this.Sum += value;
        }
    }
}