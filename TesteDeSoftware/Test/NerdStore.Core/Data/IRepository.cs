using NerdStore.Core.DominObjects;
using System;

namespace NerdStore.Core.Data
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        IUnitOfWork UnitOfWork { get; }
    }

}
