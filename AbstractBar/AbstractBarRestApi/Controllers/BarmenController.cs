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
    public class BarmenController : ApiController
    {
        private readonly IBarmenService _service;

        public BarmenController(IBarmenService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(BarmenBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(BarmenBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(BarmenBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}