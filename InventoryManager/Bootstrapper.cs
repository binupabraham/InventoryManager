using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using InventoryManager.Domain;
using InventoryManager.Repository.UnitofWork;
using InventoryManager.Repository;

namespace InventoryManager
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IInventoryDomain, InventoryDomain>();
            container.RegisterType<IUnitofWork, InventoryUOW>();
         
            return container;
        }
    }
}