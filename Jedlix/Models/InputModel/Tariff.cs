using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jedlix
{
    public class Tariff
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public decimal EnergyPrice;

        [JsonIgnore]
        internal decimal TotalSeconds;
        internal decimal SecondsLeft;
        internal bool IsCharging;

        public Tariff(DateTime StartTime, DateTime EndTime, decimal EnergyPrice)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.EnergyPrice = EnergyPrice;
        }

        public void CalculateTariffTime()
        {
            this.TotalSeconds = Convert.ToDecimal(System.Math.Abs((this.EndTime - this.StartTime).TotalSeconds));
            this.SecondsLeft = this.TotalSeconds;
        }
    }
}
