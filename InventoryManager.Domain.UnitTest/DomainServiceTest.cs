using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using InventoryManager.Repository.UnitofWork;
using InventoryManager.Repository;
using InventoryManager.Domain;
using InventoryManager.Domain.DomainModels;
using InventoryManager.DataAccess.Models;

namespace InventoryManager.Domain.UnitTest
{
    [TestClass]
    public class DomainServiceTest
    {
        [TestMethod]
        public void AddInventoryItemFailsWhenItemSizeMoreThanWarehouse()
        {
            var inventoryitemrepomock = new Mock<IInventoryRepository>();
            var inventorysizerepomock = new Mock<IInventoryLimitRepository>();
            var unitofworkmock = new Mock<IUnitofWork>();
            var item = new InventoryItemModel()
            {
                Code = "Test",
                Description = "Description",
                Quantity = 1,
                ItemSize = 100
            };
            var warehousesize = new InventorySizeSpec()
            {
                CurrentCapacity = 100,
                TotalCapacity = 150
            };
            unitofworkmock.Setup(x => x.InventoryLimitRepository).Returns(inventorysizerepomock.Object);
            unitofworkmock.Setup(x => x.InventoryRepository).Returns(inventoryitemrepomock.Object);
            unitofworkmock.Setup(x => x.InventoryLimitRepository.GetItems()).Returns(new List<InventorySizeSpec>(){warehousesize}.AsQueryable());
            var domainService = new InventoryDomain(unitofworkmock.Object);
            List<string> errors;
            var result = domainService.AddNewInventoryItem(item, out errors);
            Assert.IsFalse(result);
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void UpdateOnNonExistantItemThrowsException()
        {
            var unitofworkmock = new Mock<IUnitofWork>();
            var item = new InventoryItemModel()
            {
                Code = "Test",
                Description = "Description",
                Quantity = 1,
                ItemSize = 100
            };

            var domainService = new InventoryDomain(unitofworkmock.Object);
            List<string> errors;
            var result = domainService.UpdateExistingItem(item, out errors);
        }

        public void UpdateWarehouseLevelsFailsWithInvalidTotalSize()
        {
            var warehousesize = new InventorySizeSpec()
            {
                CurrentCapacity = 100,
                TotalCapacity = 150, 
                WarningCapacity = 50
            };
            var warehousemodel = new WarehouseSizeModel()
            {
                TotalCapacity = 50,
                WarningLevel = 25
            };
            var unitofworkmock = new Mock<IUnitofWork>();

            unitofworkmock.Setup(x => x.InventoryLimitRepository.GetItems()).Returns(new List<InventorySizeSpec>() { warehousesize }.AsQueryable());
            List<string> errors; 
            var domainService = new InventoryDomain(unitofworkmock.Object);
            var result = domainService.UpdateInventoryCapacityLevels(warehousemodel, out errors);
            Assert.IsFalse(result);
            Assert.AreEqual(1, errors.Count);
 
        }

        public void WarningLevelReachedSuccessfulTest()
        {
            var warehousesize = new InventorySizeSpec()
            {
                CurrentCapacity = 100,
                TotalCapacity = 150,
                WarningCapacity = 100
            };
            var unitofworkmock = new Mock<IUnitofWork>();

            unitofworkmock.Setup(x => x.InventoryLimitRepository.GetItems()).Returns(new List<InventorySizeSpec>() { warehousesize }.AsQueryable());
            List<string> errors;
            var domainService = new InventoryDomain(unitofworkmock.Object);
            var result = domainService.WarningLimitReached();
            Assert.IsTrue(result);

        }
    }
}
