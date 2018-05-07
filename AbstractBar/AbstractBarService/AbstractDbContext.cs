using AbstractBarModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AbstractBarService
{
    [Table("AbstractDatabase")]
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext()
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Ingredients> Ingredients { get; set; }

        public virtual DbSet<Barmen> Barmens { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<Coctail> Coctails { get; set; }

        public virtual DbSet<IngredientsCoctail> CoctailIngredients { get; set; }

        public virtual DbSet<Storage> Storages { get; set; }

        public virtual DbSet<StorageIngredients> StorageIngredients { get; set; }
    }
}