using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.Repository.UnitofWork;
using InventoryManager.Domain.DomainModels;
using InventoryManager.DataAccess.Models;

namespace InventoryManager.Domain
{
    public class InventoryDomain : IInventoryDomain
    {
        private IUnitofWork _inventoryWork;
        public InventoryDomain(IUnitofWork inventoryWork)
        {
             _inventoryWork = inventoryWork;
        }

        public bool AddNewInventoryItem(InventoryItemModel item, out List<string> errors)
        {
            // get current warehouse size, total warehouse size
            // check if quantity * item size > total warehouse size
            // current warehouse size = current warehouse size + (quantity *item size)
            // save item, update current warehouse size.
            errors = new List<string>();
            var warehousesize = _inventoryWork.InventoryLimitRepository.GetItems().ToList().FirstOrDefault();
            if (warehousesize != null && warehousesize.TotalCapacity > 0)
            {
                var itemsize = Math.Round(item.ItemSize, 2) * item.Quantity;
                if (item.ID == 0) //insert
                {
                    var inventoryItem = new InventoryItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Description = item.Description,
                            ItemSize = item.ItemSize,
                            InventoryItemStock = new InventoryItemStock(){ Quantity = item.Quantity}
                        };

                    if (warehousesize.TotalCapacity >= (warehousesize.CurrentCapacity + itemsize))
                    {
                        var isitemexisting = _inventoryWork.InventoryRepository.GetItem(x => x.Code.Equals(item.Code));
                        if (isitemexisting.Count() > 0)
                        {
                            errors.Add("Item with the same code exists in the system");
                            return false;
                        }
                        _inventoryWork.InventoryRepository.Insert(inventoryItem);
                        warehousesize.CurrentCapacity = warehousesize.CurrentCapacity + itemsize;
                        _inventoryWork.InventoryLimitRepository.Update(warehousesize);
                        _inventoryWork.Save();
                    }
                    else
                    {
                        errors = new List<string>() { "Item size exceeds warehouse size." };
                        return false;
                    }
                }
                
            }
            else
            {
                throw new ApplicationException("Total Warehouse size is not set.");
            }

            return true;

        }

        public bool UpdateExistingItem(InventoryItemModel item, out List<string> errors)
        {
            errors = new List<string>();
            if (item.ID > 0)
            {
                var existingItem = _inventoryWork.InventoryRepository.GetItem(item.ID);
                var warehousesize = _inventoryWork.InventoryLimitRepository.GetItems().ToList().FirstOrDefault();
                // reduce existing item size from current capacity
                // check if totalcapacity >= currentcapacity + (itemsize*quantity)
                var itemsize = Math.Round(item.ItemSize, 2) * item.Quantity;
                
                var currentcapacity = warehousesize.CurrentCapacity - (existingItem.ItemSize * existingItem.InventoryItemStock.Quantity);
                if (warehousesize.TotalCapacity >= (currentcapacity + (item.ItemSize * item.Quantity)))
                {
                    existingItem.ItemSize = item.ItemSize;
                    existingItem.Description = item.Description;
                    existingItem.InventoryItemStock.Quantity = item.Quantity;
                    warehousesize.CurrentCapacity = currentcapacity + (item.ItemSize * item.Quantity);
                    _inventoryWork.InventoryLimitRepository.Update(warehousesize);
                    _inventoryWork.Save();
                }
                else
                {
                    errors.Add("Warehouse does not have space to accomodate this item");
                    return false;
                }

            }
            else
            {
                throw new ApplicationException("Update Method called on non existing item");
            }
            return true;
        }

        public bool DeleteInventoryItem(int ID)
        {
            var item = _inventoryWork.InventoryRepository.GetItem(ID);
            if (item == null || item.ID > 0)
            {
                var existingItem = _inventoryWork.InventoryRepository.GetItem(item.ID);
                var warehousesize = _inventoryWork.InventoryLimitRepository.GetItems().ToList().FirstOrDefault();
                warehousesize.CurrentCapacity = warehousesize.CurrentCapacity - (existingItem.ItemSize * existingItem.InventoryItemStock.Quantity);
                _inventoryWork.InventoryRepository.Delete(existingItem);
                _inventoryWork.InventoryLimitRepository.Update(warehousesize);
                _inventoryWork.Save();
            }
            else
            {
                throw new ApplicationException("Attempt to delete non existing item");
            }

            return true;
        }

        public IEnumerable<InventoryItemModel> GetListofItems()
        {
            var listofitems = _inventoryWork.InventoryRepository.GetItems().ToList();
            var vmlistofitems = new List<InventoryItemModel>();
            listofitems.ForEach(x => vmlistofitems.Add(new InventoryItemModel()
            {
                Code = x.Code,
                ID = x.ID,
                Description = x.Description,
                ItemSize = x.ItemSize,
                Quantity = x.InventoryItemStock.Quantity
            }));

            return vmlistofitems;
        }

        public InventoryItemModel GetItem(int ID)
        {
            var item = _inventoryWork.InventoryRepository.GetItem(ID);
            if (item == null)
            {
                return null;
            }

            return new InventoryItemModel()
            {
                ID = item.ID,
                Code = item.Code,
                Description = item.Description,
                ItemSize = item.ItemSize,
                Quantity = item.InventoryItemStock.Quantity
            };
        }

        public bool UpdateInventoryCapacityLevels(WarehouseSizeModel model, out List<string> errors)
        {
            errors = new List<string>();
            var levels =_inventoryWork.InventoryLimitRepository.GetItems().FirstOrDefault();  
            if (model.WarningLevel > model.TotalCapacity)
            {
                errors.Add("Warning level cannot be greater than total capacity of the warehouse");
            }
            if (model.TotalCapacity < levels.CurrentCapacity)
            {
                errors.Add("Current size of the warehouse is greater than the intended total size");
            }
            if (errors.Any())
            {
                return false;
            }
            else
            {
                levels.TotalCapacity = model.TotalCapacity;
                levels.WarningCapacity = model.WarningLevel;
                _inventoryWork.InventoryLimitRepository.Update(levels);
                _inventoryWork.Save();
                return true;
            }
        }

        public WarehouseSizeModel GetWarehouseLimits()
        {
            var result = _inventoryWork.InventoryLimitRepository.GetItems().FirstOrDefault();
            if (result == null)
            {
                throw new ApplicationException("Warehouse size not defined");
            }

            return new WarehouseSizeModel() { TotalCapacity = result.TotalCapacity, WarningLevel = result.WarningCapacity, CurrentSize = result.CurrentCapacity };
        }

        public bool WarningLimitReached()
        {
            var result =_inventoryWork.InventoryLimitRepository.GetItems().FirstOrDefault();
            return result.CurrentCapacity >= result.WarningCapacity;
        }


        public void DisposeUnitOfWork()
        {
            _inventoryWork.Dispose();
        }
    }
}
