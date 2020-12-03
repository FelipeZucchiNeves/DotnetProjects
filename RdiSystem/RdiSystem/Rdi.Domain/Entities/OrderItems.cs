using System.Collections.Generic;
using Rdi.Core;

namespace Rdi.Domain.Entities
{
    public class OrderItems : Entity
    {
        public int OrderId { get;  set; }
        public int ProductId { get;  set; }
        public string ProductName { get;  set; }
        public string ProductArea { get; set; }
        public int AreaId { get; set; }
        public int Amount { get;  set; }

        public OrderItems(int orderId, int productId, string productName, string productArea, int amount, int area)
        {
            if(amount < Order.MIN_AMOUNT_ITEM) throw new DomainException($"Minimum {Order.MIN_AMOUNT_ITEM} unit/product");

            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            ProductArea = productArea;
            AreaId = area;
            Amount = amount;
        }

        public OrderItems()
        {
        }


        public static class OrderItemsFactory
        {
            public static List<OrderItems> NewOrderItems()
            {
                var items = new List<OrderItems>
                {
                    new OrderItems
                    {
                        ProductId = 1,
                        Amount = 1,
                        Id = 1,
                        AreaId = 1,
                        ProductName = "Hamburger",
                        OrderId = 1,
                        ProductArea = "fries"
                    },
                    new OrderItems
                    {
                        ProductId = 1,
                        Amount = 1,
                        Id = 1,
                        AreaId = 1,
                        ProductName = "Hamburger",
                        OrderId = 1,
                        ProductArea = "fries"
                    },
                    new OrderItems
                    {
                        ProductId = 1,
                        Amount = 1,
                        Id = 1,
                        AreaId = 1,
                        ProductName = "Hamburger",
                        OrderId = 1,
                        ProductArea = "fries"
                    }
                };

                return items;
            }
        }

    }
}
