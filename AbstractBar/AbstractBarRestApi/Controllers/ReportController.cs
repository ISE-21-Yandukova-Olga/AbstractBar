using AbstractBarService.BindingModels;
using AbstractBarService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbstractBarRestApi.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetStoragesLoad()
        {
            var list = _service.GetStoragesLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetCustomerRequests(ReportBindingModel model)
        {
            var list = _service.GetCustomerRequests(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveCotailPrice(ReportBindingModel model)
        {
            _service.SaveCoctailPrice(model);
        }

        [HttpPost]
        public void SaveStoragesLoad(ReportBindingModel model)
        {
            _service.SaveStoragesLoad(model);
        }

        [HttpPost]
        public void SaveCustomerRequests(ReportBindingModel model)
        {
            _service.SaveCustomerRequests(model);
        }
    }
}