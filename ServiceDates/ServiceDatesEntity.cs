using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.ServiceDates
{
    internal class ServiceDatesEntity
    {
        static DateTime? _currentServiceDate;

        public DateTime Current
        {
            get =>
                new DateTime(_currentServiceDate.Value.Year, _currentServiceDate.Value.Month, _currentServiceDate.Value.Day);
        }

        public ServiceDatesEntity()
        {
            if(_currentServiceDate == null) _currentServiceDate= DateTime.Now;
        }
        
    }
}