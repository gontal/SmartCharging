using Jedlix.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections;
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
        private static IEnumerable<object[]> GetTestTariffs()
        {
            return new List<object[]>()
            {
                 new object[]{ new List<Tariff>() { new Tariff(new DateTime(2022, 3, 4, 18, 0, 0), new DateTime(2022, 3, 4, 23, 0, 0), 10) }, 
                                                    new DateTime(2022, 3, 4, 20, 0, 0), 
                                                    new DateTime(2022, 3, 4, 9, 0, 0) },

                 new object[]{ new List<Tariff>() { new Tariff(new DateTime(2022, 3, 4, 9, 0, 0), new DateTime(2022, 3, 4, 12, 0, 0), 10),
                                                    new Tariff(new DateTime(2022, 3, 4, 18, 0, 0), new DateTime(2022, 3, 4, 23, 0, 0), 10)}, 
                                                    new DateTime(2022, 3, 4, 20, 0, 0), 
                                                    new DateTime(2022, 3, 4, 9, 0, 0) }
            };
        }

        [TestMethod]
        [DataRow(2022, 3, 4, 20, 0)]
        [DataRow(2022, 3, 4, 20, 9)]
        [DataRow(2022, 3, 4, 20, 19)]
        public void AddOneDay_IfLeavingTimeIsLessThan_StartingTime(int year, int month, int day, int startingHour, int leavingHour)
        {
            var startingTime = new DateTime(year, month, day, startingHour, 0, 0);
            var leavingTime = new DateTime(year, month, day, leavingHour, 0, 0);
            var result = engine.CalculateLeavingDate(startingTime, leavingTime);

            Assert.AreEqual(leavingTime.AddDays(1), result);
        }

        [TestMethod]
        [DataRow(2022, 3, 4, 20, 20)]
        [DataRow(2022, 3, 4, 20, 21)]
        [DataRow(2022, 3, 4, 20, 23)]
        public void LeavingDate_ShouldBeTheSameAsStartingDate_If_LeavingTimeIsMoreOrEqual_StartingTime(int year, int month, int day, int startingHour, int leavingHour)
        {
            var startingTime = new DateTime(year, month, day, startingHour, 0, 0);
            var leavingTime = new DateTime(year, month, day, leavingHour, 0, 0);
            var result = engine.CalculateLeavingDate(startingTime, leavingTime);

            Assert.AreEqual(startingTime.Date, result.Date);
        }
        [TestMethod]
        [DynamicData(nameof(GetTestTariffs), DynamicDataSourceType.Method)]
        public void ShouldDuplicateTarrifsForTheNextDate(List<Tariff> tariffs, DateTime startingDate, DateTime leavingDate)
        {
            var result = engine.GetAllTariffs(tariffs, startingDate, data.UserSettings.LeavingTime.AddDays(1));

            Assert.AreEqual(result.Count, tariffs.Count * 2);
        }


    }
}