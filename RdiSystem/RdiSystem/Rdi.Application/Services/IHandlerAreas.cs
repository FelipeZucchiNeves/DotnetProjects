using System.Collections.Generic;
using Rdi.Domain.Entities;

namespace Rdi.Application.Services
{
    public interface IHandlerAreas
    {
        void ProcessArea(IEnumerable<OrderItems> items);
        void SetNextHandler(IHandlerAreas handler);
    }
}