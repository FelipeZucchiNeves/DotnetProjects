using System.Collections.Generic;
using Rdi.Domain.Entities;
using Rdi.Domain.Enum;

namespace Rdi.Application.Services.Areas
{
    public class FriesAreaHandle : BaseHandler
    {
        public Queue<OrderItems> Fries { get; set; }

        public override void ProcessArea(IEnumerable<OrderItems> items)
        {
            Fries = EnqueueOrders(items, KitchenAreasEnum.fries);
            if (_nextHandler != null) _nextHandler.ProcessArea(items);
        }
    }
}