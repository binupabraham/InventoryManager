using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryManager.Domain;
using InventoryManager.Domain.DomainModels;
using InventoryManager.Repository.UnitofWork;
using System.Data.Entity;
using InventoryManager.UI.Models;

namespace InventoryManager.Controllers
{
    public class InventoryController : Controller
    {
        private IInventoryDomain _domainService;
        
        public InventoryController(IInventoryDomain domainService)
        {
            _domainService = domainService;
        }
        public ActionResult Index()
        {
            var result = _domainService.GetListofItems();
            var limit = _domainService.WarningLimitReached();
            var warehousesize = _domainService.GetWarehouseLimits();
            var model = new vmInventoryListModel() { InventoryItemModel = result, WarningLimit = limit, CurrentSize = warehousesize.CurrentSize, TotalCapacity = warehousesize.TotalCapacity };
            return View(model);
        }

        [HttpGet]
        public ActionResult AddItem()
        {
            var limit = _domainService.WarningLimitReached();
            return View("AddItem", new vmInventoryModel() { WarningLimit = limit, InventoryItemModel = new InventoryItemModel() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(vmInventoryModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  List<string> errors;
                  var success = _domainService.AddNewInventoryItem(item.InventoryItemModel, out errors);
                    if (!success)
                    {
                        errors.ForEach(x => ModelState.AddModelError(string.Empty, x));
                        return View(item);
                    }
                    
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log. 
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(item);
        } 
      
        [HttpGet]
        public ActionResult EditItem(int id)
        {
            var model =_domainService.GetItem(id);
            var limit = _domainService.WarningLimitReached();
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "This item does not exist in the warehouse");
            }
            return View(new vmInventoryModel() { InventoryItemModel = model, WarningLimit= limit});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(vmInventoryModel item)
        {
            List<string> errors;
            var success =_domainService.UpdateExistingItem(item.InventoryItemModel, out errors);
            if (!success)
            {
                errors.ForEach(x => ModelState.AddModelError(string.Empty, x));
                return View("EditInventory", item);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteItem(int id)
        {
            var model =_domainService.GetItem(id);
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "This item does not exist in the warehouse");
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(InventoryItemModel item)
        {
            _domainService.DeleteInventoryItem(item.ID);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SetWarehouseLimit()
        {
            var model = _domainService.GetWarehouseLimits();
            return View(model);
        }

        [HttpPost]
        public ActionResult SetWarehouseLimit(WarehouseSizeModel model)
        {
            List<string> errors;
            var result = _domainService.UpdateInventoryCapacityLevels(model, out errors);
            if (errors.Any())
            {
                errors.ForEach(x => ModelState.AddModelError(string.Empty, x));
                return View(model);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _domainService.DisposeUnitOfWork();
            base.Dispose(disposing);
        } 
    }
}