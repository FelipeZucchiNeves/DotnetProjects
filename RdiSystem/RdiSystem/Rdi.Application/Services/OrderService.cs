using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rdi.Application.Services.Areas;
using Rdi.Core;
using Rdi.Data.OrderRepository;
using Rdi.Domain.Entities;

namespace Rdi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IKitchenAreasRepository _kar;


        public OrderService(IKitchenAreasRepository kar)
        {
            _kar = kar;
        }

        public async Task<bool> ExecuteOrder(Order order)
        {
            try
            {
                if (order.OrderItems is null || !order.OrderItems.Any()) return false;

                var areas = await MappingAreas(order);
                var result = DistributeOrderItens(areas);

                return result;
            }
            catch
            {
                throw new DomainException("Something wrong to execute order");
            }
        }


        private async Task<List<OrderItems>> MappingAreas(Order order)
        {
            try
            {
                var areas = await _kar.GetKitchenAreas();

                return order.OrderItems.Join(areas,
                    x => x.ProductArea,
                    y => y.Name,
                    (x, y) => new OrderItems
                    {
                        OrderId = x.Id,
                        ProductName = x.ProductName,
                        Amount = x.Amount,
                        ProductId = x.ProductId,
                        AreaId = y.Id
                    }).ToList();
            }
            catch (Exception e)
            {
                throw new DomainException(e.Message);
            }
        }

        private bool DistributeOrderItens(List<OrderItems> areas)
        {
            var drinkAreaHandle = new DrinkAreaHandle();
            var saladAreaHandle = new SaladAreaHandle();
            var grillAreaHandle = new GrillAreaHandle();
            var friesAreaHandle = new FriesAreaHandle();
            var desertAreaHandle = new DesertAreaHandle();

            drinkAreaHandle.SetNextHandler(saladAreaHandle);
            saladAreaHandle.SetNextHandler(grillAreaHandle);
            grillAreaHandle.SetNextHandler(friesAreaHandle);
            friesAreaHandle.SetNextHandler(desertAreaHandle);

            try
            {
                drinkAreaHandle.ProcessArea(areas);
                return true;
            }
            catch
            {
                throw new DomainException("Something wrong to distribute items order");
            }
        }
    }
}