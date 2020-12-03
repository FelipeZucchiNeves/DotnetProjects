using System.Collections.Generic;
using System.Threading.Tasks;
using Rdi.Domain;

namespace Rdi.Data.OrderRepository
{
    public interface IKitchenAreasRepository
    {
        Task<List<KitchenAreas>> GetKitchenAreas();
    }
}