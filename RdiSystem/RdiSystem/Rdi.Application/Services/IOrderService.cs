using System.Threading.Tasks;
using Rdi.Domain.Entities;

namespace Rdi.Application.Services
{
    public interface IOrderService
    {
        Task<bool> ExecuteOrder(Order order);
    }
}