using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryManager.Domain.DomainModels;

namespace InventoryManager.UI.Models
{
    public class vmInventoryModel
    {
        public InventoryItemModel InventoryItemModel { get; set; }
        public bool WarningLimit { get; set; }
    }
}