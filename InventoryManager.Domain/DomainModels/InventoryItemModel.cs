using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Domain.DomainModels
{
    public class InventoryItemModel
    {
        public int ID { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name="Unit Item Size")]
        [Range(1, 100000)]
        public double ItemSize { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Quantity should be between 1 and 100")]
        public int Quantity { get; set; }

    }
}
