﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using UCA.DeviceDrivers;
using UPD.DeviceDrivers;
using UCA.Devices;

namespace UPD.Device.DeviceList
{
    class Simulator_device : IDeviceInterface
    {
        readonly Simulator Simulator;

        public Simulator_device(SerialPort serialPort)
        {
            serialPort.NewLine = "\r";
            Simulator = new Simulator(serialPort);
        }

        /// дописать OpenAllRelays
        public override DeviceResult DoCommand(DeviceData deviceData)
        {
            switch (deviceData.Command)
            {
                case DeviceCommands.CloseRelays:
                    string[] closableRelays = deviceData.Argument.Replace(" ", "").Split(',');
                    Simulator.CloseRelays(closableRelays);
                    // Проверим, всё ли замкнулось:
                    var actualClosedRelayNames = Simulator.GetClosedRelayNames();
                    bool isAllTheseRelaysClosed = false;
                    string relayName = "relay";
                    foreach (var relay in closableRelays)
                    {
                        isAllTheseRelaysClosed = actualClosedRelayNames.Contains(relay);
                        if (!isAllTheseRelaysClosed)
                        {
                            relayName = relay;
                            break;
                        }
                    }
                    if (isAllTheseRelaysClosed)
                    {
                        return DeviceResult.ResultOk("Замыкание реле прошло успешно");
                    }
                    else
                    {
                        return DeviceResult.ResultError($"ОШИБКА: реле {relayName} не замкнуто");
                    }
                case DeviceCommands.OpenRelays:
                    string[] breakableRelays = deviceData.Argument.Replace(" ", "").Split(',');
                    Simulator.OpenRelays(breakableRelays);
                    // Проверим, всё ли разомкнулось:
                    var actualClosedRelayNames1 = Simulator.GetClosedRelayNames();
                    bool anyOfTheseRelaysClosed = false;
                    var closedRelayName = "relay";
                    foreach (var relay in breakableRelays)
                    {
                        anyOfTheseRelaysClosed = actualClosedRelayNames1.Contains(relay);
                        if (anyOfTheseRelaysClosed)
                        {
                            closedRelayName = relay;
                            break;
                        }
                    }
                    if (!anyOfTheseRelaysClosed)
                    {
                        return DeviceResult.ResultOk("Размыкание реле прошло успешно");
                    }
                    return DeviceResult.ResultError($"ОШИБКА: реле {closedRelayName} замкнуто");
                case DeviceCommands.OpenAllRelays:
                    string[] breakableRelays1 = new string[] { "all" };
                    Simulator.OpenRelays(breakableRelays1);
                    // Проверим, всё ли разомкнулось:
                    var actualClosedRelayNames2 = Simulator.GetClosedRelayNames();
                    bool anyOfTheseRelaysClosed2 = actualClosedRelayNames2[0] == "none";
                    if (anyOfTheseRelaysClosed2)
                    {
                        return DeviceResult.ResultOk("Размыкание реле прошло успешно");
                    }
                    return DeviceResult.ResultError("ОШИБКА: реле остались замкнутыми");
                case DeviceCommands.CheckClosedRelays:
                    string[] signalNames = deviceData.AdditionalArg.Replace(" ", "").Split(',');
                    // Проверим, всё ли разомкнулось:
                    var actualClosedRelayNames3 = Simulator.GetClosedRelayNames();
                    bool isSignalExist = false;
                    foreach (var signal in signalNames)
                    {
                        isSignalExist = actualClosedRelayNames3.Contains(signal);
                        if (!isSignalExist)
                        {
                            break;
                        }
                    }
                    if (isSignalExist)
                    {
                        return DeviceResult.ResultOk($"Сигнал {signalNames[0]} присутствует");
                    }
                    return DeviceResult.ResultError($"Сигнал {signalNames[0]} отсутствует");
                case DeviceCommands.GetSignals:
                    string[] signalNames1 = deviceData.AdditionalArg.Replace(" ", "").Split(',');
                    // Проверим, всё ли разомкнулось:
                    var actualSignals = Simulator.GetSignals();
                    bool isSignalExist1 = false;
                    foreach (var signal in signalNames1)
                    {
                        isSignalExist1 = actualSignals.Contains(signal);
                        if (!isSignalExist1)
                        {
                            break;
                        }
                    }
                    if (isSignalExist1)
                    {
                        return DeviceResult.ResultOk($"Сигнал {signalNames1[0]} присутствует");
                    }
                    return DeviceResult.ResultError($"Сигнал {signalNames1[0]} отсутствует");
                default:
                    return DeviceResult.ResultError($"Неизвестная команда {deviceData.Command}");
            }
        }
    }
}
