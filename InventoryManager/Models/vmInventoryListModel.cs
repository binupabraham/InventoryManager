using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryManager.Domain.DomainModels;

namespace InventoryManager.UI.Models
{
    public class vmInventoryListModel
    {
        public IEnumerable<InventoryItemModel> InventoryItemModel { get; set; }
        public bool WarningLimit { get; set; }
        public double CurrentSize { get; set; }
        public double TotalCapacity { get; set; }
    }
}