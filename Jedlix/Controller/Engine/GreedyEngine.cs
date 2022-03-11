using Jedlix.Models.OutputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jedlix.Controller
{
    public class GreedyEngine : IEngine
    {
        public List<OutputData> CalculateCharging(InputData data)
        {
            DateTime startingDate = data.StartingTime;
            DateTime leavingDate = CalculateLeavingDate(startingDate, data.UserSettings.LeavingTime);
            List<Tariff> tariffs = data.UserSettings.Tariffs;

            decimal desiredStateKW = data.UserSettings.DesiredStateOfCharge * data.CarData.BatteryCapacity / 100;
            decimal directChargingKW = data.UserSettings.DirectChargingPercentage * data.CarData.BatteryCapacity / 100;

            //Get Relevant Tarifs and calculate their time in seconds
            List<Tariff> relevantTariffs = CalculateRelevantTariffs(tariffs, startingDate, leavingDate);
            CalculateTariffsTime(relevantTariffs);

            //Calculate charging time in seconds
            decimal directChargingTime = GetChargingTimeInSeconds(
                data.CarData.CurrentBatteryLevel, directChargingKW, data.CarData.BatteryCapacity, data.CarData.ChargePower);

            //Sort tarifs by Date and make Direct Charging
            relevantTariffs.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));
            Charge(ref relevantTariffs, ref directChargingTime);

            //Sort tarifs by Price and make Smart Charging
            relevantTariffs.Sort((x, y) => decimal.Compare(x.EnergyPrice, y.EnergyPrice));
            var batteryLevelAfterDirectCharging = Math.Max(directChargingKW, data.CarData.CurrentBatteryLevel);

            decimal smartChargingTime = GetChargingTimeInSeconds(
               batteryLevelAfterDirectCharging, desiredStateKW, data.CarData.BatteryCapacity, data.CarData.ChargePower);
            Charge(ref relevantTariffs, ref smartChargingTime);

            //Calculate Output data based on used tariffs, then sort the result by Date
            List<OutputData> result = CalculateOutputResult(relevantTariffs);
            result.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));

            return result;
        }
        public DateTime CalculateLeavingDate(DateTime startingDate, DateTime leavingTime)
        {
            DateTime leavingDate = startingDate.Date + leavingTime.TimeOfDay;

            if (startingDate > leavingDate)
            {
                leavingDate = leavingDate.AddDays(1);
            }
            return leavingDate;
        }
        public decimal GetChargingTimeInSeconds(decimal currentLevel, decimal desiredLevel, decimal batteryCapacity, decimal chargePower)
        {
            if (currentLevel >= desiredLevel) return 0;
            return (desiredLevel - currentLevel) * 3600 / chargePower;
        }
        public List<Tariff> GetAllTariffs(List<Tariff> tariffs, DateTime startingDate, DateTime leavingDate)
        {
            List<Tariff> tariffsForFirstDay = new List<Tariff>();
            foreach (var tariff in tariffs)
            {
                var startDate = startingDate.Date + tariff.StartTime.TimeOfDay;
                var endDate = startingDate.Date + tariff.EndTime.TimeOfDay;

                if (startDate.TimeOfDay > endDate.TimeOfDay)
                {
                    endDate = endDate.AddDays(1);
                }

                tariffsForFirstDay.Add(new Tariff(startDate, endDate, tariff.EnergyPrice));
            }

            List<Tariff> tariffsForSecondDay = new List<Tariff>();
            if (leavingDate.Date != startingDate.Date)
            {
                foreach (var tariff in tariffsForFirstDay)
                {
                    tariffsForSecondDay.Add(new Tariff(tariff.StartTime.AddDays(1), tariff.EndTime.AddDays(1), tariff.EnergyPrice));
                }
            }

            return tariffsForFirstDay.Concat(tariffsForSecondDay).ToList();
        }

        public List<Tariff> CalculateRelevantTariffs(List<Tariff> tariffs, DateTime startingDate, DateTime leavingDate)
        {
            List<Tariff> allTariffs = GetAllTariffs(tariffs, startingDate, leavingDate);

            List<Tariff> relevantTariffs = new List<Tariff>();
            foreach(var tariff in allTariffs)
            {
                if (startingDate > tariff.EndTime) continue;
                if (leavingDate < tariff.StartTime) continue;

                relevantTariffs.Add(tariff);
            }
            foreach(var tariff in relevantTariffs)
            {
                if (startingDate > tariff.StartTime)
                {
                    tariff.StartTime = startingDate;
                }

                if (leavingDate < tariff.EndTime)
                {
                    tariff.EndTime = leavingDate;
                }
            }

            return relevantTariffs;
        }
        public void Charge(ref List<Tariff> relevantTariffs, ref decimal chargingTime)
        {
            foreach (var tariff in relevantTariffs)
            {
                if (chargingTime <= 0) return;

                if (tariff.SecondsLeft > 0)
                {
                    tariff.IsCharging = true;

                    if (tariff.SecondsLeft <= chargingTime)
                    {
                        chargingTime -= tariff.SecondsLeft;
                        tariff.SecondsLeft = 0;
                    }
                    else
                    {
                        tariff.SecondsLeft -= chargingTime;
                        chargingTime = 0;
                        return;
                    }
                }
            }
        }
        public void CalculateTariffsTime(List<Tariff> tariffs)
        {
            foreach(var tariff in tariffs)
            {
                tariff.CalculateTariffTime();
            }
        }
        public List<OutputData> CalculateOutputResult(List<Tariff> tariffs)
        {
            List<OutputData> result = new List<OutputData>();
            foreach (var tariff in tariffs)
            {
                DateTime endTime = (tariff.IsCharging) ?
                    tariff.EndTime.AddSeconds(Convert.ToDouble(-tariff.SecondsLeft)) 
                    : tariff.EndTime;
 
                result.Add(new OutputData(tariff.StartTime, endTime, tariff.IsCharging));
            }
            return result;
        }

    }
}
