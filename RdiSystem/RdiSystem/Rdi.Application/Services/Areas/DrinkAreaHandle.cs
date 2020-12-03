using System.Collections.Generic;
using Rdi.Domain.Entities;
using Rdi.Domain.Enum;

namespace Rdi.Application.Services.Areas
{
    public class DrinkAreaHandle : BaseHandler
    {
        public Queue<OrderItems> Drink { get; set; }

        public override void ProcessArea(IEnumerable<OrderItems> items)
        {
            Drink = EnqueueOrders(items, KitchenAreasEnum.drink);
            if (_nextHandler != null) _nextHandler.ProcessArea(items);
        }
    }
}