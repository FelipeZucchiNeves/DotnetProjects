using System.Collections.Generic;
using System.Threading.Tasks;
using Moq.AutoMock;
using Rdi.Application.Services;
using Rdi.Application.Services.Areas;
using Rdi.Data.OrderRepository;
using Rdi.Domain;
using Rdi.Domain.Entities;
using Rdi.Domain.Enum;
using Xunit;

namespace Rdi.Application.Tests
{
    public class HandlerAreasTests
    {
        private readonly AutoMocker _mocker;
        private readonly BaseHandler _handler;

        public HandlerAreasTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<BaseHandler>();
        }

        [Fact(DisplayName = "BaseHandler Validation")]
        [Trait("Category", "Application - Services")]
        public void BaseHandler_EnqueueOrders_MustExecuteWithSuccess()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrder(1);

            // Act

            var result =_handler.EnqueueOrders(order.OrderItems, KitchenAreasEnum.fries);

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact(DisplayName = "BaseHandler Validation")]
        [Trait("Category", "Application - Services")]
        public void BaseHandler_ExecuteProcess_MustExecuteWithoutSuccess()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrder(1);
            order.OrderItems[1].AreaId = 2;

            // Act

            var result = _handler.EnqueueOrders(order.OrderItems, KitchenAreasEnum.fries);

            // Assert
            Assert.False(result.Count == 3);
        }

        [Fact(DisplayName = "Chain of Responsibility Validation")]
        [Trait("Category", "Application - Services")]
        public void Handler_DistributeOrders_MustExecuteWithSuccess()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrder(1);
            order.OrderItems.Add(new OrderItems(1, 2, "Salad", "salad", 1, 3));
            order.OrderItems.Add(new OrderItems(1, 3, "lettuce", "salad", 1,3));
            order.OrderItems.Add(new OrderItems(1, 4, "Ice Cream", "desert", 1,5));
            order.OrderItems.Add(new OrderItems(1, 5, "Cheese Burger", "grill", 1,2));
            order.OrderItems.Add(new OrderItems(1, 5, "Cheese Burger", "grill", 1,2));
            order.OrderItems.Add(new OrderItems(1, 6, "DietCoke", "drink", 1,4));
            order.OrderItems.Add(new OrderItems(1, 6, "Soda", "drink", 1,4));

            var dah = new DrinkAreaHandle();
            var sah = new SaladAreaHandle();
            var gah = new GrillAreaHandle();
            var fah = new FriesAreaHandle();
            var deh = new DesertAreaHandle();


            // Act

            var drink = dah.EnqueueOrders(order.OrderItems, KitchenAreasEnum.drink);
            var salad = sah.EnqueueOrders(order.OrderItems, KitchenAreasEnum.salad);
            var grill = gah.EnqueueOrders(order.OrderItems, KitchenAreasEnum.grill);
            var fries = fah.EnqueueOrders(order.OrderItems, KitchenAreasEnum.fries);
            var desert = deh.EnqueueOrders(order.OrderItems, KitchenAreasEnum.desert);

            // Assert

            Assert.Equal(2, drink.Count);
            Assert.Equal(2, salad.Count);
            Assert.Equal(2, grill.Count);
            Assert.Equal(3, fries.Count);
            Assert.Equal(2, drink.Count);

        }
    }
}
