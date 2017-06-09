using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.DataAccess.Models;
namespace InventoryManager.Repository
{
    public interface IInventoryLimitRepository : IEntityRepository<InventorySizeSpec>
    {
    }
}
