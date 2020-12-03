using System.Collections.Generic;
using System.Threading.Tasks;
using Rdi.Data.OrderRepository;
using Rdi.Domain;

namespace Rdi.Data.Repository
{
    public class KitchenAreasRepository : IKitchenAreasRepository
    {
        public async Task<List<KitchenAreas>> GetKitchenAreas()
        {
            return new List<KitchenAreas>
            {
                new KitchenAreas(1, "fries"),
                new KitchenAreas(2, "grill"),
                new KitchenAreas(3, "salad"),
                new KitchenAreas(4, "drink"),
                new KitchenAreas(5, "desert")
            };
        }
    }
}