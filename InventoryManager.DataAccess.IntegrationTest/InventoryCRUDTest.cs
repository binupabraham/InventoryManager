using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventoryManager.Repository.UnitofWork;
using InventoryManager.DataAccess.Models;

namespace InventoryManager.DataAccess.IntegrationTest
{
    [TestClass]
    public class InventoryCRUDTest
    {
        [TestMethod]
        public void InsertNewInventoryItemTest()
        {
            var newItem = new InventoryItem()
            {
                Code = "InsertCode",
                Description = "TestDescription",
                InventoryItemStock = new InventoryItemStock() { Quantity = 1 },
                ItemSize = 100
            };

            var dao = new InventoryUOW();
            dao.InventoryRepository.Insert(newItem);
            dao.Save();
            var savedItem = dao.InventoryRepository.GetItem(x => x.Code == "InsertCode").FirstOrDefault();
            Assert.IsNotNull(savedItem);
            Assert.AreEqual("InsertCode", savedItem.Code);
            Assert.AreEqual("TestDescription", savedItem.Description);
            Assert.AreEqual(100, savedItem.ItemSize);
            dao.InventoryRepository.Delete(savedItem);
            dao.Save();
        }

        [TestMethod]
        public void UpdateExistingInventoryItemTest()
        {
            var newItem = new InventoryItem()
            {
                Code = "UpdateCode",
                Description = "TestDescription",
                InventoryItemStock = new InventoryItemStock() { Quantity = 1 },
                ItemSize = 100
            };

            var dao = new InventoryUOW();
            dao.InventoryRepository.Insert(newItem);
            dao.Save();
            var savedItem = dao.InventoryRepository.GetItem(x => x.Code == "UpdateCode").FirstOrDefault();
            Assert.IsNotNull(savedItem);
            savedItem.Description = "Updated description";
            savedItem.InventoryItemStock.Quantity = 2;
            int savedID = savedItem.ID;
            dao.InventoryRepository.Update(savedItem);
            dao.Save();
            var updatedItem = dao.InventoryRepository.GetItem(savedID);
            Assert.AreEqual("Updated description", updatedItem.Description);
            Assert.AreEqual(2, updatedItem.InventoryItemStock.Quantity);

            dao.InventoryRepository.Delete(updatedItem);
            dao.Save();
        }

        [TestMethod]
        public void DeleteExistingInventoryItemTest()
        {
            var newItem = new InventoryItem()
            {
                Code = "DeleteItem",
                Description = "TestDescription",
                InventoryItemStock = new InventoryItemStock() { Quantity = 1 },
                ItemSize = 100
            };

            var dao = new InventoryUOW();
            dao.InventoryRepository.Insert(newItem);
            dao.Save();
            var savedItem = dao.InventoryRepository.GetItem(x => x.Code == "DeleteItem").FirstOrDefault();
            Assert.IsNotNull(savedItem);
            dao.InventoryRepository.Delete(savedItem);
            dao.Save();
            var deletedItem = dao.InventoryRepository.GetItem(savedItem.ID);
            Assert.IsNull(deletedItem);
        }
    }
}
