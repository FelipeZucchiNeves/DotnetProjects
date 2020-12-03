using System;

namespace Rdi.Core.Data
{
    public interface IRepository<T> : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }
}