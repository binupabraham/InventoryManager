using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Repository
{
    public interface IEntityRepository<T> 
    {
        IQueryable<T> GetItems();
        void Update(T item);
    }
}
