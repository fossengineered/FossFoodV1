using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FossFoodV1.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FossFoodV1.OrderManager
{
    internal class OrderManagerEntity
    {
        DateTime _serviceDate;
        OrderManagerRepoSqlite _repoSqlite;

        public List<OrderManagerOrders> OpenOrders { get => _repoSqlite.GetOpenOrders(_serviceDate); }
        public List<OrderManagerOrders> CloseOrders { get => _repoSqlite.GetOpenClosed(_serviceDate); }

        public OrderManagerEntity(DateTime serviceDate)
        {
            _serviceDate = serviceDate;
            _repoSqlite = new OrderManagerRepoSqlite(serviceDate);
        }

        internal void AddNewOrder(List<OrderWithToppings> orderWithToppings, OrderCustomerDetails customerDetails)
        {
            _repoSqlite.AddNewOrder(new OrderManagerOrders
            {
                CustomerName = customerDetails.CustomerName,
                ServiceDateId = int.Parse(_serviceDate.ToString("yyyyMMdd")),
                RowStatus = RowStatus.Open,
                PagerNumber = customerDetails.PagerNumber,
                OrderData = JsonConvert.SerializeObject(orderWithToppings),
                CreatedOn = DateTime.Now
            });
        }

        internal void CloseOrder(int orderId) => _repoSqlite.CloseOrder(orderId);

        internal void ReOpenOrder(int orderId) => _repoSqlite.ReOpenOrder(orderId);
    }
}