using System.Collections.Generic;
using Rdi.Domain.Entities;
using Rdi.Domain.Enum;

namespace Rdi.Application.Services.Areas
{
    public class SaladAreaHandle : BaseHandler
    {
        public Queue<OrderItems> Salad { get; set; }

        public override void ProcessArea(IEnumerable<OrderItems> items)
        {
            Salad = EnqueueOrders(items, KitchenAreasEnum.salad);
            if (_nextHandler != null) _nextHandler.ProcessArea(items);
        }
    }
}