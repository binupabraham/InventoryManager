using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using InventoryManager.DataAccess.Models;

namespace InventoryManager.DataAccess
{
    public class InventoryInitialiser : DropCreateDatabaseIfModelChanges<InventoryDBContext>
    {
        protected override void Seed(InventoryDBContext context)
        {
            var items = new List<InventoryItem>(){
                new InventoryItem(){ Code = "INV001", Description = "Electrical", ItemSize = 100, InventoryItemStock = new InventoryItemStock(){
                Quantity = 1
                }},
                new InventoryItem(){ Code = "INV002", Description = "Computer", ItemSize = 50, InventoryItemStock = new InventoryItemStock(){
                Quantity = 1
                }},
                new InventoryItem(){Code = "INV003", Description = "Plumbing", ItemSize = 50, InventoryItemStock = new InventoryItemStock(){
                Quantity = 1
                }}
            };

            context.InventoryItem.AddRange(items);
            context.SaveChanges();
           
            var limits = new InventorySizeSpec()
            {
                CurrentCapacity = 200,
                TotalCapacity = 500,
                WarningCapacity = 400
            };

            context.InventorySizeSpec.Add(limits);
            context.SaveChanges();
        }
    }
}