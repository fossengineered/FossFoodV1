using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XFBluetoothPrint
{
    public interface IBlueToothService
    {
        IList<string> GetDeviceList();
        Task Print(string deviceName, string text);
    }
}