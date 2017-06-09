using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.DataAccess.Models;

namespace InventoryManager.Repository
{
    public interface IInventoryRepository : IEntityRepository<InventoryItem>
    {
        void Insert(InventoryItem item);
        void Delete(InventoryItem item);
        InventoryItem GetItem(int ID);
        IEnumerable<InventoryItem> GetItem(Expression<Func<InventoryItem, bool>> filter);
    }
}
