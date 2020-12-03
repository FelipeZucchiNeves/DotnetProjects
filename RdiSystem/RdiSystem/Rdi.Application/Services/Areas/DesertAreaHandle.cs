using System.Collections.Generic;
using Rdi.Domain.Entities;
using Rdi.Domain.Enum;

namespace Rdi.Application.Services.Areas
{
    public class DesertAreaHandle : BaseHandler
    {
        public Queue<OrderItems> Desert { get; set; }

        public override void ProcessArea(IEnumerable<OrderItems> items)
        {
            Desert = EnqueueOrders(items, KitchenAreasEnum.desert);
            if (_nextHandler != null) _nextHandler.ProcessArea(items);
        }
    }
}