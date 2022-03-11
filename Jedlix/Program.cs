using Jedlix;
using Jedlix.Controller;
using Jedlix.Models.OutputModel;

var inputJSONString = IOHandler.ReadFileToString("InputData.json");
InputData? data = JSONConverter.FromString(inputJSONString);

GreedyEngine engine = new GreedyEngine();
var results = engine.CalculateCharging(data);

var output = JSONConverter.ToString(results);
IOHandler.WriteFileFromString(output);

foreach (var result in results)
{
    Console.WriteLine("{0} , {1} , {2}", result.StartTime, result.EndTime, result.IsCharging);
}