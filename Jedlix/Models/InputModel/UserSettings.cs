using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jedlix
{
    public class UserSettings
    {
        public int DesiredStateOfCharge;
        public DateTime LeavingTime;
        public int DirectChargingPercentage;
        public List<Tariff> Tariffs;
    }
}
