using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.Repository.UnitofWork;
using InventoryManager.DataAccess;

namespace InventoryManager.Repository.UnitofWork
{
    public class InventoryUOW : IUnitofWork
    {
        private InventoryDBContext _dbContext = new InventoryDBContext();
        private bool disposed = false;
        private IInventoryLimitRepository _inventorySize;
        private IInventoryRepository _inventoryItem;

        public IInventoryLimitRepository InventoryLimitRepository
        {
            get
            {
                return _inventorySize ?? new InventoryLimitRepository(_dbContext);
            }
            set
            {
                _inventorySize = value;
            }
         }
        public IInventoryRepository InventoryRepository
        {
            get
            {
                return _inventoryItem ?? new InventoryItemRepository(_dbContext);
            }
            set
            {
                _inventoryItem = value;
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
