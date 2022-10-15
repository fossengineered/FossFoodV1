using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
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
            .Select(i => i.Address).ToList();
            return btdevice;

        }

        public void BulkPrint(List<string> text, Action<string> toast)
        {
            try
            {
                List<BluetoothDevice> devices = (from bd in bluetoothAdapter?.BondedDevices select bd).ToList();

                List<BluetoothSocket> sockets = devices.Select(d => d?.
                        CreateRfcommSocketToServiceRecord(
                        UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"))).ToList();

                //BluetoothSocket bluetoothSocket = device?.
                //        CreateRfcommSocketToServiceRecord(
                //        UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                sockets.ForEach(s => s?.Connect());

                //toast("Sockets: " + sockets.Count);

                //bluetoothSocket?.Connect();

                foreach (var line in text)
                {
                    sockets.ForEach(s =>
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(line);
                        s?.OutputStream.Write(buffer, 0, buffer.Length);
                        s?.OutputStream.WriteByte(0x0A);
                        Thread.Sleep(100);
                    });

                }

                sockets.ForEach(s => {                    
                    s?.OutputStream.WriteByte(0x0A);
                    s?.OutputStream.WriteByte(0x0A);
                    s?.OutputStream.WriteByte(0x0A);
                    s?.OutputStream.WriteByte(0x0A);
                    s?.OutputStream.WriteByte(0x0A);

                    Thread.Sleep(100);
                });

                sockets.ForEach(s => s?.Close());
                // bluetoothSocket.Close();
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);

                throw exp;
            }
        }


        public void PrintX(string deviceId, string text)
        {

            BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                      where bd?.Address == deviceId
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