using System;
using System.Collections.Generic;
using System.Linq;
using Rdi.Domain.Entities;
using Rdi.Domain.Enum;

namespace Rdi.Application.Services.Areas
{
    public class BaseHandler : IHandlerAreas
    {
        protected IHandlerAreas _nextHandler;

        public BaseHandler()
        {
            _nextHandler = null;
        }


        public virtual void ProcessArea(IEnumerable<OrderItems> items)
        {
            throw new NotImplementedException();
        }

        public void SetNextHandler(IHandlerAreas handler)
        {
            _nextHandler = handler;
        }

        public Queue<OrderItems> EnqueueOrders(IEnumerable<OrderItems> items, KitchenAreasEnum areaId)
        {
            using var en = items.Where(i => i.AreaId.Equals((int) areaId)).GetEnumerator();
            var queue = new Queue<OrderItems>();
            while (en.MoveNext()) queue.Enqueue(en.Current);
            return queue;
        }
    }
}