using AbstractBarModel;
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.ImplementationsBD
{
    public class CustomerServiceBD : ICustomerService
    {
        private AbstractDbContext context_;

        public CustomerServiceBD(AbstractDbContext context_)
        {
            this.context_ = context_;
        }

        public List<CustomerViewModel> GetList()
        {
            List<CustomerViewModel> result = context_.Customers
                .Select(rec => new CustomerViewModel
                {
                    Id = rec.Id,
                    CustomerFIO = rec.CustomerFIO
                })
                .ToList();
            return result;
        }

        public CustomerViewModel GetElement(int id)
        {
            Customer element = context_.Customers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CustomerViewModel
                {
                    Id = element.Id,
                    CustomerFIO = element.CustomerFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CustomerBindingModel model)
        {
            Customer element = context_.Customers.FirstOrDefault(rec => rec.CustomerFIO == model.CustomerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context_.Customers.Add(new Customer
            {
                CustomerFIO = model.CustomerFIO
            });
            context_.SaveChanges();
        }

        public void UpdElement(CustomerBindingModel model)
        {
            Customer element = context_.Customers.FirstOrDefault(rec =>
                                    rec.CustomerFIO == model.CustomerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context_.Customers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CustomerFIO = model.CustomerFIO;
            context_.SaveChanges();
        }

        public void DelElement(int id)
        {
            Customer element = context_.Customers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context_.Customers.Remove(element);
                context_.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}

