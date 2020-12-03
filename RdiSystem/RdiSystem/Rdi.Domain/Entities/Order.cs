using System;
using System.Collections.Generic;
using Rdi.Core;

namespace Rdi.Domain.Entities
{
    public class Order : Entity
    {
        public static int MIN_AMOUNT_ITEM => 1;
        public int? CustomerId { get;  set; }
        public DateTime RegisterDate { get;  set; }
        public  OrderStatus Status { get; set; }
        public List<OrderItems> OrderItems { get; set; }

        public Order(List<OrderItems> orderItems, int? customerId, DateTime registerDate, OrderStatus status)
        {
            OrderItems = orderItems;
            CustomerId = customerId;
            RegisterDate = registerDate;
            Status = status;    
        }

        public Order()
        {
        }



        public static class OrderFactory
        {
            public static Order NewOrder(int customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId,
                    RegisterDate = DateTime.Now,
                    OrderItems = Entities.OrderItems.OrderItemsFactory.NewOrderItems(),
                    Id = 1
                };

                return order;
            }
        }


    }
}
