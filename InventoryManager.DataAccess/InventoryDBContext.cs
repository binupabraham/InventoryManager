using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using InventoryManager.DataAccess.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace InventoryManager.DataAccess
{
    public class InventoryDBContext : DbContext
    {
        public InventoryDBContext() : base("InventoryDBContext")
        {
        }

        public DbSet<InventoryItem> InventoryItem { get; set; }
        public DbSet<InventoryItemStock> InventoryItemStock { get; set; }
        public DbSet<InventorySizeSpec> InventorySizeSpec { get; set; }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
