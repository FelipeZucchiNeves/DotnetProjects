using System.Collections.Generic;
using Rdi.Domain.Entities;
using Rdi.Domain.Enum;

namespace Rdi.Application.Services.Areas
{
    public class GrillAreaHandle : BaseHandler
    {
        public Queue<OrderItems> Grill { get; set; }

        public override void ProcessArea(IEnumerable<OrderItems> items)
        {
            Grill = EnqueueOrders(items, KitchenAreasEnum.grill);
            if (_nextHandler != null) _nextHandler.ProcessArea(items);
        }
    }
}