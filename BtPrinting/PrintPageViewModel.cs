using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using XFBluetoothPrint.Droid;

namespace XFBluetoothPrint
{
    public class PrintPageViewModel
    {
        private readonly AndroidBlueToothService _blueToothService;

        private IList<string> _deviceList;
        public IList<string> DeviceList
        {
            get
            {
                if (_deviceList == null)
                    _deviceList = new ObservableCollection<string>();
                return _deviceList;
            }
            set
            {
                _deviceList = value;
            }
        }

        private string _printMessage;
        public string PrintMessage
        {
            get
            {
                return _printMessage;
            }
            set
            {
                _printMessage = value;
            }
        }

        private string _selectedDevice;
        public string SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
            }
        }

        //public ICommand PrintCommand => new Command(async () =>
        //{
        //    PrintMessage += " Xamarin Forms is awesome!";
        //    await _blueToothService.Print(SelectedDevice, PrintMessage);
        //});

        public PrintPageViewModel()
        {
            _blueToothService = new AndroidBlueToothService();

            var list = _blueToothService.GetDeviceList();
            DeviceList.Clear();
            foreach (var item in list)
            {
                DeviceList.Add(item);
            }
        }

    }
}