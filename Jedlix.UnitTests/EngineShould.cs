using Jedlix.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace Jedlix.UnitTests
{
    [TestClass]
    public class EngineShould
    {
        InputData data;
        GreedyEngine engine;

        [TestInitialize]
        public void TestInitialize()
        {
            data = new InputData()
            {
                StartingTime = new DateTime(2022, 3, 4, 20, 0, 0),
                UserSettings = new UserSettings()
                {
                    DesiredStateOfCharge = 80,
                    LeavingTime = new DateTime(2022, 3, 4, 09, 0, 0),
                    DirectChargingPercentage = 40,
                    Tariffs = new List<Tariff>()
                    {
                        new Tariff(new DateTime(2022, 3, 4, 18, 0, 0), new DateTime(2022, 3, 4, 23, 0, 0), 10),
                        new Tariff(new DateTime(2022, 3, 4, 16, 0, 0), new DateTime(2022, 3, 4, 18, 0, 0), 10)
                    }
                },
                CarData = new CarData()
                {
                    ChargePower = 10,
                    BatteryCapacity = 100,
                    CurrentBatteryLevel = 20
                }
            };

            engine = new GreedyEngine();
        }

        [TestMethod]
        public void AddOneDayIfLeavingTimeIsLessThanStartingTime()
        {
            var result = engine.CalculateLeavingDate(data.StartingTime, data.UserSettings.LeavingTime);

            Assert.AreEqual(data.UserSettings.LeavingTime.AddDays(1), result);
        }
        [TestMethod]
        public void GetsTarifsForTwoDays()
        {
            var result = engine.GetAllTariffs(data.UserSettings.Tariffs, data.StartingTime, data.UserSettings.LeavingTime.AddDays(1));

            Assert.AreEqual(result.Count, 4);
        }

    }
}