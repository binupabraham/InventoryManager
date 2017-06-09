using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace InventoryManager.DataAccess.Models
{
    public class InventoryItem
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double ItemSize { get; set; }
        
        public virtual InventoryItemStock InventoryItemStock { get; set; }
    }
}
