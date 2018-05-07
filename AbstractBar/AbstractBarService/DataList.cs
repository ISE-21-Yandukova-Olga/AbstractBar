using AbstractBarModel;
using System.Collections.Generic;


namespace AbstractBarService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Customer> Customers { get; set; }

        public List<Ingredients> Ingredients { get; set; }

        public List<Barmen> Barmens { get; set; }

        public List<Request> Requests { get; set; }

        public List<Coctail> Coctails { get; set; }

        public List<IngredientsCoctail> CoctailIngredients { get; set; }

        public List<Storage> Storages { get; set; }

        public List<StorageIngredients> StorageIngredients { get; set; }

        private DataListSingleton()
        {
            Customers = new List<Customer>();
            Ingredients = new List<Ingredients>();
            Barmens = new List<Barmen>();
            Requests = new List<Request>();
            Coctails = new List<Coctail>();
            CoctailIngredients = new List<IngredientsCoctail>();
            Storages = new List<Storage>();
            StorageIngredients = new List<StorageIngredients>();
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
