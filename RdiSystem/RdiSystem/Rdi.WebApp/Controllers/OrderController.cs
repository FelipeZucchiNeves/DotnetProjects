
using Microsoft.AspNetCore.Mvc;
using Rdi.Application.Services;
using Rdi.Domain;

using System.Threading.Tasks;
using Rdi.Domain.Entities;

namespace Rdi.WebApp.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("api/execute_order")]
        public async Task<IActionResult> Post([FromBody] Order order)
        {

            if (order is null) return BadRequest();

            await _orderService.ExecuteOrder(order);

            return Ok();
        }
    }
}
