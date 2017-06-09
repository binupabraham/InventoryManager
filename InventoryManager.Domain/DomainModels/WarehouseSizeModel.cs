using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Domain.DomainModels
{
    public class WarehouseSizeModel
    {
        [Required]
        [Range(1, double.MaxValue)]
        [Display(Name="Total Capacity (in sq metres)")]
        public double TotalCapacity { get; set; }
        [Required]
        [Display(Name = "Warning level (in sq metres)")]
        public double WarningLevel { get; set; }
        public double CurrentSize { get; set; }

    }
}
