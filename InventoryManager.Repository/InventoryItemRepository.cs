using InventoryManager.DataAccess;
using InventoryManager.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace InventoryManager.Repository
{
    public class InventoryItemRepository : IInventoryRepository
    {
        private InventoryDBContext _dbContext;
        public InventoryItemRepository(InventoryDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<InventoryItem> GetItems()
        {
            return _dbContext.InventoryItem.Include(x => x.InventoryItemStock);
        }

        public InventoryItem GetItem(int ID)
        {
            return _dbContext.InventoryItem.Find(ID);
        }

        public IEnumerable<InventoryItem> GetItem(Expression<Func<InventoryItem, bool>> filter)
        {
            return _dbContext.InventoryItem.Where(filter).ToList();
        }

        public void Insert(InventoryItem item)
        {
            _dbContext.InventoryItem.Add(item);
        }
        public void Update(InventoryItem item)
        {
            _dbContext.InventoryItem.Attach(item);
            _dbContext.Entry(item).State = EntityState.Modified; 
        }
        public void Delete(InventoryItem item)
        {
            if (_dbContext.Entry(item).State == EntityState.Detached)
            {
                _dbContext.InventoryItem.Attach(item);
            }

            _dbContext.InventoryItem.Remove(item);
        }
 
    } 
}
