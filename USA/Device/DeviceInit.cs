﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace UCA.Devices
{
    public class DeviceInit
    {
        public readonly Dictionary<DeviceNames, IDeviceInterface> Devices = new Dictionary<DeviceNames, IDeviceInterface>();
        public readonly List<Device> DeviceList;

        public DeviceInit InitDevices()
        {
            return new DeviceInit(DeviceList);
        }

        public void CloseDevicesSerialPort(List<Device> deviceList)
        {
            foreach (var device in deviceList)
            {
                device.SerialPort.Close();
            }
        }

        public DeviceInit(List<Device> deviceList)
        {
            this.DeviceList = deviceList;
            foreach (var device in deviceList)
            {
                IDeviceInterface newDevice = null;
                switch (device.Name)
                {
                    case DeviceNames.PSP_405:
                    case DeviceNames.PSP_405_power:
                        newDevice = new PSP405_device(device.SerialPort);
                        break;
                    case DeviceNames.GDM_78261:
                        newDevice = new GDM78261_device(device.SerialPort);
                        break;
                    case DeviceNames.Keithley2401_1:
                    case DeviceNames.Keithley2401_2:
                        newDevice = new Keithley2401_device(device.SerialPort);
                        break;
                    case DeviceNames.Commutator:
                    case DeviceNames.Commutator_1:
                        newDevice = new Commutator_device(device.SerialPort);
                        break;
                    case DeviceNames.None:
                        newDevice = new None();
                        break;
                    // УСА_Т
                    case DeviceNames.PSH_73610:
                        newDevice = new PSH73610_device(device.SerialPort);
                        break;
                    case DeviceNames.PSH_73630:
                        newDevice = new PSH73610_device(device.SerialPort);
                        break;
                    case DeviceNames.PCI_1762:
                        newDevice = new PCI1762_device(device.Description);
                        break;
                    case DeviceNames.ATH_8030:
                        newDevice = new ATH8030_device(device.SerialPort.PortName);
                        break;
                    // УПД
                    case DeviceNames.GDM_78341:
                        newDevice = new GDM78261_device(device.SerialPort);
                        break;
                    case DeviceNames.PCI_1761_1:
                    case DeviceNames.PCI_1761_2:
                        newDevice = new PCI1762_device(device.Description);
                        break;
                    case DeviceNames.PCI_1762_1:
                    case DeviceNames.PCI_1762_2:
                    case DeviceNames.PCI_1762_3:
                    case DeviceNames.PCI_1762_4:
                    case DeviceNames.PCI_1762_5:
                        newDevice = new PCI1762_device(device.Description);
                        break;
                    case DeviceNames.AKIP_3407:
                        newDevice = new AKIP3407_device(device.SerialPort);
                        break;
                }
                Devices.Add(device.Name, newDevice);
            }
        }

        public DeviceResult ProcessDevice(DeviceData deviceData)
        {
            try
            {
                return Devices[deviceData.DeviceName].DoCommand(deviceData);
            }
            catch (IOException)
            {
                return DeviceResult.ResultNotConnected($"NOT_CONNECTED: Порт {deviceData.DeviceName} закрыт");
            }
            catch (InvalidOperationException)
            {
                return DeviceResult.ResultNotConnected($"NOT_CONNECTED: Порт {deviceData.DeviceName} закрыт");
            }
            /*
            catch (FormatException)
            {
                return DeviceResult.ResultError($"BAD_ARGUMENT: Аргумент команды имеет неверный формат: {deviceData.ExpectedValue}");
            }
            */
        }
    }
}