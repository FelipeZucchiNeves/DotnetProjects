
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq.AutoMock;
using Rdi.Application.Services;
using Rdi.Data.OrderRepository;
using Rdi.Domain.Entities;
using Xunit;

namespace Rdi.Domain.Tests
{
    public class OrderTests
    {
        private readonly AutoMocker _mocker;
        private readonly OrderService _orderService;

        public OrderTests()
        {
            _mocker = new AutoMocker();
            _orderService = _mocker.CreateInstance<OrderService>();   
        }

        [Fact(DisplayName = "Flow Validation")]
        [Trait("Category", "Sales - Order")]
        public void ValidateFlow_ExecuteProcess_MustExecuteWithSuccess()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrder(1);
            _mocker.GetMock<IKitchenAreasRepository>()
                .Setup(r => r.GetKitchenAreas())
                .Returns(Task.FromResult<List<KitchenAreas>>(new List<KitchenAreas>{
                new KitchenAreas(1, "fries"),
                new KitchenAreas(2, "grill"),
                new KitchenAreas(3, "salad"),
                new KitchenAreas(4, "drink"),
                new KitchenAreas(5, "desert")

                }));

            // Act
            var result = _orderService.ExecuteOrder(order).Result;
            

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Flow Validation")]
        [Trait("Category", "Sales - Order")]
        public void ValidateFlow_ExecuteProcess_MustExecuteWithoutSuccess()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrder(1);
            order.OrderItems = null;
            _mocker.GetMock<IKitchenAreasRepository>()
                .Setup(r => r.GetKitchenAreas())
                .Returns(Task.FromResult<List<KitchenAreas>>(new List<KitchenAreas>{
                    new KitchenAreas(1, "fries"),
                    new KitchenAreas(2, "grill"),
                    new KitchenAreas(3, "salad"),
                    new KitchenAreas(4, "drink"),
                    new KitchenAreas(5, "desert")

                }));

            // Act
            var result = _orderService.ExecuteOrder(order).Result;


            // Assert
            Assert.False(result);
        }
    }
}
