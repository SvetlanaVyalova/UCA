﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UCA.Devices;
using UCA.DeviceDrivers;
using System.Collections.Generic;
using System.Threading;
using UPD.Device;
using static UCA.Auxiliary.UnitValuePair;
using UPD.DeviceDrivers;
using UPD.Device.DeviceList;
using static UPD.DeviceDrivers.MK;

namespace TestProject
{
    [TestClass]

    public class UnitTest1
    {
        enum PowerState
        {
            OFF,
            ON
        }

        #region ASBL


        #endregion

        #region MK

        [TestMethod]
        public void ParseRelayNumbers()
        {
            int[] expected = new int[] { 1, 2, 3, 4, 54 };
            var actual = MK_device.ParseRelayNumbers("1, 2, 3, 4, 54");
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseRelayNumbers_oneRelay()
        {
            int[] expected = new int[] { 1 };
            var actual = MK_device.ParseRelayNumbers("1 ");
            CollectionAssert.AreEqual(expected, actual);
        }


        #endregion

        #region Песочница

        [TestMethod]
        public void IsEnumEqualsString()
        {
            var state = PowerState.OFF;
            Assert.AreEqual(state.ToString(), "OFF");
            Assert.AreEqual(Convert.ToInt32(state), 0);
        }

        #endregion

        #region GDM saving and getting values
        [TestMethod]
        public void SaveValue()
        {
            IDeviceInterface.AddValues("1", 0.111);
            IDeviceInterface.AddValues("2", 0.222);
            IDeviceInterface.AddValues("3", 0.333);
            IDeviceInterface.AddValues("4", 0.444);
            IDeviceInterface.AddValues("5", 0.555);
            IDeviceInterface.AddValues("6", 0.666);
            IDeviceInterface.AddValues("7", 0.777);
            var expectedSavedValues = new Dictionary<string, double>()
            {
                { "1", 0.111 },
                { "2", 0.222 },
                { "3", 0.333 },
                { "4", 0.444 },
                { "5", 0.555 },
                { "6", 0.666 },
                { "7", 0.777 }

            };
            var actual = IDeviceInterface.GetValuesDictionary();
            CollectionAssert.AreEqual(expectedSavedValues, actual);
        }
        [TestMethod]
        public void SaveValue_equalKeys()
        {
            IDeviceInterface.AddValues("1", 0.111);
            IDeviceInterface.AddValues("2", 0.222);
            IDeviceInterface.AddValues("3", 0.333);
            IDeviceInterface.AddValues("4", 0.444);
            IDeviceInterface.AddValues("5", 0.555);
            IDeviceInterface.AddValues("6", 0.666);
            IDeviceInterface.AddValues("7", 0.777);
            IDeviceInterface.AddValues("5", 0.777);
            var expectedSavedValues = new Dictionary<string, double>()
            {
                { "1", 0.111 },
                { "2", 0.222 },
                { "3", 0.333 },
                { "4", 0.444 },
                { "5", 0.777 },
                { "6", 0.666 },
                { "7", 0.777 }

            };
            CollectionAssert.AreEqual(expectedSavedValues, IDeviceInterface.GetValuesDictionary());
        }

        [TestMethod]
        public void GetValue()
        {
            IDeviceInterface.AddValues("1", 0.111);
            IDeviceInterface.AddValues("2", 0.222);
            IDeviceInterface.AddValues("3", 0.333);
            IDeviceInterface.AddValues("4", 0.444);
            IDeviceInterface.AddValues("5", 0.555);
            IDeviceInterface.AddValues("6", 0.666);
            IDeviceInterface.AddValues("7", 0.777);
            var expectedSavedValues = new Dictionary<string, double>()
            {
                { "1", 0.111 },
                { "2", 0.222 },
                { "3", 0.333 },
                { "4", 0.444 },
                { "5", 0.555 },
                { "6", 0.666 },
                { "7", 0.777 }

            };
            foreach (var key in expectedSavedValues.Keys)
            {
                Assert.AreEqual(expectedSavedValues[key], IDeviceInterface.GetValue(key));
            }
        }

        #endregion

        #region None Tests

        [TestMethod]
        public void GetKeys()
        {
            var actual = None.GetKeys("1, 2; V");
            var expected = new ValueKeys (UnitType.Voltage, new string[] { "1", "2"});
            Assert.ReferenceEquals(expected, actual);
        }

        [TestMethod]
        public void GetKeys_oneKey()
        {
            var actual = None.GetKeys("1; A");
            var expected = new ValueKeys(UnitType.Frequency, new string[] { "1" });
            Assert.ReferenceEquals(expected, actual);
        }

        /*        [TestMethod]
                public void Divide()
                {
                    var actual = None.Divide(4.5, 1.5);
                    var expected = 3.0;
                    Assert.AreEqual(expected, actual);
                }*/

        #endregion

        #region ATH_8030

        #endregion
        
    }
}
