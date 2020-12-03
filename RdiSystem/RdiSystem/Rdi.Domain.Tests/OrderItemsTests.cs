using System;
using Rdi.Core;
using Rdi.Domain.Entities;
using Xunit;

namespace Rdi.Domain.Tests
{
    public class OrderItemsTests
    {
        [Fact(DisplayName = "New Item Order with units below allowed")]
        [Trait("Category", "Sales - Order Items")]
        public void AddItemOrder_UnitsBelowAllowed_MustReturnException()
        {

            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new OrderItems(1, 1, "hamburger", "fries", Order.MIN_AMOUNT_ITEM-1, 1));
        }
    }
    
}
