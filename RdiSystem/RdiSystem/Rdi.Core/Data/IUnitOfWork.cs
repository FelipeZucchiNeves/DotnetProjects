using System.Threading.Tasks;

namespace Rdi.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}