using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using InventoryManager.Repository;

namespace InventoryManager.Repository.UnitofWork
{
    public interface IUnitofWork : IDisposable
    {
        void Save();
        IInventoryRepository InventoryRepository { get; }
        IInventoryLimitRepository InventoryLimitRepository { get; }
    }
}
