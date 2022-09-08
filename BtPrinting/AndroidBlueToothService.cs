using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Java.Util;
using XFBluetoothPrint.Droid;

namespace XFBluetoothPrint.Droid
{

    public class AndroidBlueToothService 
    {

        BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

        public IList<string> GetDeviceList()
        {

            var btdevice = bluetoothAdapter?.BondedDevices
            .Select(i => i.Name).ToList();
            return btdevice;

        }

        public void Print(string deviceName, string text)
        {

            BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                      where bd?.Name == deviceName
                                      select bd).FirstOrDefault();
            try
            {
                BluetoothSocket bluetoothSocket = device?.
                    CreateRfcommSocketToServiceRecord(
                    UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                bluetoothSocket?.Connect();
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                bluetoothSocket?.OutputStream.WriteByte(0x0A);

                bluetoothSocket.Close();


            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);

                throw exp;
            }
        }

    }
}