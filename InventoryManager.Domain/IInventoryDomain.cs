using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.Domain.DomainModels;

namespace InventoryManager.Domain
{
    public interface IInventoryDomain
    {
        IEnumerable<InventoryItemModel> GetListofItems();
        InventoryItemModel GetItem(int ID);
        bool UpdateExistingItem(InventoryItemModel item, out List<string> errors);
        bool AddNewInventoryItem(InventoryItemModel item, out List<string> errors);
        bool DeleteInventoryItem(int ID);
        bool UpdateInventoryCapacityLevels(WarehouseSizeModel model, out List<string> errors);
        WarehouseSizeModel GetWarehouseLimits();
        bool WarningLimitReached();
        void DisposeUnitOfWork();


    }
}
