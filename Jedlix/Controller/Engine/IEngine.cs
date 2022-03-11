using Jedlix.Models.OutputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jedlix.Controller
{
    public interface IEngine
    {
        public List<OutputData> CalculateCharging(InputData data);
    }
}
