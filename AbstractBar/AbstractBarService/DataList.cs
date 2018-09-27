using AbstractBarModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Customer> Customers { get; set; }

        public List<Ingredient> Ingredients { get; set; }

        public List<Barmen> Barmens { get; set; }

        public List<Request> Requests { get; set; }

        public List<Coctail> Coctails { get; set; }

        public List<CoctailIngredient> CoctailIngredients { get; set; }

        public List<Storage> Storages { get; set; }

        public List<StorageIngredient> StorageIngredients { get; set; }

        private DataListSingleton()
        {
            Customers = new List<Customer>();
            Ingredients = new List<Ingredient>();
            Barmens = new List<Barmen>();
            Requests = new List<Request>();
            Coctails = new List<Coctail>();
            CoctailIngredients = new List<CoctailIngredient>();
            Storages = new List<Storage>();
            StorageIngredients = new List<StorageIngredient>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}