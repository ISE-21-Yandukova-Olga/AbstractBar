using AbstracBarView;
using AbstractBarService;
using AbstractBarService.ImplementationsBD;
using AbstractBarService.ImplementationsList;
using AbstractBarService.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace AbstractBarView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerService, CustomerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IIngredientsService, IngredientsServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBarmenService, BarmenServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICoctailService, CoctailServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStorageService, StorageServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceBD>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}