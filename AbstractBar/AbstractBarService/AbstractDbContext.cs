﻿using AbstractBarModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService
{
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext() : base("AbstractDatab")
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Ingredient> Ingredients { get; set; }

        public virtual DbSet<Barmen> Barmens { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<Coctail> Coctails { get; set; }

        public virtual DbSet<CoctailIngredient> CoctailIngredients { get; set; }

        public virtual DbSet<Storage> Storages { get; set; }

        public virtual DbSet<StorageIngredient> StorageIngredients { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
                throw;
            }
        }
    }
}
    
