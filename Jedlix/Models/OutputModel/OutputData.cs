using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jedlix.Models.OutputModel
{
    public class OutputData
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime;
        public bool IsCharging;

        public OutputData(DateTime startTime, DateTime endTime, bool IsCharging)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.IsCharging = IsCharging;
        }
    }
}
