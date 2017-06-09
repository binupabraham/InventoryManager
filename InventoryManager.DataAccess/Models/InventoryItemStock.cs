using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.DataAccess.Models
{
    public class InventoryItemStock
    {
        [Key]
        [ForeignKey("InventoryItem")]
        public int InventoryItemID { get; set; }

        public int Quantity { get; set; }
      
        public virtual InventoryItem InventoryItem { get; set; }
    }

}
