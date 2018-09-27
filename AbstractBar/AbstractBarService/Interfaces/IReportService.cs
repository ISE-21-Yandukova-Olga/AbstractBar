using AbstractBarService.BindingModels;
using AbstractBarService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractBarService.Interfaces
{
    public interface IReportService
    {
        void SaveCoctailPrice(ReportBindingModel model);

        List<StoragesLoadViewModel> GetStoragesLoad();

        void SaveStoragesLoad(ReportBindingModel model);

        List<CustomerRequestsModel> GetCustomerRequests(ReportBindingModel model);

        void SaveCustomerRequests(ReportBindingModel model);
    }
}