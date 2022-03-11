using Jedlix.Models.OutputModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jedlix
{
    public static class JSONConverter
    {
        public static InputData? FromString(string jsonString) {
            return JsonConvert.DeserializeObject<InputData>(jsonString);
        }
        public static string ToString(List<OutputData> outputData)
        {
            return JsonConvert.SerializeObject(outputData);
        }
    }
}
