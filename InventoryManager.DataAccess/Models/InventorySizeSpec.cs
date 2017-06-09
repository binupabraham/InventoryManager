using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InventoryManager.DataAccess.Models
{
    public class InventorySizeSpec
    {
        [Key]
        public int ID { get; set; }

        public double CurrentCapacity { get; set; }

        public double TotalCapacity { get; set; }

        public double WarningCapacity { get; set; }
    }
}
