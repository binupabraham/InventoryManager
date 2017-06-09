using InventoryManager.DataAccess;
using InventoryManager.DataAccess.Models;
using System.Data.Entity;
using System.Linq;

namespace InventoryManager.Repository
{
    public class InventoryLimitRepository : IInventoryLimitRepository
    {
        private InventoryDBContext _dbContext;
        public InventoryLimitRepository(InventoryDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<InventorySizeSpec> GetItems()
        {
            return _dbContext.InventorySizeSpec;
        }

        public void Update(InventorySizeSpec item)
        {
            _dbContext.InventorySizeSpec.Attach(item);
            _dbContext.Entry(item).State = EntityState.Modified;
        }

    }
}
